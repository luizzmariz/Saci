using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateMachine : EnemyStateMachine
{
    [Header("Components")]
    [HideInInspector] public BossFightManager bossFightManager;

    [Header("Bool variables")]
    public bool battleStarted;
    
    protected override void Awake() 
    {
        base.Awake();

        bossFightManager = GameObject.Find("LevelManager").GetComponent<BossFightManager>();
    }
}

