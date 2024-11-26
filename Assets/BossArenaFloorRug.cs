using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArenaFloorRug : MonoBehaviour
{
    GameObject bossGameObject;
    Boss1StateMachine bossStateMachine;
    Coroutine rugPush;

    public void LinkRugsToBoss(GameObject boss)
    {
        bossGameObject = boss;
        bossStateMachine = boss.GetComponent<Boss1StateMachine>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            this.GetComponent<Collider>().enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            rugPush = StartCoroutine(StartRugPushCountdown());
        }
    }

    IEnumerator StartRugPushCountdown()
    {
        yield return new WaitForSeconds(3);

        bossStateMachine.PushRug();
    }

    void StopRugPushCountdown()
    {
        if(rugPush != null)
        {
            StopCoroutine(rugPush);
            rugPush = null;
        }   
    }
}
