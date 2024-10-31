using System.Collections;
using UnityEngine;
using TMPro;
using System;

public class CanvaManager : MonoBehaviour
{
    [Header("Full Screen Messsage")]
    [SerializeField] GameObject fullScreenMessage;
    [SerializeField] Animator fullScreenMessageAnimator;
    float fullScreenMessageDuration;

    [Header("Area Transition Screen")]
    [SerializeField] GameObject areaTransitionScreen;
    [SerializeField] Animator areaTransitionScreenAnimator;
    float areaTransitionScreenDuration;

    void Awake()
    {
        GetComponents();
    }

    void GetComponents()
    {
        if(fullScreenMessage == null)
        {
            fullScreenMessage = GameObject.Find("FullScreenMessage");
        }
        if(fullScreenMessageAnimator == null)
        {
            fullScreenMessageAnimator = fullScreenMessage.GetComponent<Animator>();
        }
        if(areaTransitionScreen == null)
        {
            areaTransitionScreen = GameObject.Find("TransitionScreen");
        }
        if(fullScreenMessageAnimator == null)
        {
            areaTransitionScreenAnimator = areaTransitionScreen.GetComponent<Animator>();
        }
    }

    public void SetFullScreenMessage(string content, float messageDuration)
    {
        fullScreenMessage.GetComponentInChildren<TMP_Text>().text = content;
        fullScreenMessageDuration = messageDuration;

        StartCoroutine(ShowFullScreenMessage());
    }

    IEnumerator ShowFullScreenMessage()
    {
        fullScreenMessage.SetActive(true);
        fullScreenMessageAnimator.SetBool("messageOn", true);

        yield return new WaitForSeconds(fullScreenMessageDuration);

        fullScreenMessageAnimator.SetBool("messageOn", false);

        yield return new WaitForSeconds(1);

        fullScreenMessage.SetActive(false);
    }

    public IEnumerator StartAreaTransitionAnimation(Action<bool> callback)
    {
        areaTransitionScreen.SetActive(true);
        areaTransitionScreenAnimator.SetTrigger("StartTransition");

        yield return new WaitForSeconds(0.1f);

        while(areaTransitionScreenAnimator.GetCurrentAnimatorStateInfo(0).IsName("TransitionStart"))
        {
            yield return new WaitForFixedUpdate();
        }
        
        callback(true);
    }

    public IEnumerator EndAreaTransitionAnimation(Action<bool> callback)
    {
        areaTransitionScreenAnimator.SetTrigger("EndTransition");

        yield return new WaitForSeconds(0.1f);

        while(areaTransitionScreenAnimator.GetCurrentAnimatorStateInfo(0).IsName("TransitionEnd"))
        {
            yield return new WaitForFixedUpdate();
        }

        areaTransitionScreen.SetActive(false);

        callback(true);
    }
}
