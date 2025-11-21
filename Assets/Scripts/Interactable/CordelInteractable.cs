using UnityEngine;

public class CordelInteractable : Interactable
{
    protected override void Interact()
    {
        Debug.Log("You interacted with " + promptMessage);
    }

    public override string GetPromptMessage()
    {
        return "interact with " + promptMessage;
    }
}