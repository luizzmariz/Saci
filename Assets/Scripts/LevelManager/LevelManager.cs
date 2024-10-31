using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{
    [Header("Level Events")]
    [SerializeField] private List<LevelEvent> levelEvents;
    int currentLevelEventIndex = 0;
    [SerializeField] float timeBetweenEventsInSeconds;

    [Header("Boss Fight System")]
    [HideInInspector] public BossFightManager bossFightManager;

    [Header("Wave System")]
    [HideInInspector] public WaveSpawner waveSpawner;

    [Header("Player")]
    [HideInInspector] public PlayerDamageable playerDamageable;

    [Header("Canva")]
    [HideInInspector] CanvaManager canvaManager;

    [Header("Debug")]
    [SerializeField] InputAction unlockExtraBossFight;
    [SerializeField] InputAction testStart;
    [SerializeField] BossFight extraBossFight;
    bool levelStarted = false;

    void Awake()
    {
        GetComponents();

        unlockExtraBossFight.Enable();
        unlockExtraBossFight.performed += context => UnlockExtraBossFight();

        testStart.Enable();
        testStart.performed += context => StartLevel();
    }

    void GetComponents()
    {
        if(playerDamageable == null)
        {
            playerDamageable = GameObject.Find("Player").GetComponent<PlayerDamageable>();
        }
        if(waveSpawner == null)
        {
            waveSpawner = GetComponent<WaveSpawner>();
        }
        if(bossFightManager == null)
        {
            bossFightManager = GetComponent<BossFightManager>();
        }
        if(canvaManager == null)
        {
            canvaManager = GetComponent<CanvaManager>();
        }
    }

    void Start()
    {
        LoadAllLevelEvents();
    }

    void LoadAllLevelEvents()
    {
        foreach(LevelEvent levelEvent in levelEvents)
        {
            levelEvent.OnEnable();
        }
    }

    public void StartLevel()
    {
        if(!levelStarted)
        {
            StartCoroutine(StartNextLevelEvent(0));
            levelStarted = true;
        }
    }

    IEnumerator StartNextLevelEvent(float timeBeforeFunctionStartsInSeconds)
    {
        yield return new WaitForSeconds(timeBeforeFunctionStartsInSeconds);

        LevelEvent currentlevelEvent = levelEvents[currentLevelEventIndex];
        
        switch(currentlevelEvent.eventType)
        {
            case LevelEvent.levelEventType.WAVE:
                string waveClearText = "Wave " + waveSpawner.currentWaveInOrder;
                SendMessageToCanva(waveClearText, 1f);

                yield return new WaitForSeconds(timeBetweenEventsInSeconds);

                waveSpawner.StartWave((Wave)currentlevelEvent);
            break;

            case LevelEvent.levelEventType.BOSSFIGHT:
                string bossFightText = "Boss unlocked";
                SendMessageToCanva(bossFightText, 1f);

                yield return new WaitForSeconds(timeBetweenEventsInSeconds);

                bossFightManager.UnlockBossFight((BossFight)currentlevelEvent);
            break;

            case LevelEvent.levelEventType.OTHER:
            break;
        }
    }

    public void LevelEventEnd(LevelEvent levelEvent)
    {
        currentLevelEventIndex++;

        if(currentLevelEventIndex >= levelEvents.Count)
        {
            EndGame();
        }
        else
        {
            RegenPlayerHealth();

            switch(levelEvent.eventType)
            {
                case LevelEvent.levelEventType.WAVE:
                    string waveClearText = "Wave \n Cleared";
                    SendMessageToCanva(waveClearText, 1f);
                break;

                case LevelEvent.levelEventType.BOSSFIGHT:
                    string bossFightText = "Boss \n Defeated";
                    SendMessageToCanva(bossFightText, 1f);
                break;

                case LevelEvent.levelEventType.OTHER:
                break;
            }
            
            StartCoroutine(StartNextLevelEvent(timeBetweenEventsInSeconds + 1f));
        }
    }

    void RegenPlayerHealth()
    {
        playerDamageable.Heal(8);
    }

    void SendMessageToCanva(string messageContent, float duration)
    {
        canvaManager.SetFullScreenMessage(messageContent, duration);
    }

    void EndGame()
    {
        if(GameManager.instance != null)
        {
            StartCoroutine(GameManager.instance.EndGame(true));
        }
        else
        {
            Debug.Log("Game end, there is no Game Manager to proceed");
        }
    }

    //Debug functions below

    void UnlockExtraBossFight()
    {
        if(extraBossFight != null)
        {
            bossFightManager.UnlockBossFight(extraBossFight);
        }
    }
}
