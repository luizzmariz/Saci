using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEnemyStateMachine : EnemyStateMachine
{
    [Header("Components")]
    [HideInInspector] public PathRequestManager pathRequestManager;
    [HideInInspector] public WaveSpawner waveSpawner;
    
    [Header("Bool variables")]
    public bool canMove;
    public bool canAttack;

    [Header("Attributes")]
    [Range(0f, 50f)] public float rangeOfView;
    [Range(0f, 25f)] public float rangeOfAttack;

    [Header("Attack")]
    public float attackCooldownTimer;

    protected override void Awake() {
        base.Awake();

        pathRequestManager = GameObject.Find("PathfindingManager").GetComponent<PathRequestManager>();
        waveSpawner = GameObject.Find("LevelManager").GetComponent<WaveSpawner>();

        canAttack = true;
        canMove = true;
    }
}

