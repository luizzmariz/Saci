using System;
using UnityEngine;

[System.Serializable]
public class NPC : MonoBehaviour
{
    public string npcName;
    public int relationLevel;
    public Dialogue[] dialogues;

    public void UpgradeRelationLevel()
    {
        relationLevel++;
    }

    public void StartTalk()
    {
        if(!DialogueManager.instance.isHappening)
        {
            if(!dialogues[relationLevel].hasBeenSpoke)
            {
                DialogueManager.instance.StartDialogue(npcName, dialogues[relationLevel].interactionSentences, 
                dialogues[relationLevel].dialogueSprite, dialogues[relationLevel].isTrigger);

                dialogues[relationLevel].hasBeenSpoke = true;
                
                // if(dialogues[relationLevel].isTrigger)
                // {
                //     LevelManager.EncounterTrigger?.Invoke();
                // }

                if(dialogues[relationLevel].increaseRelationLevel)
                {
                    relationLevel++;
                }
            }
            else
            {
                DialogueManager.instance.StartDialogue(npcName, dialogues[relationLevel].posInteractionSentences,
                dialogues[relationLevel].dialogueSprite, dialogues[relationLevel].isTrigger);
            }
        }
    }

    [System.Serializable]
    public struct Dialogue
    {
        public bool hasBeenSpoke;
        public Sprite dialogueSprite;
        public bool increaseRelationLevel;
        public bool isTrigger;
        [TextArea(3, 10)] public string[] interactionSentences;
        [TextArea(3, 10)] public string[] posInteractionSentences;
    }
}
