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
                dialogues[relationLevel].dialogueSprite);

                dialogues[relationLevel].hasBeenSpoke = true;
                
                if(dialogues[relationLevel].increaseRelationLevel)
                {
                    relationLevel++;
                }
            }
            else
            {
                DialogueManager.instance.StartDialogue(npcName, dialogues[relationLevel].posInteractionSentences,
                dialogues[relationLevel].dialogueSprite);
            }
        }
    }

    [System.Serializable]
    public struct Dialogue
    {
        public bool hasBeenSpoke;
        public Sprite dialogueSprite;
        public bool increaseRelationLevel;
        [TextArea(3, 10)] public string[] interactionSentences;
        [TextArea(3, 10)] public string[] posInteractionSentences;
    }
}
