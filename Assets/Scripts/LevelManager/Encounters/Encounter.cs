using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Encounter", menuName = "Data/Level Events/Encounter", order = 2)]
public class Encounter : LevelEvent
{
    [Header("Encounter Info")]
    public List<NpcInScene> npcs;
    public Vector3 encounterTriggerPosition;
    public float encounterTriggerRadius;

    public Encounter()
    {
        base.eventType = levelEventType.ENCOUNTER;
    }

    public override void OnEnable() 
    {

    }

    [Serializable]
    public class NpcInScene
    {
        public GameObject npcPrefab;
        public Vector3 npcPosition;
    }
}
