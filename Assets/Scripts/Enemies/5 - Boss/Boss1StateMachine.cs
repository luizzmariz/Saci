using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1StateMachine : BaseEnemyStateMachine
{
    [Header("States")]
    [HideInInspector] public Boss1IdleState idleState;
    [HideInInspector] public Boss1ChaseState chaseState;
    [HideInInspector] public Boss1AttackState attackState;
    [HideInInspector] public Boss1DamageState damageState;
    [HideInInspector] public Boss1DeadState deadState;

    [Header("ShowPath")]
    public float nodeRadius;
    public bool ShowChasePath;

    protected override void Awake() {
        base.Awake();

        idleState = new Boss1IdleState(this);
        chaseState = new Boss1ChaseState(this);
        attackState = new Boss1AttackState(this);
        damageState = new Boss1DamageState(this);
        deadState = new Boss1DeadState(this);

        canAttack = true;
        canMove = true;
        waveSpawner.EnemySpawned(this);
    }

    protected override BaseState GetInitialState() {
        return idleState;
    }

    public override IEnumerator Cooldown(string ability)
    {
        switch(ability)
        {
            case "attack":
            // Debug.Log("attack cooldown started");
            yield return new WaitForSeconds(attackCooldownTimer);
            // Debug.Log("attack cooldown ended");
            canAttack = true;
            break;

            default:
            break;
        }
    }

    // private void OnGUI()
    // {
    //     GUILayout.BeginArea(new Rect(250, 125, 200f, 150f));
    //     string content = currentState != null ? "Boss: " + currentState.name : "Boss: " + "(no current state)";
    //     GUILayout.Label($"<color='red'><size=40>{content}</size></color>");
    //     GUILayout.EndArea();
    // }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangeOfView);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeOfAttack);

		if(ShowChasePath && chaseState != null && chaseState.path != null)
		{
			foreach(Vector3 tile in chaseState.path)
			{
				Gizmos.color = new Color(0,0,1,0.3f);
				Gizmos.DrawCube(tile, Vector3.one * (nodeRadius-.1f));
			}
		}
    }

    public override void TakeDamage(Vector3 knockbackVector) 
    {
        //Debug.Log("chegou aqui - " + enemyDamageable.currentHealth);
        if(enemyDamageable.currentHealth <= 0)
        {
            ChangeState(deadState);
        }
        else
        {
            this.knockbackVector = knockbackVector;
            ChangeState(damageState);
        }
    }
}

