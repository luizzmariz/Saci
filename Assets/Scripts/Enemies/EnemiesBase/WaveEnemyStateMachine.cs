using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEnemyStateMachine : EnemyStateMachine
{
    [Header("Components")]
    [HideInInspector] public PathRequestManager pathRequestManager;
    [HideInInspector] public WaveSpawner waveSpawner;

    protected override void Awake() {
        base.Awake();

        pathRequestManager = GameObject.Find("PathfindingManager").GetComponent<PathRequestManager>();
        waveSpawner = GameObject.Find("WaveSpawner").GetComponent<WaveSpawner>();
    }
}

