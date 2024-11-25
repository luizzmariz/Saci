using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    #region Components 
    PlayerStateMachine playerStateMachine;
    PlayerAbilityHolder playerAbilityHolder;
    CharacterOrientation playerOrientation;

    [HideInInspector] public PlayerInput playerInput;
    #endregion

    int attackdebugindex = 0;

    public enum PlayerControlScheme
    {
        keyboardAndMouse,
        gamepad,
        other
    }

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
        if(playerAbilityHolder == null)
        {
            playerAbilityHolder = GetComponent<PlayerAbilityHolder>();
        }
        if(playerInput == null)
        {
            playerInput = GetComponent<PlayerInput>();
        }
        if(playerOrientation == null)
        {
            playerOrientation = GetComponent<CharacterOrientation>();
        }
    }

    public Vector3 GetPlayerAimVector()
    {
        Vector3 targetPoint = this.transform.position;

        if(GetPlayerControlScheme() == PlayerControlScheme.keyboardAndMouse)
        {
            Plane playerPlane = new Plane(Vector3.up, new Vector3(0, targetPoint.y, 0));
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float hitDist;

            Debug.DrawRay(ray.origin, ray.direction * 50, Color.blue, 50);

            if(playerPlane.Raycast(ray, out hitDist))
            {
                targetPoint = ray.GetPoint(hitDist);
            }
        }
        else if(GetPlayerControlScheme() == PlayerControlScheme.gamepad)
        {
            Vector3 lookDirection = playerOrientation.lastOrientation;
            if(playerStateMachine.isAiming)
            {
                Vector2 aimVector2 = playerInput.actions["move"].ReadValue<Vector2>();
                lookDirection.x = aimVector2.x;
                lookDirection.z = aimVector2.y;
            }

            targetPoint = new Vector3(targetPoint.x + lookDirection.x * 10, targetPoint.y, targetPoint.z + lookDirection.z * 10);
        }

        return targetPoint;
    }

    public PlayerControlScheme GetPlayerControlScheme()
    {
        if(playerInput.currentControlScheme == "Keyboard&Mouse")
        {
            return PlayerControlScheme.keyboardAndMouse;
        }
        else if(playerInput.currentControlScheme == "Gamepad")
        {
            return PlayerControlScheme.gamepad;
        }
        else
        {
            return PlayerControlScheme.other;
        }
    }

    #region Player Input Events 
    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.performed && playerAbilityHolder.meleeAttack != null)
        {
            if(playerAbilityHolder.CheckIfCanUseAbility(playerAbilityHolder.meleeAttack))
            {
                playerStateMachine.ChangeToAttackState();
                playerAbilityHolder.meleeAttack.Activate();
            }
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if(context.performed && playerAbilityHolder.rangedAttack != null)
        {
            if(playerAbilityHolder.CheckIfCanUseAbility(playerAbilityHolder.rangedAttack))
            {
                attackdebugindex++;
                //Debug.Log("PlayerInput: " + attackdebugindex);

                playerStateMachine.ChangeToFireState();
                playerAbilityHolder.UseAbility(playerAbilityHolder.rangedAttack);
            }
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if(context.performed && playerAbilityHolder.dash != null)
        {
            if(playerAbilityHolder.CheckIfCanUseAbility(playerAbilityHolder.dash))
            {
                 playerStateMachine.ChangeToDashState();
                playerAbilityHolder.UseAbility(playerAbilityHolder.dash);
            }
        }
    }
    #endregion
}
