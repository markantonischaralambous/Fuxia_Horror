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
    private DialogueLine[] currentLines;
    private int lineIndex;
    private int pendingEndState;
    private bool isTyping = false;
    private bool canProcessInput = false;

    void Awake()
    {
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void TriggerDialogue(string characterName, DialogueLine[] lines, int endState)
    {
        if (dialoguePanel == null) return;

        isDialogueActive = true;
        canProcessInput = false;
        pendingEndState = endState;
        currentLines = lines;
        lineIndex = 0;

        dialoguePanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        StartCoroutine(DisplayCurrentLine());
        Invoke("EnableInput", 0.1f);
    }

    void EnableInput() => canProcessInput = true;

    void Update()
    {
        if (!isDialogueActive || !canProcessInput) return;

        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = currentLines[lineIndex].text;
                isTyping = false;
            }
            else
            {
                NextLine();
            }
        }
    }

    IEnumerator DisplayCurrentLine()
    {
        isTyping = true;
        dialogueText.text = "";
        nameText.text = currentLines[lineIndex].actorName;

        foreach (char c in currentLines[lineIndex].text.ToCharArray())
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
            StartCoroutine(DisplayCurrentLine());
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