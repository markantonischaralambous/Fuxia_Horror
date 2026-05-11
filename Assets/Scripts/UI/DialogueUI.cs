using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class DialogueUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    private Queue<string> lines = new Queue<string>();

    // 1. This hides the UI correctly when the game starts
    void Awake()
    {
        gameObject.SetActive(false);
    }

    // 2. This is called by the Cube when you bump it
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

    // 3. This moves to the next sentence
    public void NextLine()
    {
        if (lines.Count == 0)
        {
            gameObject.SetActive(false);
            return;
        }

        dialogueText.text = lines.Dequeue();
    }

    // 4. This listens for clicks to progress the text
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            NextLine();
        }
    }
}