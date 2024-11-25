using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAbilityHolder : MonoBehaviour
{
    #region Components 
    EnemyStateMachine enemyStateMachine;
    #endregion

    public List<Ability> Abilities;

    [HideInInspector] public bool isUsingAbility {get; private set;}

    void Awake()
    {
        GetComponents();
    }

    void GetComponents()
    {
        if(enemyStateMachine == null)
        {
            enemyStateMachine = GetComponent<EnemyStateMachine>();
        }
    }

    void Start()
    {
        AssignAbilities();
    }

    void AssignAbilities()
    {
        foreach(Ability ability in Abilities)
        {
            ability.AssignAbilityOwner(this.gameObject);
        }
    }

    public bool CheckIfCanUseAbility(Ability ability)
    {
        bool abilityIsReady = CheckIfAbilityIsReady(ability);

        return abilityIsReady && !isUsingAbility;
    }

    public bool CheckIfAbilityIsReady(Ability ability)
    {
        if(ability.state == Ability.AbilityState.ready)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UseAbility(Ability ability)
    {
        if(CheckIfAbilityIsReady(ability))
        {
            isUsingAbility = true;

            ability.Activate();
            
            ability.state = Ability.AbilityState.active;
        }
    }

    public void EndAbility(Ability ability)
    {
        if(ability.state == Ability.AbilityState.active)
        {
            ability.Deactivate();
        }
    }

    public IEnumerator Cooldown(Ability ability)
    {
        ability.state = Ability.AbilityState.cooldown;
        isUsingAbility = false; 

        yield return new WaitForSeconds(ability.cooldownTime);
        
        ability.state = Ability.AbilityState.ready;
    }
}
