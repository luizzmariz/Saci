using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : StateMachine
{
    #region States 
    [HideInInspector] public PlayerIdleState idleState;
    [HideInInspector] public PlayerMoveState moveState;
    [HideInInspector] public PlayerAttackState attackState;
    [HideInInspector] public PlayerFireState fireState;
    [HideInInspector] public PlayerDashState dashState;
    [HideInInspector] public PlayerDamageState damageState;
    [HideInInspector] public PlayerDeadState deadState;
    [HideInInspector] public PlayerUncontrollableState uncontrollableState;
    #endregion
    
    #region Components 
    [HideInInspector] public PlayerInput playerInput;
    [HideInInspector] public Rigidbody rigidBody;
    [HideInInspector] public SpriteRenderer bodySpriteRenderer;
    [HideInInspector] public SpriteRenderer handsSpriteRenderer;
    [HideInInspector] public CharacterOrientation characterOrientation;
    [HideInInspector] public PlayerDamageable playerDamageable;
    [HideInInspector] public PlayerHands playerHands;
    [HideInInspector] public PlayerAbilityHolder playerAbilityHolder;
    [HideInInspector] public Animator playerAnimator;
    #endregion

    // public WeaponManager weaponManager;
    // public TrailRenderer trailRenderer;

    [Header("Bool variables")]
    public bool canMove;
    public bool canDash;
    public bool canAttack;
    public bool canFire;
    public bool isAiming;
    public bool uncontrollable;
    bool CanUseAbility
    {
        get => !isAttacking && !isDashing;
    }

    [Header("Movement")]
    public float runningMultiplier;
    public float movementSpeed;

    [Header("Dash")]
    [HideInInspector] public bool isDashing;
    // public float dashingPower;
    // public float dashCooldownTime;
    // public float dashingTime;

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

    [Header("Debug")]
    [SerializeField] InputAction debugInput;
    [SerializeField] public bool debugVariable;

    void Awake() 
    {
        GetComponents();

        SetStates();

        canAttack = true;
        canFire = true;
        canMove = true;
        canDash = true;
        playerDamageable.damageable = true;
        uncontrollable = false;
    }

    void GetComponents()
    {
        playerInput = GetComponent<PlayerInput>();

        rigidBody = GetComponent<Rigidbody>();
        bodySpriteRenderer = transform.Find("Visual").GetComponent<SpriteRenderer>();
        handsSpriteRenderer = transform.Find("Visual").Find("HandsVisual").GetComponent<SpriteRenderer>();
        characterOrientation = GetComponent<CharacterOrientation>();
        playerDamageable = GetComponent<PlayerDamageable>();
        playerHands = transform.Find("Hands").GetComponent<PlayerHands>();
        playerAbilityHolder = GetComponent<PlayerAbilityHolder>();
        playerAnimator = GetComponent<Animator>();
    }

    void SetStates()
    {
        idleState = new PlayerIdleState(this);
        moveState = new PlayerMoveState(this);
        attackState = new PlayerAttackState(this);
        fireState = new PlayerFireState(this);
        dashState = new PlayerDashState(this);
        damageState = new PlayerDamageState(this);
        deadState = new PlayerDeadState(this);
        uncontrollableState = new PlayerUncontrollableState(this);
    }

    private void OnEnable()
    {
        debugInput.Enable();
        debugInput.performed += context => DebugFunction();
    }

    private void OnDisable()
    {
        debugInput.Disable();
        debugInput.performed -= context => DebugFunction();
    }

    void DebugFunction()
    {
        debugVariable = !debugVariable;
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

    public void ChangeToAttackState()
    {
        if(!uncontrollable && CanUseAbility)
        {
            ChangeState(attackState);
        }
    }

    public void ChangeToFireState()
    {
        if(!uncontrollable && CanUseAbility)
        {
            ChangeState(fireState);
        }
    }

    public void ChangeToDashState()
    {
        if(!uncontrollable && CanUseAbility)
        {
            ChangeState(dashState);
        }
    }

    public IEnumerator Cooldown(string ability)
    {
        switch(ability)
        {
            // case "dash":
            // yield return new WaitForSeconds(dashCooldownTime);
            // canDash = true;
            // break;

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
        isAttacking = false;
    }

    public void DashEnd()
    {
        isDashing = false;
    }
}