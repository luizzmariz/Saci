using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : ScriptableObject
{
    [Header("Ability General Statistics")]
    public new string name;
    public float cooldownTime;

    protected GameObject abilityOwnerGameObject;
    protected Transform attacksParentGameObject;

    Animator abilityAnimator;

    public enum AbilityState
    {
        ready,
        active,
        cooldown
    }

    public AbilityState state = AbilityState.ready;

    void OnEnable()
    {
        state = AbilityState.ready;
    }

    public void AssignAbilityOwner(GameObject abilityOwner) 
    {
        abilityOwnerGameObject = abilityOwner;
        attacksParentGameObject = abilityOwner.transform;
    }

    public virtual void Activate() 
    {} 
}
