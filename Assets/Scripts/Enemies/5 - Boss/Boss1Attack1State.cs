using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Attack1State : BaseState
{
    Boss1StateMachine enemyStateMachine;

    Vector3 holderPosition;
    Vector3 playerPosition;

    bool hasAttacked;

    public Boss1Attack1State(Boss1StateMachine stateMachine) : base("Attack1", stateMachine)
    {
        enemyStateMachine = stateMachine;
    }

    public override void Enter() 
    {
        enemyStateMachine.rigidBody.velocity = enemyStateMachine.movementVector;

        // enemyStateMachine.canMove = false;
        //enemyStateMachine.canDoAttack1 = false;
        enemyStateMachine.enemyDamageable.damageable = true;
        enemyStateMachine.isAttacking = true;
        hasAttacked = false;
    }

    public override void UpdateLogic() {
        if(!enemyStateMachine.isAttacking)
        {
            // if(enemyStateMachine.playerGameObject.GetComponent<PlayerStateMachine>().currentState == enemyStateMachine.playerGameObject.GetComponent<PlayerStateMachine>().deadState)
            // {
            //     stateMachine.ChangeState(enemyStateMachine.idleState);
            // }
            
            // holderPosition = enemyStateMachine.transform.position;
            // playerPosition = enemyStateMachine.playerGameObject.transform.position;

            // if(Vector3.Distance(holderPosition, playerPosition) > enemyStateMachine.attack1Range)
            // {
            //     if(Vector3.Distance(holderPosition, playerPosition) <= enemyStateMachine.rangeOfView)
            //     {
            //         stateMachine.ChangeState(enemyStateMachine.chaseState);
            //     }
            //     else
            //     {
            //         stateMachine.ChangeState(enemyStateMachine.idleState);
            //     }
            // }
            // else
            // {
            //     stateMachine.ChangeState(enemyStateMachine.idleState);
            // }
            stateMachine.ChangeState(enemyStateMachine.idleState);
        }
    }

    public override void UpdatePhysics() 
    {
        if(!hasAttacked)
        {
            //enemyStateMachine.enemyAbilityHolder.UseAbility(enemyStateMachine.attack1);
            enemyStateMachine.animator.SetTrigger("Attack1");

            hasAttacked = true;
        }

        // if(!hasAttacked)
        // {
        //     holderPosition = enemyStateMachine.transform.position;
        //     playerPosition = enemyStateMachine.playerGameObject.transform.position;

        //     Vector3 attackDirection = playerPosition - holderPosition;
        //     enemyStateMachine.enemyHands.Attack(attackDirection);
        //     hasAttacked = true;

        //     // enemyStateMachine.isAttacking = false;
        // }
    }

    public override void Exit() 
    {
        enemyStateMachine.rigidBody.velocity = Vector3.zero;
        enemyStateMachine.enemyAbilityHolder.EndAbility(enemyStateMachine.attack1);
        
        // enemyStateMachine.canMove = true;
        //hasAttacked = false;
        //enemyStateMachine.StartCoroutine(enemyStateMachine.Cooldown("attack1"));
    }
}
