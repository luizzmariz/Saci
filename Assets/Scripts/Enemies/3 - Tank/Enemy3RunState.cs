using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy3RunState : BaseState
{
    Enemy3StateMachine enemyStateMachine;

    Vector3 holderPosition;
    Vector3 playerPosition;

    bool hasStartedRunning;
    Coroutine run;
    bool isRunning;

    // List<Collider> scenarioColliders;
    Collider[] scenarioColliders;
    //ContactFilter2D scenarioContactFilter2D;
    Collider[] damageableCollider;
    //List<Collider> damageableCollider;
    List<Collider> usedDamageableCollider;
    //ContactFilter2D damageableContactFilter2D;
    

    public Enemy3RunState(Enemy3StateMachine stateMachine) : base("Run", stateMachine) 
    {
        enemyStateMachine = stateMachine;

        //scenarioColliders = new List<Collider>();

        // scenarioContactFilter2D = new ContactFilter2D
        // {
        //     layerMask = LayerMask.GetMask("Collision"),
        //     useLayerMask = true
        // };

        //damageableCollider = new List<Collider>();

        // damageableContactFilter2D = new ContactFilter2D
        // {
        //     layerMask = LayerMask.GetMask("Damageable"),
        //     useLayerMask = true
        // };
    }

    public override void Enter() 
    {
        enemyStateMachine.enemyDamageable.damageable = false;
        isRunning = true;
        //enemyStateMachine.runCollider.enabled = true;
        enemyStateMachine.runSprite.enabled = true;
    }

    public override void UpdateLogic() 
    {
        if(!isRunning)
        {
            holderPosition = enemyStateMachine.transform.position;
            playerPosition = enemyStateMachine.playerGameObject.transform.position;
            
            if(Vector3.Distance(holderPosition, playerPosition) <= enemyStateMachine.rangeOfAttack)
            {
                if(enemyStateMachine.canAttack)
                {
                    stateMachine.ChangeState(enemyStateMachine.attackState);
                }
                enemyStateMachine.characterOrientation.ChangeOrientation(playerPosition);
            }
            else if(Vector3.Distance(holderPosition, playerPosition) <= enemyStateMachine.rangeOfView)
            {
                stateMachine.ChangeState(enemyStateMachine.chaseState);
            }
        }
    }

    public override void UpdatePhysics() 
    {
        if(!hasStartedRunning)
        {
            StartRun();
        }
        else
        {
            scenarioColliders = Physics.OverlapSphere(enemyStateMachine.transform.position, enemyStateMachine.runColliderRadius * 0.75f, LayerMask.GetMask("Collision"));
            if(//enemyStateMachine.GetComponent<Collider>().OverlapCollider(scenarioContactFilter2D, scenarioColliders) > 0
            scenarioColliders.Count() > 0 
            )
            {
                //Debug.Log("SCENARIO COLLIDED");
                enemyStateMachine.StopCoroutine(run);
                enemyStateMachine.rigidBody.velocity = Vector3.zero;
                hasStartedRunning = false;
                isRunning = false;
            }
            damageableCollider = Physics.OverlapSphere(enemyStateMachine.transform.position, enemyStateMachine.runColliderRadius, LayerMask.GetMask("Damageable"));
            if(//enemyStateMachine.runCollider.OverlapCollider(damageableContactFilter2D, damageableCollider) > 0
            damageableCollider.Count() > 0
            )
            {
                foreach(Collider collider in damageableCollider)
                {
                    if(!usedDamageableCollider.Contains(collider))
                    {
                        //Debug.Log("DAMAGEABLE COLLIDED");
                        if(collider.GetComponent<PlayerDamageable>())
                        {
                            collider.GetComponent<PlayerDamageable>().Damage(enemyStateMachine.runDamage, enemyStateMachine.transform.position);
                            enemyStateMachine.StopCoroutine(run);
                            enemyStateMachine.rigidBody.velocity = Vector3.zero;
                            hasStartedRunning = false;
                            isRunning = false;
                        }
                    }
                    usedDamageableCollider.Add(collider);
                }
            }
        }
    }

    void StartRun()
    {
        hasStartedRunning = true;
        run = enemyStateMachine.StartCoroutine(RunTimer());
        //enemyStateMachine.animator;
        usedDamageableCollider = new List<Collider>();

        holderPosition = enemyStateMachine.transform.position;
        playerPosition = enemyStateMachine.playerGameObject.transform.position;

        Vector3 runVector = (playerPosition - holderPosition).normalized;
        enemyStateMachine.rigidBody.velocity = runVector * enemyStateMachine.runSpeed;
    }

    IEnumerator RunTimer()
    {
        yield return new WaitForSeconds(enemyStateMachine.maxRunDuration);
        //enemyStateMachine.animator;
        enemyStateMachine.rigidBody.velocity = Vector3.zero;
        hasStartedRunning = false;
        isRunning = false;
    }

    public override void Exit() 
    {
        //enemyStateMachine.runCollider.enabled = false;
        enemyStateMachine.runSprite.enabled = false;
        enemyStateMachine.rigidBody.velocity = Vector3.zero;
        enemyStateMachine.enemyDamageable.damageable = true;
        enemyStateMachine.canRun = false;
        usedDamageableCollider = null;
        enemyStateMachine.StartCoroutine(enemyStateMachine.Cooldown("run"));
    }
}
