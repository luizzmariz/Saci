using UnityEngine;

public class EncounterManager : MonoBehaviour
{
    [Header("Encounter")]
    Encounter currentEncounter;

    public void SetEncounter(Encounter encounter)
    {
        currentEncounter = encounter;

        SpawnNPCs();
    }

    void SpawnEnemy()
    {
        // GameObject enemySpawned = Instantiate(enemyToSpawn, 
        //             spawnLocation.position, 
        //             Quaternion.identity);
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
}
