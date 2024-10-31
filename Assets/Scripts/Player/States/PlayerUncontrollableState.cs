using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUncontrollableState : BaseState
{
    PlayerStateMachine playerStateMachine;

    bool couldMove;
    bool couldAttack;
    bool couldFire;
    bool couldDash;

    public PlayerUncontrollableState(PlayerStateMachine stateMachine) : base("Uncontrollable", stateMachine) {
        playerStateMachine = stateMachine;
    }

    public override void Enter() {
        couldMove = false;
        couldAttack = false;
        couldFire = false;
        couldDash = false;

        if(playerStateMachine.canMove)
        {
            couldMove = true;
        }
        playerStateMachine.canMove = false;
        if(playerStateMachine.canAttack)
        {
            couldAttack = true;
        }
        playerStateMachine.canAttack = false;
        if(playerStateMachine.canFire)
        {
            couldFire = true;
        }
        playerStateMachine.canFire = false;
        if(playerStateMachine.canDash)
        {
            couldDash = true;
        }
        playerStateMachine.canDash = false;
        playerStateMachine.playerDamageable.damageable = false;
    }

    public override void UpdateLogic() 
    {
        if(!playerStateMachine.uncontrollable)
        {
            playerStateMachine.ChangeState(playerStateMachine.idleState);
        }
    }

    public override void UpdatePhysics() 
    {

    }

    public override void Exit()
    {
        playerStateMachine.playerDamageable.damageable = true;
        if(couldMove)
        {
            playerStateMachine.canMove = true;
        }
        if(couldAttack)
        {
            playerStateMachine.canAttack = true;
        }
        if(couldFire)
        {
            playerStateMachine.canFire = true;
        }
        if(couldDash)
        {
            playerStateMachine.canDash = true;
        }
    }
}