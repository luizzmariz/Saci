using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : StateMachine
{
    //States
    [HideInInspector] public PlayerIdleState idleState;
    [HideInInspector] public PlayerMoveState moveState;
    [HideInInspector] public PlayerAttackState attackState;
    [HideInInspector] public PlayerFireState fireState;
    [HideInInspector] public PlayerDashState dashState;
    [HideInInspector] public PlayerDamageState damageState;
    [HideInInspector] public PlayerDeadState deadState;
    [HideInInspector] public PlayerUncontrollableState uncontrollableState;
    

    //Global information
    
    //GameObject information
    [HideInInspector] public PlayerInput playerInput;
    [HideInInspector] public Rigidbody rigidBody;
    [HideInInspector] public SpriteRenderer bodySpriteRenderer;
    [HideInInspector] public SpriteRenderer handsSpriteRenderer;
    [HideInInspector] public CharacterOrientation characterOrientation;
    [HideInInspector] public PlayerDamageable playerDamageable;
    [HideInInspector] public PlayerHands playerHands;

    // public WeaponManager weaponManager;
    // public TrailRenderer trailRenderer;

    [Header("Bool variables")]
    public bool canMove;
    public bool canDash;
    public bool canAttack;
    public bool canFire;
    public bool isAiming;
    public bool uncontrollable;

    [Header("Movement")]
    public float runningMultiplier;
    public float movementSpeed;

    [Header("Dash")]
    public bool isDashing;
    public float dashingPower;
    public float dashCooldownTime;
    public float dashingTime;

    [Header("Damage")]
    public float knockbackDuration;
    [HideInInspector] public Vector3 knockbackVector;
    public bool beingPushed;

    [Header("Attack")]
    public bool isAttacking;
    public bool attacked;
    public float attack1CooldownTimer;
    public float fireCooldownTimer;

    [Header("InvencibilityTime")]
    public float invencibilityTime;

    private void Awake() {
        playerInput = GetComponent<PlayerInput>();

        rigidBody = GetComponent<Rigidbody>();
        bodySpriteRenderer = transform.Find("Visual").GetComponent<SpriteRenderer>();
        handsSpriteRenderer = transform.Find("Visual").Find("HandsVisual").GetComponent<SpriteRenderer>();
        characterOrientation = GetComponent<CharacterOrientation>();
        playerDamageable = GetComponent<PlayerDamageable>();
        playerHands = transform.Find("Hands").GetComponent<PlayerHands>();
        

        idleState = new PlayerIdleState(this);
        moveState = new PlayerMoveState(this);
        attackState = new PlayerAttackState(this);
        fireState = new PlayerFireState(this);
        dashState = new PlayerDashState(this);
        damageState = new PlayerDamageState(this);
        deadState = new PlayerDeadState(this);
        uncontrollableState = new PlayerUncontrollableState(this);

        canAttack = true;
        canFire = true;
        canMove = true;
        canDash = true;
        playerDamageable.damageable = true;
        uncontrollable = false;
    }

    protected override BaseState GetInitialState() {
        return idleState;
    }

    protected override void Update()
    {
        base.Update();

        if(playerInput.actions["Aim"].ReadValue<float>() != 0)
        {
            isAiming = true;
        }
        else
        {
            isAiming = false;
        }
    }

    // private void OnGUI()
    // {
    //     GUILayout.BeginArea(new Rect(450, 125, 200f, 150f));
    //     string content = currentState != null ? currentState.name : "(no current state)";
    //     GUILayout.Label($"<color='black'><size=40>{content}</size></color>");
    //     GUILayout.EndArea();
    // }

    public void OnMove(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(!uncontrollable && canMove && !isAiming && !isDashing)
            {
                ChangeState(moveState);
            }
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(!uncontrollable && canAttack && !isAttacking && !isDashing)
            {
                //Debug.Log("Changing to attackState, canAttack: " + canAttack + ", isDashing: " + isDashing + ", isAttacking: " + isAttacking + ". Time: " + Time.time);
                // attackType = 1;
                ChangeState(attackState);
            }
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(!uncontrollable && canFire && !isAttacking && !isDashing)
            {
                // attackType = 2;
                ChangeState(fireState);
            }
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(!uncontrollable && canDash && !isDashing && !isAttacking)
            {
                //Debug.Log("Changing to dashState, canDash: " + canDash + ", isDashing: " + isDashing + ", isAttacking: " + isAttacking + ". Time: " + Time.time);
                ChangeState(dashState);
            }
        }
    }

    public IEnumerator Cooldown(string ability)
    {
        switch(ability)
        {
            case "dash":
            yield return new WaitForSeconds(dashCooldownTime);
            canDash = true;
            break;

            case "attack":
            yield return new WaitForSeconds(attack1CooldownTimer);
            canAttack = true;
            break;

            case "fire":
            yield return new WaitForSeconds(fireCooldownTimer);
            canFire = true;
            break;

            default:
            break;
        }
        
        // runningCoroutines.Remove(ability);
    }

    public void AttackEnd()
    {
        attacked = false;
        playerHands.AttackEnd();
    }
}