using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightManager : MonoBehaviour
{
    [Header("Level Components")]
    LevelManager levelManager;

    [Header("Boss Fight")]
    BossFight currentBossFight;
    BossStateMachine currentBossStateMachine;

    [Header("Arena")]
    bool insideArena;

    [Header("Canva")]
    CanvaManager canvaManager;

    [Header("Player")]
    [HideInInspector] public GameObject player;

    [Header("Camera")]
    [HideInInspector] public GameObject cameraGameObject;

    void Awake()
    {
        GetComponents();
    }

    void GetComponents()
    {
        if(canvaManager == null)
        {
            canvaManager = GetComponent<CanvaManager>();
        }
        if(levelManager == null)
        {
            levelManager = GetComponent<LevelManager>();
        }
        if(player == null)
        {
            player = GameObject.Find("Player");
        }
        if(cameraGameObject == null)
        {
            cameraGameObject = GameObject.Find("Main Camera");
        }
    }

    public void UnlockBossFight(BossFight bossFight)
    {
        currentBossFight = bossFight;

        OpenBossArenaGate();
    }

    void OpenBossArenaGate()
    {
        if(!insideArena)
        {
            currentBossFight.bossArenaEntrance.GetComponent<Collider>().enabled = true;
            currentBossFight.bossArenaEntrance.GetComponent<BossArenaGate>().SetTargetindicator();
        }
        else
        {
            currentBossFight.bossArenaExit.GetComponent<Collider>().enabled = true;
            currentBossFight.bossArenaExit.GetComponent<BossArenaGate>().SetTargetindicator();
        }
    }

    public void SetBossArenaTransition(Vector3 PlayerPositionToTransition, BossArenaGate.GateType gateType)
    {
        StartCoroutine(BossArenaTransitionAnimation(PlayerPositionToTransition, gateType));
    }

    IEnumerator BossArenaTransitionAnimation(Vector3 PlayerPositionToTransition, BossArenaGate.GateType gateType)
    {
        player.GetComponent<PlayerStateMachine>().uncontrollable = true;

        while(Vector3.Distance(player.transform.position, PlayerPositionToTransition) > 0.75f)
        {
            Vector3 moveVector = PlayerPositionToTransition - player.transform.position;
            player.GetComponent<Rigidbody>().velocity = moveVector.normalized * player.GetComponent<PlayerStateMachine>().movementSpeed;
            
            yield return new WaitForFixedUpdate();
        }
        
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<CharacterOrientation>().ChangeOrientation(player.transform.position + Vector3.forward);

        switch(gateType)
        {
            case BossArenaGate.GateType.ENTRANCE:
                //Make PlayerEnterArenaAnimation here {}
                StartCoroutine(canvaManager.StartAreaTransitionAnimation(EnterBossArena));
                insideArena = true;
            break;

            case BossArenaGate.GateType.EXIT:
                //Make PlayerExitArenaAnimation here {}
                StartCoroutine(canvaManager.StartAreaTransitionAnimation(ExitBossArena));
            break;
        }
    }

    public void EnterBossArena(bool debugVerification)
    {
        player.transform.position = currentBossFight.playerSpawnPositionInArena;
        cameraGameObject.transform.position = currentBossFight.playerSpawnPositionInArena;
    
        GameObject bossSpawned = Instantiate(currentBossFight.bossEnemy, 
                    currentBossFight.bossSpawnPosition, 
                    Quaternion.identity, 
                    this.transform);

        
        currentBossStateMachine = bossSpawned.GetComponent<BossStateMachine>();

        StartCoroutine(canvaManager.EndAreaTransitionAnimation(SetBossFightIntro));
    } 

    void SetBossFightIntro(bool debugVerification)
    {
        StartCoroutine(BossFightIntroAnimation());
    }

    IEnumerator BossFightIntroAnimation()
    {
        //Make BossFightIntroAnimation here {}

        yield return new WaitForSeconds(1.5f);
        
        StartBossFight();
    }

    void StartBossFight()
    {
        player.GetComponent<PlayerStateMachine>().uncontrollable = false;

        currentBossStateMachine.battleStarted = true;
    }

    public void BossDefeated()
    {
        StartCoroutine(BossFightEndAnimation());
    }

    IEnumerator BossFightEndAnimation()
    {
        //Make BossFightEndAnimation here {}

        yield return new WaitForSeconds(0);
        
        OpenBossArenaGate();
    }

    public void ExitBossArena(bool debugVerification)
    {
        player.transform.position = currentBossFight.playerExitPositionsAfterBossFight;
        cameraGameObject.transform.position = currentBossFight.playerExitPositionsAfterBossFight;

        player.GetComponent<PlayerStateMachine>().uncontrollable = false;

        StartCoroutine(canvaManager.EndAreaTransitionAnimation(BossFightEventEnded));
    }

    public void BossFightEventEnded(bool debugVerification)
    {
        levelManager.LevelEventEnd(currentBossFight);
    }
}
