using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEditor;
using UnityEngine;

public class PlayerFireState : BaseState
{
    PlayerStateMachine playerStateMachine;

    bool hasAttacked;

    bool couldMove;
    bool couldAttack;
    bool couldDash;

    public PlayerFireState(PlayerStateMachine stateMachine) : base("Attack", stateMachine) 
    {
        playerStateMachine = stateMachine;
    }

    public override void Enter() 
    {
        playerStateMachine.rigidBody.velocity = Vector3.zero;
        // couldMove = false;
        // couldAttack = false;
        // couldDash = false;

        // if(playerStateMachine.canMove)
        // {
        //     couldMove = true;
        // }
        // playerStateMachine.canMove = false;
        // if(playerStateMachine.canAttack)
        // {
        //     couldAttack = true;
        // }
        // playerStateMachine.canAttack = false;
        // if(playerStateMachine.canDash)
        // {
        //     couldDash = true;
        // }
        // playerStateMachine.canDash = false;
        // playerStateMachine.canFire = false;
        playerStateMachine.isAttacking = true;
        // playerStateMachine.attacked = true;
        //hasAttacked = false;

        playerStateMachine.playerAnimator.SetTrigger("playerAttackRanged");
    }

    public override void UpdateLogic() 
    {
        // if(!playerStateMachine.attacked)
        // {
        //     playerStateMachine.StartCoroutine(playerStateMachine.Cooldown("fire"));
        //     playerStateMachine.isAttacking = false;

        //     if(playerStateMachine.uncontrollable)
        //     {
        //         playerStateMachine.ChangeState(playerStateMachine.uncontrollableState);
        //     }

        //     Vector2 moveVector = playerStateMachine.playerInput.actions["move"].ReadValue<Vector2>();

        //     if(moveVector != Vector2.zero && !playerStateMachine.isAiming)
        //     {
        //         playerStateMachine.ChangeState(playerStateMachine.moveState);
        //     }
        //     else
        //     {
        //         playerStateMachine.ChangeState(playerStateMachine.idleState);
        //     }
        // }
        if(!playerStateMachine.isAttacking)
        {
            if(playerStateMachine.uncontrollable)
            {
                playerStateMachine.ChangeState(playerStateMachine.uncontrollableState);
            }

            Vector2 moveVector = playerStateMachine.playerInput.actions["move"].ReadValue<Vector2>();

            if(moveVector != Vector2.zero && !playerStateMachine.isAiming)
            {
                playerStateMachine.ChangeState(playerStateMachine.moveState);
            }
            else
            {
                playerStateMachine.ChangeState(playerStateMachine.idleState);
            }
        }
    }

    public override void UpdatePhysics() 
    {
        // if(!hasAttacked)
        // {
        //     Vector3 targetPoint = playerStateMachine.transform.position;

        //     if(playerStateMachine.playerInput.currentControlScheme == "Keyboard&Mouse")
        //     {
        //         Plane playerPlane = new Plane(Vector3.up, new Vector3(0, targetPoint.y, 0));
        //         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //         float hitDist;

        //         Debug.DrawRay(ray.origin, ray.direction * 50, Color.blue, 50);

        //         if(playerPlane.Raycast(ray, out hitDist))
        //         {
        //             targetPoint = ray.GetPoint(hitDist);
        //             playerStateMachine.characterOrientation.ChangeOrientation(targetPoint);
        //         }
        //     }
        //     else if(playerStateMachine.playerInput.currentControlScheme == "Gamepad")
        //     {
        //         Vector3 lookDirection = playerStateMachine.characterOrientation.lastOrientation;
        //         if(playerStateMachine.isAiming)
        //         {
        //             Vector2 aimVector2 = playerStateMachine.playerInput.actions["move"].ReadValue<Vector2>();
        //             lookDirection.x = aimVector2.x;
        //             lookDirection.z = aimVector2.y;
        //         }

        //         targetPoint = new Vector3(targetPoint.x + lookDirection.x * 10, targetPoint.y, targetPoint.z + lookDirection.z * 10);
        //         playerStateMachine.characterOrientation.ChangeOrientation(targetPoint);
        //     }

        //     Vector3 attackDirection = (targetPoint - playerStateMachine.transform.position).normalized;

        //     playerStateMachine.playerHands.Attack(attackDirection, 1); //ISSO AQUI É NECESSÁRIO PRA FUNCIONAR CERTINHO
        //     hasAttacked = true;

        //     // playerStateMachine.attacked = false; //ISSO AQUI *NÃO* É NECESSÁRIO PRA FUNCIONAR CERTINHO
        // }
    }

    public override void Exit() 
    {
        playerStateMachine.StartCoroutine(playerStateMachine.playerAbilityHolder.Cooldown(playerStateMachine.playerAbilityHolder.rangedAttack));
        // hasAttacked = false;

        // if(couldMove)
        // {
        //     playerStateMachine.canMove = true;
        // }
        // if(couldAttack)
        // {
        //     playerStateMachine.canAttack = true;
        // }
        // if(couldDash)
        // {
        //     playerStateMachine.canDash = true;
        // }
    }
}
