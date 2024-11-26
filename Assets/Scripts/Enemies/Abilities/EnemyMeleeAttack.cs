using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyMeleeAttack", menuName = "Data/Enemy/Abilities/MeleeAttack")]
public class EnemyMeleeAttack : EnemyAbility
{
    [Header("Enemy Melee Attack Statistics")]
    [SerializeField] GameObject meleeBase;
    [HideInInspector] Vector3 meleeBaseLocalPosition;
    [SerializeField] List<MeleeAttack> attacks;

    int attacksDealt = 0; 
    Coroutine nextAttackCooldown;
    
    public override void OnEnable()
    {
        base.OnEnable();
        attacksDealt = 0;
    }

    public override void Activate()
    {
        base.Activate();
        GetComponents();

        Vector3 targetPoint = GetTarget();
        ChangeEnemyOrientation(targetPoint);

        SetAttackProperties(targetPoint);
        StartAttack();
    }

    protected override void GetComponents()
    {
        base.GetComponents();

        if(meleeBase == null)
        {
            meleeBase = enemy.transform.Find("Hands").GetChild(0).gameObject;
            meleeBaseLocalPosition = meleeBase.transform.localPosition;
        }
    }

    Vector3 GetTarget()
    {
        Vector3 targetPoint = player.transform.position;
        return targetPoint;
    }

    public void StartAttack()
    {
        meleeBase.SetActive(true);
        attacksDealt++;

        enemyStateMachine.ChangeToAttackState(this);
    }

    void SetAttackProperties(Vector3 targetPoint)
    {
        Attack actualAttack = meleeBase.GetComponent<Attack>();
        meleeBaseLocalPosition = meleeBase.transform.localPosition;

        MeleeAttack currentAttackInformation = attacks[attacksDealt];

        //attack spawn offset
        meleeBase.transform.localPosition = new Vector3(meleeBaseLocalPosition.x, meleeBaseLocalPosition.y, meleeBaseLocalPosition.z -currentAttackInformation.attackSpawnOffset);

        //attack damage
        actualAttack.damageAmount = currentAttackInformation.damage;
        
        //attack collision radius
        actualAttack.GetComponent<CapsuleCollider>().radius = currentAttackInformation.radius;
        
        //attack movement
        Vector3 movementDirection = targetPoint - enemy.transform.position;
        enemyStateMachine.movementVector = movementDirection.normalized * currentAttackInformation.moveSpeed;
    }

    void ChangeEnemyOrientation(Vector3 targetPoint)
    {
        enemyOrientation.ChangeOrientation(targetPoint);
    }

    public override void Deactivate()
    {
        meleeBase.SetActive(false);
        meleeBase.transform.localPosition = meleeBaseLocalPosition;

        if(attacksDealt < attacks.Count)
        {
            if(nextAttackCooldown == null)
            {
                nextAttackCooldown = enemyStateMachine.StartCoroutine(cooldownBeforeNextAttack());
            }
            else
            {
                enemyStateMachine.StopCoroutine(nextAttackCooldown);
                enemyStateMachine.StartCoroutine(enemy.GetComponent<EnemyAbilityHolder>().Cooldown(this));
            }
        }
        else
        {
            attacksDealt = 0;
            enemyStateMachine.StartCoroutine(enemy.GetComponent<EnemyAbilityHolder>().Cooldown(this));
        }
    }

    IEnumerator cooldownBeforeNextAttack()
    {
        yield return new WaitForSeconds(attacks[attacksDealt-1].timeInSecondsBeforeNextAttack);

        nextAttackCooldown = null;
        Activate();
    }

    [Serializable] public struct MeleeAttack 
	{
		public int damage;
        public float attackSpawnOffset;
        public float radius;

        public bool moveCharacter;
        public float moveSpeed;

        public float timeInSecondsBeforeNextAttack;

		public MeleeAttack(int _damage, float _radius, bool _moveCharacter, float _moveSpeed, float _attackSpawnOffset, float _timeInSecondsBeforeNextAttack) 
		{
			damage = _damage;
            attackSpawnOffset = _attackSpawnOffset;
            radius = _radius;

            moveCharacter = _moveCharacter;
            moveSpeed = _moveSpeed;

            timeInSecondsBeforeNextAttack = _timeInSecondsBeforeNextAttack;
		}
	}
}
