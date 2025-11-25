using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEvent : ScriptableObject
{
    public enum levelEventType
    {
        WAVE,
        BOSSFIGHT,
        ENCOUNTER,
        OTHER
    }

    [SerializeField] public levelEventType eventType {get; protected set;}

    public virtual void OnEnable() 
    {
        
    }
}
