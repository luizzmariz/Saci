using UnityEngine;

public class NPCInteractable : Interactable
{
    [SerializeField] NPC NPCProfile;
    protected override void Interact()
    {
        NPCProfile.StartTalk();
    }

    public override string GetPromptMessage()
    {
        return "interact with " + promptMessage;
    }
}