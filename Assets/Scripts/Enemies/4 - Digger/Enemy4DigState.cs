using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy4DigState : BaseState
{
    Enemy4StateMachine enemyStateMachine;

    Vector3 holderPosition;
    Vector3 playerPosition;

    bool hasDigged;
    Coroutine dig;
    bool isDigging;
    bool digWentWrong;

    // List<Collider> colliders;
    Collider[] colliders;
    

    public Enemy4DigState(Enemy4StateMachine stateMachine) : base("Run", stateMachine) 
    {
        enemyStateMachine = stateMachine;

        //colliders = new List<Collider2D>();
        // contactFilter2D = new ContactFilter2D
        // {
        //     layerMask = LayerMask.GetMask("Collision"),
        //     useLayerMask = true
        // };
    }

    public override void Enter() 
    {
        enemyStateMachine.enemyDamageable.damageable = false;
        isDigging = true;
    }

    public override void UpdateLogic() 
    {
        if(!isDigging)
        {
            hasDigged = false;
            if(digWentWrong)
            {
                stateMachine.ChangeState(enemyStateMachine.idleState);
            }
            else
            {
                stateMachine.ChangeState(enemyStateMachine.attackState);
            }
        }
    }

    public override void UpdatePhysics() 
    {
        if(!hasDigged)
        {
            enemyStateMachine.StartCoroutine(StartDig());
        }
        else
        {
            holderPosition = enemyStateMachine.transform.position;
            playerPosition = enemyStateMachine.playerGameObject.transform.position;

            Vector3 digDirection = (playerPosition - holderPosition).normalized;
            enemyStateMachine.rigidBody.velocity = digDirection * enemyStateMachine.digSpeed;

            colliders = Physics.OverlapSphere(enemyStateMachine.transform.position, 0.5f, LayerMask.GetMask("Collision"));

            if(Vector3.Distance(holderPosition, playerPosition) <= (enemyStateMachine.rangeOfAttack * 0.85))
            {
                if(dig != null)
                {
                    enemyStateMachine.StopCoroutine(dig);
                    enemyStateMachine.StartCoroutine(DigOut());
                }
            }
            else if(colliders.Count() > 0)
            {
                enemyStateMachine.rigidBody.velocity = Vector2.zero;
                enemyStateMachine.StopCoroutine(dig);
                digWentWrong = true;
                enemyStateMachine.StartCoroutine(DigOut());
            }
        }
    }

    IEnumerator StartDig()
    {
        Color previousColor =  enemyStateMachine.bodySpriteRenderer.color;
        //enemyStateMachine.animator;
        yield return new WaitForSeconds(enemyStateMachine.diggingTime);

        enemyStateMachine.bodySpriteRenderer.color = new Color(previousColor.r, previousColor.g, previousColor.b, 0.3f);
        enemyStateMachine.handsSpriteRenderer.color = new Color(previousColor.r, previousColor.g, previousColor.b, 0.3f);

        hasDigged = true;
        digWentWrong = false;
        enemyStateMachine.enemyDamageable.damageable = false;
        

        dig = enemyStateMachine.StartCoroutine(DigTimer());
    }

    IEnumerator DigTimer()
    {
        yield return new WaitForSeconds(enemyStateMachine.maxDigDuration);

        digWentWrong = true;
        enemyStateMachine.StartCoroutine(DigOut());
    }

    IEnumerator DigOut()
    {
        Color previousColor =  enemyStateMachine.bodySpriteRenderer.color;

        yield return new WaitForSeconds(enemyStateMachine.diggingTime);
        
        enemyStateMachine.bodySpriteRenderer.color = new Color(previousColor.r, previousColor.g, previousColor.b, 1f);
        enemyStateMachine.handsSpriteRenderer.color = new Color(previousColor.r, previousColor.g, previousColor.b, 1f);

        isDigging = false;
        enemyStateMachine.enemyDamageable.damageable = true;
        enemyStateMachine.rigidBody.velocity = Vector3.zero;
    }

    public override void Exit() 
    {
        enemyStateMachine.rigidBody.velocity = Vector3.zero;
        enemyStateMachine.enemyDamageable.damageable = true;
        enemyStateMachine.canDig = false;
        enemyStateMachine.StartCoroutine(enemyStateMachine.Cooldown("dig"));
    }
}
