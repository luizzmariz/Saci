using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightManager : MonoBehaviour
{
    public GameObject Player;
    public List<Collider> bossArenaEntrances; 
    public List<Vector3> playerSpawnPositionsInBossArenas;
    public List<Vector3> playerExitPositionsAfterBossFights;
    public List<GameObject> Bosses;
    public List<Vector3> BossesSpawnPositions;
    public GameObject transitionScreen;
    public Animator transitionScreenAnimator;

    int bossIndex;

    void Start()
    {
        bossIndex = 0;
        transitionScreenAnimator = transitionScreen.GetComponent<Animator>();
    }

    public void UnlockNextBossFight()
    {
        OpenPath();
    }

    void OpenPath()
    {
        bossArenaEntrances[bossIndex].enabled = true;
    }

    public IEnumerator LoadBossArena()
    {
        transitionScreen.SetActive(true);
        transitionScreenAnimator.SetBool("Transiting",true);

        yield return new WaitForSeconds(1.5f);

        Player.transform.position = playerSpawnPositionsInBossArenas[bossIndex];

        GameObject bossSpawned = Instantiate(Bosses[bossIndex], 
                    BossesSpawnPositions[bossIndex], 
                    Quaternion.identity, 
                    this.transform);

        yield return new WaitForSeconds(1.5f);

        transitionScreenAnimator.SetBool("Transiting",false);
        transitionScreen.SetActive(false);

        yield return new WaitForSeconds(3f);

        bossSpawned.GetComponent<BossStateMachine>().battleStarted = true;
    }

    public void BossDefeated()
    {
        bossIndex++;
        if(bossIndex >= Bosses.Count)
        {
            if(GameManager.instance == null)
            {
                Debug.Log("Boss final morreu, sem GameManager pra prosseguir");
            }
            else
            {
                StartCoroutine(GameManager.instance.EndGame(true));
            }
        }
        else
        {
            Debug.Log("Ta na hora de ajeitar o fluxo lógico desse código");
        }
    }
}
