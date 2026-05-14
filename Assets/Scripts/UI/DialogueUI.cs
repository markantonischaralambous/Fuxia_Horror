using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    [Header("Settings")]
    public float typingSpeed = 0.04f;

    [HideInInspector] public bool isDialogueActive = false;
    private string[] currentLines;
    private int lineIndex;
    private int pendingEndState;
    private bool isTyping = false;
    private bool canProcessInput = false; // Prevents instant skipping

    void Awake()
    {
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void TriggerDialogue(string characterName, string[] lines, int endState)
    {
        if (dialoguePanel == null) return;

        isDialogueActive = true;
        canProcessInput = false; // Disable input for a split second
        pendingEndState = endState;
        currentLines = lines;
        lineIndex = 0;
        nameText.text = characterName;

        dialoguePanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        StartCoroutine(TypeLine());

        // Brief delay before the player can skip/progress
        Invoke("EnableInput", 0.1f);
    }

    void EnableInput() => canProcessInput = true;

    void Update()
    {
        if (!isDialogueActive || !canProcessInput) return;

        // Check for 'E' key OR Left Mouse Click (0)
        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = currentLines[lineIndex];
                isTyping = false;
            }
            else
            {
                NextLine();
            }
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char c in currentLines[lineIndex].ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    void NextLine()
    {
        if (lineIndex < currentLines.Length - 1)
        {
            lineIndex++;
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
        if (dialoguePanel != null) dialoguePanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (GameManager.instance != null)
        {
            GameManager.instance.storyState = pendingEndState;
        }
    }
}