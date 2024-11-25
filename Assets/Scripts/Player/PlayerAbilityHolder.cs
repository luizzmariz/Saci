using System;
using System.Collections;
using UnityEngine;

public class PlayerAbilityHolder : MonoBehaviour
{
    #region Components 
    PlayerStateMachine playerStateMachine;
    PlayerAbilityIcons playerAbilityIcons;
    #endregion

    public Ability meleeAttack;
    public Ability rangedAttack;
    public Ability dash;

    bool isUsingAbility;

    void Awake()
    {
        GetComponents();
    }

    void GetComponents()
    {
        if(playerStateMachine == null)
        {
            playerStateMachine = GetComponent<PlayerStateMachine>();
        }
        if(playerAbilityIcons == null)
        {
            playerAbilityIcons = GameObject.Find("Canvas").transform.Find("AbilityIcons").GetComponent<PlayerAbilityIcons>();
        }
    }

    void Start()
    {
        AssignAbilities();
    }

    void AssignAbilities()
    {
        if(meleeAttack != null)
        {
            meleeAttack.AssignAbilityOwner(this.gameObject);
            playerAbilityIcons.SetAbilityIcon(meleeAttack);
        }
        if(rangedAttack != null)
        {
            rangedAttack.AssignAbilityOwner(this.gameObject);
            playerAbilityIcons.SetAbilityIcon(rangedAttack);
        }
        if(dash != null)
        {
            dash.AssignAbilityOwner(this.gameObject);
            playerAbilityIcons.SetAbilityIcon(dash);
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
            playerAbilityIcons.ChangeAbilityIcon(ability);
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

        float abilityCooldownTime = 0;

        while(abilityCooldownTime < ability.cooldownTime)
        {
            abilityCooldownTime += 0.01f;

            float completeCooldownPercentage = (float)Math.Round(abilityCooldownTime/ability.cooldownTime, 2);

            playerAbilityIcons.ChangeAbilityIconCooldownPercentage(ability, completeCooldownPercentage);
            yield return new WaitForSeconds(0.01f);
        }
        //yield return new WaitForSeconds(ability.cooldownTime);
        
        ability.state = Ability.AbilityState.ready;
        playerAbilityIcons.ChangeAbilityIcon(ability);
    }
}
