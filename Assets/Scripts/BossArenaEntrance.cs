using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArenaEntrance : MonoBehaviour
{
    BossFightManager bossFightManager;

    void Start()
    {
        if(bossFightManager == null)
        {
            bossFightManager = GameObject.Find("BossFightManager").GetComponent<BossFightManager>();
        }  
        this.GetComponent<Collider>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            StartCoroutine(bossFightManager.LoadBossArena());
        }
    }
}
