using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1IdleState : BaseState
{
    Boss1StateMachine enemyStateMachine;

    public Boss1IdleState(Boss1StateMachine stateMachine) : base("Idle", stateMachine) {
        enemyStateMachine = stateMachine;
    }

    public override void Enter() {
        //enemyStateMachine.canMove = true;
        // enemyStateMachine.canAttack = true;
        //enemyStateMachine.enemyDamageable.damageable = true;
    }

    public override void UpdateLogic() {
        if(!(enemyStateMachine.playerGameObject.GetComponent<PlayerStateMachine>().currentState == enemyStateMachine.playerGameObject.GetComponent<PlayerStateMachine>().deadState))
        {
            Vector3 holderPosition = enemyStateMachine.transform.position;
            Vector3 playerPosition = enemyStateMachine.playerGameObject.transform.position;
            
            // if(Vector3.Distance(holderPosition, playerPosition) <= enemyStateMachine.rangeOfAttack)
            // {
            //     if(enemyStateMachine.canAttack)
            //     {
            //         stateMachine.ChangeState(enemyStateMachine.attackState);
            //     }
            //     enemyStateMachine.characterOrientation.ChangeOrientation(playerPosition);
            // }
            // else if(Vector3.Distance(holderPosition, playerPosition) <= enemyStateMachine.rangeOfView)
            // {
            //     stateMachine.ChangeState(enemyStateMachine.chaseState);
            // }

            ChooseNextAttack(holderPosition, playerPosition);
        }
    }

    public void ChooseNextAttack(Vector3 holderPosition, Vector3 playerPosition)
    {
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

    public override void UpdatePhysics() {
        enemyStateMachine.rigidBody.velocity = Vector3.zero;
    }

    public override void Exit() 
    {
        
    }
}
