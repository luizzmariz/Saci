using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossFight", order = 2)]
public class BossFight : LevelEvent
{
    [Header("Boss Info")]
    public GameObject bossEnemy;
    public Vector3 bossSpawnPosition;

    [Header("Arena")]
    [SerializeField] string bossArenaEntranceName;
    [HideInInspector] public GameObject bossArenaEntrance;
    [SerializeField] string bossArenaExitName;
    [HideInInspector] public GameObject bossArenaExit;
    public Vector3 playerSpawnPositionInArena;
    public Vector3 playerExitPositionsAfterBossFight;

    public BossFight()
    {
        base.eventType = levelEventType.BOSSFIGHT;
    }

    public override void OnEnable() 
    {
        bossArenaEntrance = GameObject.Find(bossArenaEntranceName);
        bossArenaExit = GameObject.Find(bossArenaExitName);
    }

    [Serializable]
    public class BossRules
    {
        
    }
}
