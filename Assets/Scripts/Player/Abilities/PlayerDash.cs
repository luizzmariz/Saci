using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDash", menuName = "Data/Player/Abilities/Dash")]
public class PlayerDash : PlayerAbility
{
    [Header("Player Dash Statistics")]
    public float dashingPower; 
    public float dashingTime;

    public override void Activate()
    {
        base.Activate();
        GetComponents();

        Debug.Log(playerOrientation);

        Vector3 dashDirection = playerOrientation.lastOrientation;

        if(dashDirection == Vector3.zero)
        {
            dashDirection = Vector3.back;
        }

        playerStateMachine.StartCoroutine(Dash(dashDirection));
    }

    public IEnumerator Dash(Vector3 dashDirection)
    {
        // playerStateMachine.trailRenderer.emitting = true;
        player.GetComponent<Rigidbody>().velocity = dashDirection.normalized * dashingPower;

        yield return new WaitForSeconds(dashingTime);

        // playerStateMachine.trailRenderer.emitting = false;
        playerStateMachine.rigidBody.velocity = Vector3.zero;

        ContactStateMachine();
    }

    void ContactStateMachine()
    {
        playerStateMachine.DashEnd();
    }
}
