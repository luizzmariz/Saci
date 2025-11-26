using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EncounterManager : MonoBehaviour
{
    [Header("Encounter")]
    Encounter currentEncounter;

    [Header("Encounter Events")]
    [SerializeField] List<UnityEvent> events = new List<UnityEvent>();

    public void SetEncounter(Encounter encounter)
    {
        currentEncounter = encounter;

        SpawnNPCs();
    }

    public void SpawnNPCs()
    {
        for(int i = 0; i < currentEncounter.npcs.Count; i++)
        {
            GameObject npcSpawned = Instantiate(currentEncounter.npcs[i].npcPrefab, 
                currentEncounter.npcs[i].npcPosition, 
                Quaternion.identity, 
                GameObject.Find("InstantiatedObjects").transform);
            
            // enemySpawned.name = "W" + i + " (" + o + ") " + currentEncounter.subwaves[currentEncounter.currentSubWaveIndex].enemies[o].name;
        }
    }

    void OnEnable()
	{
        LevelManager.EncounterTrigger += Trigger;
	}

    void OnDisable()
    {
        LevelManager.EncounterTrigger -= Trigger;
    }

    void Trigger()
    {
        events[0].Invoke();
    }
}
