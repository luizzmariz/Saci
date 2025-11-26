using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    public void NotifyGameManager()
    {
        GameManager.instance.TransitionScreenAnimationFinished();
    } 
}
