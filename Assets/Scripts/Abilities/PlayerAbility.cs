using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbility : Ability
{
    #region Components
    protected GameObject player;
    protected PlayerInputHandler playerInpuHandler;
    protected PlayerStateMachine playerStateMachine;
    protected CharacterOrientation playerOrientation;
    #endregion

    protected void GetComponents()
    {
        if(abilityOwnerGameObject != null)
        {
            player = this.abilityOwnerGameObject;
            playerInpuHandler = player.GetComponent<PlayerInputHandler>();
            playerStateMachine = player.GetComponent<PlayerStateMachine>();
            playerOrientation = player.GetComponent<CharacterOrientation>();
        }
    }
}
