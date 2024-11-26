using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1StateMachine : BossStateMachine
{
    [Header("Components")]
    [HideInInspector] public PathRequestManager pathRequestManager;
    [HideInInspector] public EnemyAbilityHolder enemyAbilityHolder;

    [Header("States")]
    [HideInInspector] public Boss1IdleState idleState;
    [HideInInspector] public Boss1ChaseState chaseState;
    [HideInInspector] public Boss1Attack1State attack1State;
    [HideInInspector] public Boss1Attack2State attack2State;
    [HideInInspector] public Boss1DamageState damageState;
    [HideInInspector] public Boss1DeadState deadState;

    [Header("Abilities")]
    [HideInInspector] public Ability attack1;
    [HideInInspector] public Ability attack2;

    [Header("Bool variables")]
    public bool canDoAttack1;
    public bool canDoAttack2;
    public bool isPerformingAttack2;

    [Header("ShowPath")]
    public float nodeRadius;
    public bool ShowChasePath;

    [Header("Attributes")]
    [Range(0f, 25f)] public float attack1Range;
    [Range(0f, 25f)] public float attack2Range;

    [Header("Attack")]
    public float attack1CooldownTimer;
    public float attack2CooldownTimer;

    protected override void Awake() {
        base.Awake();

        pathRequestManager = GameObject.Find("Boss1Arena").GetComponent<PathRequestManager>();
        enemyAbilityHolder = GetComponent<EnemyAbilityHolder>();
        //Debug.Log(pathRequestManager);

        if(enemyAbilityHolder.Abilities != null)
        {
            attack1 = enemyAbilityHolder.Abilities[0];
            attack2 = enemyAbilityHolder.Abilities[1];
        }

        idleState = new Boss1IdleState(this);
        chaseState = new Boss1ChaseState(this);
        attack1State = new Boss1Attack1State(this);
        attack2State = new Boss1Attack2State(this);
        damageState = new Boss1DamageState(this);
        deadState = new Boss1DeadState(this);

        canDoAttack1 = true;
        canDoAttack2 = true;

        Debug.Log("Teste");
    }

    protected override BaseState GetInitialState() {
        return idleState;
    }

    public override void ChangeToAttackState(Ability ability)
    {
        if(ability == attack1)
        {
            ChangeState(attack1State);
        }
        else if(ability == attack2)
        {
            ChangeState(attack2State);
        }
    }

    // public override IEnumerator Cooldown(string ability)
    // {
    //     switch(ability)
    //     {
    //         case "attack1":
    //         // Debug.Log("attack cooldown started");
    //         yield return new WaitForSeconds(attack1CooldownTimer);
    //         // Debug.Log("attack cooldown ended");
    //         canDoAttack1 = true;
    //         break;

    //         case "attack2":
    //         // Debug.Log("attack cooldown started");
    //         yield return new WaitForSeconds(attack2CooldownTimer);
    //         // Debug.Log("attack cooldown ended");
    //         canDoAttack2 = true;
    //         break;

    //         default:
    //         break;
    //     }
    // }

    public void PushRug()
    {
        if(!isAttacking)
        {
            ChangeState(deadState);
        }
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(250, 125, 200f, 150f));
        string content = currentState != null ? "Boss: " + currentState.name : "Boss: " + "(no current state)";
        GUILayout.Label($"<color='red'><size=40>{content}</size></color>");
        GUILayout.EndArea();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attack2Range);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attack1Range);

		if(ShowChasePath && chaseState != null && chaseState.path != null)
		{
			foreach(Vector3 tile in chaseState.path)
			{
				Gizmos.color = new Color(0,0,1,1f);
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
        else if(!isAttacking)
        {
            this.knockbackVector = knockbackVector;
            ChangeState(damageState);
        }
    }
}

