using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour {

	[Header("Dialogue Manager")]
	public static DialogueManager instance = null;
	public Animator animator;


	[Header("Speaker Info")]
	public TMP_Text nameText;
	public TMP_Text dialogueText;
    public Image dialogueImage;


	[Header("Dialogue Stats")]	
	private Queue<string> sentences;
    private string currentSentence;
    public float typingSpeed;
    public bool isTyping;
	

	[Header("Player")]
	public GameObject player;
	public PlayerStateMachine playerStateMachine;

	void Start() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);
		}

		sentences = new Queue<string>();
		player = GameObject.Find("Player");
		playerStateMachine = player.GetComponent<PlayerStateMachine>();
	}

	public void StartDialogue(string speakerName, string[] dialogue, Sprite speakerSprite)
	{	
		// playerStateMachine.ChangeState(playerStateMachine.interactState);
		animator.SetBool("DialogueBoxIsOpen", true);

		nameText.text = speakerName;
        dialogueImage.sprite = speakerSprite;

		sentences.Clear();

		foreach (string sentence in dialogue)
		{
			sentences.Enqueue(sentence);
		}

		DisplayNextSentence();
	}

	public void DisplayNextSentence()
	{
		if (sentences.Count == 0 && !isTyping)
		{
			EndDialogue();
			return;
		}

        StopAllCoroutines();
        
        if(isTyping)
        {
            FinishTypingEarly(currentSentence);
        }
        else if(!isTyping)
        {
            currentSentence = sentences.Dequeue();
		    StartCoroutine(TypeSentence(currentSentence));
        }
	}

	IEnumerator TypeSentence(string sentence)
	{
        isTyping = true;

        int maxVisibleChars = 0;

		dialogueText.text = sentence;
        dialogueText.maxVisibleCharacters = maxVisibleChars;

		foreach(char letter in sentence.ToCharArray())
		{
            maxVisibleChars++;
            dialogueText.maxVisibleCharacters = maxVisibleChars;

            yield return new WaitForSecondsRealtime(typingSpeed);
		}

        isTyping = false;
	}

    private void FinishTypingEarly(string sentence)
    {
        dialogueText.text = sentence;
        dialogueText.maxVisibleCharacters = sentence.Length;

        isTyping = false;
    }

	void EndDialogue()
	{
		animator.SetBool("DialogueBoxIsOpen", false);

		// playerStateMachine.interactState.ExitState();
		// LevelManager.instance.currentLevelPart.SendActionTokens();
	}
}