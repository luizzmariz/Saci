using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DialogueManager : MonoBehaviour {

	[Header("Dialogue Manager")]
	[HideInInspector] public static DialogueManager instance = null;
	public Animator animator;
	public static event Action<bool> DialogueWindowChanged;
	public bool isHappening;
	public bool inTransition;
	bool eventTrigger;

	[Header("Speaker Info")]
	public TMP_Text nameText;
	public TMP_Text dialogueText;
    public Image dialogueImage;

	[Header("Dialogue Stats")]	
	private Queue<string> sentences;
    private string currentSentence;
    public float typingSpeed;
	public bool isTyping;

	// void OnEnable()
	// {

	// }

    // void OnDisable()
    // {
        
    // }

    void Start() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);
		}

		sentences = new Queue<string>();
	}

	public void StartDialogue(string speakerName, string[] dialogue, Sprite speakerSprite, bool isTrigger)
	{	
		DialogueWindowChanged?.Invoke(true);
		inTransition = true;
		isHappening = true;

		animator.SetBool("dialogue", true);

		nameText.text = speakerName;
        dialogueImage.sprite = speakerSprite;

		dialogueText.text = "";

		eventTrigger = isTrigger;

		sentences.Clear();

		foreach (string sentence in dialogue)
		{
			sentences.Enqueue(sentence);
		}

		// DisplayNextSentence();
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
        if (eventTrigger)
        {
            LevelManager.instance.InvokeEncounterEvent();
            eventTrigger = false;
        }

		DialogueWindowChanged?.Invoke(false);
		inTransition = true;

		animator.SetBool("dialogue", false);
	}

	public void DialogueTransitionEnd()
    {
        inTransition = false;

		if(animator.GetBool("dialogue"))
        {
			DisplayNextSentence();
        }
		else
        {
			dialogueText.text = "";
			isHappening = false;
        }
    }
}