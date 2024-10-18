using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy1ChaseState : BaseState
{
    Enemy1StateMachine enemyStateMachine;
    
    public Vector3 holderPosition;
    public Vector3 playerPosition;
    public Vector3 lastPlayerPosition;
    public bool hasAskedPath;
    public bool followingPath;

    int targetIndex;
    public Vector3[] path;

    public Enemy1ChaseState(Enemy1StateMachine stateMachine) : base("Chase", stateMachine) {
        enemyStateMachine = stateMachine;
    }

    public override void Enter() {
        enemyStateMachine.enemyDamageable.damageable = true;
        hasAskedPath = false;
        followingPath = false;
    }

    public override void UpdateLogic() {
        if(enemyStateMachine.playerGameObject.GetComponent<PlayerStateMachine>().currentState == enemyStateMachine.playerGameObject.GetComponent<PlayerStateMachine>().deadState)
        {
            stateMachine.ChangeState(enemyStateMachine.idleState);
        }

        holderPosition = enemyStateMachine.transform.position;
        playerPosition = enemyStateMachine.playerGameObject.transform.position;
        
        if(Vector3.Distance(holderPosition, playerPosition) > enemyStateMachine.rangeOfView)
        {
            stateMachine.ChangeState(enemyStateMachine.idleState);
        }
        else if(Vector3.Distance(holderPosition, playerPosition) <= enemyStateMachine.rangeOfAttack)
        {
            if(enemyStateMachine.canAttack)
            {
                stateMachine.ChangeState(enemyStateMachine.attackState);
            }
            else
            {
                stateMachine.ChangeState(enemyStateMachine.idleState);
            }
        }
    }

    public override void UpdatePhysics() {
        holderPosition = enemyStateMachine.transform.position;
        playerPosition = enemyStateMachine.playerGameObject.transform.position;

        if(path != null && path.Count() <= 0)
        {
            followingPath = false;
        }

        if(Vector3.Distance(holderPosition, playerPosition) < enemyStateMachine.rangeOfView * 0.8f)
        {
            if(!hasAskedPath && !followingPath)
            {
                hasAskedPath = true;
                lastPlayerPosition = playerPosition;
                enemyStateMachine.pathRequestManager.RequestPath(holderPosition, playerPosition, OnPathFound, enemyStateMachine.gameObject); 
            }
            else if(followingPath)
            {
                if(Vector3.Distance(playerPosition, lastPlayerPosition) > 1.25f)
                {
                    followingPath = false;
                    hasAskedPath = true;
                    lastPlayerPosition = playerPosition;
                    enemyStateMachine.pathRequestManager.RequestPath(holderPosition, playerPosition, OnPathFound, enemyStateMachine.gameObject); 
                }
                else
                {
                    FollowPath();
                }
            }
        }
        else
        {
            if(followingPath)
            {
                FollowPath();
            }
            else
            {
                enemyStateMachine.rigidBody.velocity = (playerPosition - holderPosition).normalized * enemyStateMachine.movementSpeed;
            }
        }
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
		if (pathSuccessful && enemyStateMachine.currentState == this)
        {
            targetIndex = 0;
            hasAskedPath = false;
            followingPath = true;
			path = newPath;
		}
        else
        {
            stateMachine.ChangeState(enemyStateMachine.idleState);
        }
	}

    public void FollowPath() 
    {
		Vector3 currentWaypoint = path[targetIndex];
        currentWaypoint.y = 0;
        
        //Debug.Log("the Distance between holderPosition: " + holderPosition + " and currentWaypoint: " + currentWaypoint + " is " + Vector3.Distance(holderPosition, currentWaypoint));

		if(Vector3.Distance(holderPosition, currentWaypoint) <= 0.1) 
        {
			targetIndex ++;
			if(targetIndex >= path.Count()) 
            {
                followingPath = false;
                return;
			}
		}
        
        enemyStateMachine.characterOrientation.ChangeOrientation(currentWaypoint);
        if(enemyStateMachine.ShowChasePath)
        {
            Debug.DrawRay(stateMachine.transform.position, currentWaypoint - stateMachine.transform.position, Color.grey);
        }

        Vector3 movementDirection = currentWaypoint - holderPosition;
        enemyStateMachine.rigidBody.velocity = movementDirection.normalized * enemyStateMachine.movementSpeed;
	}

    public override void Exit() 
    {
        enemyStateMachine.rigidBody.velocity = Vector3.zero;
        followingPath = false;
        path = null;
    }
}