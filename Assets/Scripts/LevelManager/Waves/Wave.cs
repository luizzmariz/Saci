using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyWave", menuName = "Data/Level Events/Enemy Wave", order = 1)]
public class Wave : LevelEvent
{
    [Header("Wave Info")]
    public List<Subwave> subwaves;
    [HideInInspector] public int currentSubWaveIndex;
    public float timeBetweenSubwaves;
    
    [Header("Spawnpoint")]
    public bool useSpecificSpawnArea; 
    public Vector3 spawnPosition;
    public float spawnRange;

    public Wave()
    {
        base.eventType = levelEventType.WAVE;
    }

    public override void OnEnable() 
    {
        currentSubWaveIndex = 0;
    }

    [Serializable]
    public class Subwave
    {
        public List<GameObject> enemies;
        public float timeToNextEnemy;
    }
}
