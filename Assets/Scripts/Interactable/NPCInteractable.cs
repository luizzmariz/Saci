using UnityEngine;

public class NPCInteractable : Interactable
{
    [SerializeField] NPC NPCProfile;
    public string npcName;

    void Awake()
    {
        if(promptMessage == "" || promptMessage == null)
        {
            promptMessage = "interact with ";
        }
    }

    protected override void Interact()
    {
        NPCProfile.StartTalk();
    }

    public override string GetPromptMessage()
    {
        return promptMessage + npcName;
    }
}