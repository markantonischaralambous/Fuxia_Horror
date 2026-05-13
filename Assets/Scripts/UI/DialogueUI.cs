using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class DialogueUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public float typingSpeed = 0.05f;

    private Queue<string> lines = new Queue<string>();
    private Coroutine typingCoroutine;
    private string fullCurrentLine; // Stores the full sentence
    private bool isTyping = false; // Tracks if we are currently "crawling" text

    void Awake()
    {
        gameObject.SetActive(false);
    }

    public void TriggerDialogue(string name, string[] dialogueLines)
    {
        gameObject.SetActive(true);
        nameText.text = name;

        lines.Clear();
        foreach (string line in dialogueLines)
        {
            lines.Enqueue(line);
        }

        NextLine();
    }

    public void NextLine()
    {
        // If we are currently typing, SKIP to the end of the sentence
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = fullCurrentLine;
            isTyping = false;
            return;
        }

        // If we are NOT typing, try to get the next sentence
        if (lines.Count == 0)
        {
            gameObject.SetActive(false);
            return;
        }

        fullCurrentLine = lines.Dequeue();
        typingCoroutine = StartCoroutine(TypeLine(fullCurrentLine));
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    void Update()
    {
        // Handle clicking/continuing
        if (Input.GetMouseButtonDown(0))
        {
            NextLine();
        }
    }
}