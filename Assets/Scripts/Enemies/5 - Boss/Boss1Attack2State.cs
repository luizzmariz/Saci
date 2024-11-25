using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Attack2State : BaseState
{
    Boss1StateMachine enemyStateMachine;

    int attackIndex;
    bool hasAttacked;

    public Boss1Attack2State(Boss1StateMachine stateMachine) : base("Attack2", stateMachine)
    {
        enemyStateMachine = stateMachine;

        attackIndex = 0;
    }

    public override void Enter() 
    {
        enemyStateMachine.rigidBody.velocity = Vector3.zero;

        // enemyStateMachine.canMove = false;
        //enemyStateMachine.canDoAttack2 = false;
        enemyStateMachine.enemyDamageable.damageable = true;
        enemyStateMachine.isAttacking = true;
        enemyStateMachine.isPerformingAttack2 = true;
        hasAttacked = false;
    }

    public override void UpdateLogic() 
    {
        if(!enemyStateMachine.isAttacking)
        {
            // if(enemyStateMachine.playerGameObject.GetComponent<PlayerStateMachine>().currentState == enemyStateMachine.playerGameObject.GetComponent<PlayerStateMachine>().deadState)
            // {
            //     stateMachine.ChangeState(enemyStateMachine.idleState);
            // }
            
            // holderPosition = enemyStateMachine.transform.position;
            // playerPosition = enemyStateMachine.playerGameObject.transform.position;

            // if(Vector3.Distance(holderPosition, playerPosition) > enemyStateMachine.rangeOfAttack)
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
            enemyStateMachine.enemyAbilityHolder.UseAbility(enemyStateMachine.attack2);
            enemyStateMachine.animator.SetTrigger("Attack1");

            attackIndex++;
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
        enemyStateMachine.enemyAbilityHolder.EndAbility(enemyStateMachine.attack2);

        if(attackIndex >= 3)
        {
            attackIndex = 0;
            enemyStateMachine.isPerformingAttack2 = false;
        }
        //enemyStateMachine.canMove = true;
        // hasAttacked = false;
        // enemyStateMachine.StartCoroutine(enemyStateMachine.Cooldown("attack2"));
    }
}
