using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHands : MonoBehaviour
{
    [SerializeField] public Transform attacksParentGameObject;
    [SerializeField] public Animator handsAnimator;

    EnemyStateMachine enemyStateMachine;

    [SerializeField] List<GameObject> attacks;
    public Attack actualAttack;

    void Awake()
    {
        enemyStateMachine = GetComponentInParent<EnemyStateMachine>(); 
        attacksParentGameObject = GameObject.Find("InstantiatedObjects").transform;
        //handsAnimator = GetComponent<Animator>(); 
    }

    public void Attack(Vector3 attackDirection)
    {
        if(attacks.Count <= 0)
        {
            Debug.Log("There isn't any attacks settled");
        }
        else if(attacks[0].GetComponent<Attack>().isProjectile)
        {
            GameObject projectile = Instantiate(attacks[0], transform.position, Quaternion.identity, attacksParentGameObject);
            
            actualAttack = projectile.GetComponent<Attack>();
            projectile.GetComponent<Rigidbody>().AddForce(attackDirection * actualAttack.fireForce, ForceMode.Impulse);
            Destroy(projectile, actualAttack.projectileDuration);
        }
        else
        {
            attacks[0].SetActive(true);
            actualAttack = attacks[0].GetComponent<Attack>();
        }
        
        handsAnimator.SetTrigger("Attack");
        actualAttack.ExecuteAttack();
    }

    // public void Attack(AttackType attackType, int attackIndex)
    // {
    //     if(attackType == AttackType.Projectile)
    //     {
    //         Instantiate(projectileAttacks[attackIndex-1], transform.position, Quaternion.identity, attackdParentGameObject);
    //     }
    // }

    public void AttackEnd()
    {
        if(actualAttack != null && !actualAttack.isProjectile)
        {
            actualAttack.StopAttack();
        }
    
        //enemyStateMachine.isAttacking = false;
    }
}
