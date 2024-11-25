using UnityEngine;

public class PlayerMoveState : BaseState
{
    PlayerStateMachine playerStateMachine;
    Vector3 moveVector;

    public PlayerMoveState(PlayerStateMachine stateMachine) : base("Move", stateMachine) {
        playerStateMachine = stateMachine;
    }

    public override void Enter() {
        
    }

    public override void UpdateLogic() 
    {
        if(playerStateMachine.uncontrollable)
        {
            playerStateMachine.ChangeState(playerStateMachine.uncontrollableState);
        }

        Vector2 moveVectorV2 = playerStateMachine.playerInput.actions["move"].ReadValue<Vector2>();
        moveVector.x = moveVectorV2.x;
        moveVector.z = moveVectorV2.y;

        if(moveVector == Vector3.zero || playerStateMachine.isAiming)
        {
            playerStateMachine.rigidBody.velocity = Vector3.zero;
            // playerStateMachine.animator.SetBool("isMoving", false);
            playerStateMachine.ChangeState(playerStateMachine.idleState);
        }
    }

    public override void UpdatePhysics() {
        Move();
        SendOrientation();

        // playerStateMachine.animator.SetBool("isMoving", true);
    }

    void Move()
    {
        // if(playerStateMachine.debugVariable)
        // {
        //     Debug.Log("Time.fixedDeltaTime: " + Time.fixedDeltaTime);
        //     //Debug.Log("Time.deltaTime: " + Time.deltaTime);

        //     playerStateMachine.rigidBody.velocity += moveVector.normalized * playerStateMachine.movementSpeed * Time.fixedDeltaTime;
        // }
        // else
        // {
            playerStateMachine.rigidBody.velocity = moveVector.normalized * playerStateMachine.movementSpeed;
        // }
    }

    void SendOrientation()
    {
        if(!playerStateMachine.isAttacking)
        {
            //Vector2 orientation = new Vector2(playerStateMachine.transform.position.x + moveVector.x, playerStateMachine.transform.position.y + moveVector.y);
            Vector3 orientation = new Vector3(playerStateMachine.transform.position.x + moveVector.x, playerStateMachine.transform.position.y, playerStateMachine.transform.position.z + moveVector.z);
            //Debug.Log("PlayerMoveState - orientation: " + orientation);
            playerStateMachine.characterOrientation.ChangeOrientation(orientation);
        }
    }
}