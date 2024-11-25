using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAbility : Ability
{
    #region Components
    protected GameObject enemy;
    protected EnemyStateMachine enemyStateMachine;
    protected CharacterOrientation enemyOrientation;
    protected GameObject player;
    
    #endregion

    protected virtual void GetComponents()
    {
        if(abilityOwnerGameObject != null)
        {
            enemy = this.abilityOwnerGameObject;
            enemyStateMachine = enemy.GetComponent<EnemyStateMachine>();
            enemyOrientation = enemy.GetComponent<CharacterOrientation>();
            player = GameObject.Find("Player");
        }
    }
}
