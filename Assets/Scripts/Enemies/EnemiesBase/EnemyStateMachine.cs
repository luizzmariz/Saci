using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    [Header("Components")]
    [HideInInspector] public GameObject playerGameObject;
    [HideInInspector] public Rigidbody rigidBody;
    [HideInInspector] public Animator animator;
    [HideInInspector] public EnemyDamageable enemyDamageable;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public CharacterOrientation characterOrientation;
    [HideInInspector] public EnemyHands enemyHands;
    [HideInInspector] public SpriteRenderer bodySpriteRenderer;
    [HideInInspector] public SpriteRenderer handsSpriteRenderer;

    [Header("Bool variables")]
    public bool isAttacking;

    [Header("Attributes")]
    [Range(0f, 10f)] public float movementSpeed;
    [HideInInspector] public Vector3 movementVector;

    [Header("Damage")]
    public float knockbackDuration;
    public float knockbackPower;
    [HideInInspector] public Vector3 knockbackVector;
    public bool beingPushed;

    protected virtual void Awake() {
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        characterOrientation = GetComponent<CharacterOrientation>();
        bodySpriteRenderer = transform.Find("Visual").GetComponent<SpriteRenderer>();
        handsSpriteRenderer = transform.Find("Visual").Find("HandsVisual").GetComponent<SpriteRenderer>();
        enemyDamageable = transform.Find("DamageCollider").GetComponent<EnemyDamageable>();
        enemyHands = transform.Find("Hands").GetComponent<EnemyHands>();

        playerGameObject = GameObject.Find("Player");
    }

    public virtual IEnumerator Cooldown(string ability)
    {
        yield return null;
    }

    public virtual void TakeDamage(Vector3 knockbackVector) 
    {
        
    }

    public virtual void AttackEnd()
    {
        isAttacking = false;
    }

    public virtual void ChangeToAttackState(Ability ability)
    {

    }
}

