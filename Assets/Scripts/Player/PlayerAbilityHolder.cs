using System.Collections;
using UnityEngine;

public class PlayerAbilityHolder : MonoBehaviour
{
    #region Components 
    PlayerStateMachine playerStateMachine;
    #endregion

    public Ability meleeAttack;
    public Ability rangedAttack;
    public Ability dash;

    void Awake()
    {
        GetComponents();

        AssignAbilities();
    }

    void GetComponents()
    {
        if(playerStateMachine == null)
        {
            playerStateMachine = GetComponent<PlayerStateMachine>();
        }
    }

    void AssignAbilities()
    {
        if(meleeAttack != null)
        {
            meleeAttack.AssignAbilityOwner(this.gameObject);
        }
        if(rangedAttack != null)
        {
            rangedAttack.AssignAbilityOwner(this.gameObject);
        }
        if(dash != null)
        {
            dash.AssignAbilityOwner(this.gameObject);
        }
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
            ability.Activate();
            ability.state = Ability.AbilityState.active;
        }
    }

    public IEnumerator Cooldown(Ability ability)
    {
        ability.state = Ability.AbilityState.cooldown;

        yield return new WaitForSeconds(ability.cooldownTime);
        
        ability.state = Ability.AbilityState.ready;
    }
}
