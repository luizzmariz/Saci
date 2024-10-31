using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArenaGate : MonoBehaviour
{
    public enum GateType
    {
        ENTRANCE,
        EXIT
    }

    [Header("Gate Type")]
    public GateType gateType;

    [Header("Managers")]
    BossFightManager bossFightManager;
    TargetIndicatorController targetIndicatorController;
    
    void Awake()
    {
        GetComponents();
    }

    void GetComponents()
    {
        if(bossFightManager == null)
        {
            bossFightManager = GameObject.Find("LevelManager").GetComponent<BossFightManager>();
        }  
        if(targetIndicatorController == null)
        {
            targetIndicatorController = GameObject.Find("TargetIndicator").GetComponent<TargetIndicatorController>();
        }
    }

    void Start()
    {
        this.GetComponent<Collider>().enabled = false;
    }

    public void SetTargetindicator()
    {
        targetIndicatorController.AddTargetIndicator(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            bossFightManager.SetBossArenaTransition(this.transform.position, gateType);
            
            RemoveTargetindicator();

            this.GetComponent<Collider>().enabled = false;
        }
    }

    public void RemoveTargetindicator()
    {
        targetIndicatorController.RemoveTargetIndicator(this.gameObject);
    }
}
