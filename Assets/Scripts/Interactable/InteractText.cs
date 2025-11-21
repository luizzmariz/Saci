using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractText : MonoBehaviour
{
    [SerializeField] private TMP_Text InteractionText;
    void OnEnable()
	{
        PlayerInteract.SetInteractText += ChangeInteractText;
	}

    void OnDisable()
    {
        PlayerInteract.SetInteractText -= ChangeInteractText;
    }
    
    void Start()
    {
        InteractionText.gameObject.SetActive(true);
        InteractionText.text = "";
    }

    void ChangeInteractText(PlayerInput playerInput, Interactable interactable)
    {
        if (interactable == null)
        {
            InteractionText.text = "";
        }
        else
        {
            InteractionText.text = "Press " +
            playerInput.actions["Interact"].GetBindingDisplayString(group: playerInput.currentControlScheme) +
            " to " +
            interactable.GetPromptMessage();
        }
    }
}
