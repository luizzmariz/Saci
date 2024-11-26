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
        //Debug.Log("teste");
    }

    public override void UpdateLogic() {
        if(enemyStateMachine.battleStarted)
        {
            if(!(enemyStateMachine.playerGameObject.GetComponent<PlayerStateMachine>().currentState == enemyStateMachine.playerGameObject.GetComponent<PlayerStateMachine>().deadState))
            {
                Vector3 holderPosition = enemyStateMachine.transform.position;
                Vector3 playerPosition = enemyStateMachine.playerGameObject.transform.position;

                ChooseNextAttack(holderPosition, playerPosition);
            }
        }
    }

    public void ChooseNextAttack(Vector3 holderPosition, Vector3 playerPosition)
    {
        //enemyStateMachine.characterOrientation.ChangeOrientation(playerPosition);
        
        if(Vector3.Distance(holderPosition, playerPosition) <= enemyStateMachine.attack1Range && !enemyStateMachine.isPerformingAttack2)
        {
            if(enemyStateMachine.enemyAbilityHolder.CheckIfCanUseAbility(enemyStateMachine.attack1))
            {
                enemyStateMachine.enemyAbilityHolder.UseAbility(enemyStateMachine.attack1);
                //stateMachine.ChangeState(enemyStateMachine.attack1State);
            }
            else if(enemyStateMachine.enemyAbilityHolder.CheckIfCanUseAbility(enemyStateMachine.attack2))
            {
                enemyStateMachine.enemyAbilityHolder.UseAbility(enemyStateMachine.attack2);
                //stateMachine.ChangeState(enemyStateMachine.attack2State);
            }
        }
        else if(Vector3.Distance(holderPosition, playerPosition) <= enemyStateMachine.attack2Range)
        {
            if(enemyStateMachine.enemyAbilityHolder.CheckIfCanUseAbility(enemyStateMachine.attack2))
            {
                enemyStateMachine.enemyAbilityHolder.UseAbility(enemyStateMachine.attack2);
                //stateMachine.ChangeState(enemyStateMachine.attack2State);
            }
            else
            {
                stateMachine.ChangeState(enemyStateMachine.chaseState);
            }
        }
        else
        {
            stateMachine.ChangeState(enemyStateMachine.chaseState);
        }

        // if(enemyStateMachine.isPerformingAttack2)
        // {
            
        // }
        // else
        // {
        //     if(Vector3.Distance(holderPosition, playerPosition) <= enemyStateMachine.attack1Range)
        //     {
        //         if(enemyStateMachine.enemyAbilityHolder.CheckIfCanUseAbility(enemyStateMachine.attack1))
        //         {
        //             stateMachine.ChangeState(enemyStateMachine.attack1State);
        //         }
        //         else if(enemyStateMachine.enemyAbilityHolder.CheckIfCanUseAbility(enemyStateMachine.attack2))
        //         {
        //             stateMachine.ChangeState(enemyStateMachine.attack2State);
        //         }
        //     }
        //     else if(Vector3.Distance(holderPosition, playerPosition) <= enemyStateMachine.attack2Range)
        //     {
        //         if(enemyStateMachine.enemyAbilityHolder.CheckIfCanUseAbility(enemyStateMachine.attack2))
        //         {
        //             stateMachine.ChangeState(enemyStateMachine.attack2State);
        //         }
        //     }
        //     else
        //     {
        //         stateMachine.ChangeState(enemyStateMachine.chaseState);
        //     }
        // }
    }

    public override void UpdatePhysics() {
        enemyStateMachine.rigidBody.velocity = Vector3.zero;
    }

    public override void Exit() 
    {
        
    }
}
