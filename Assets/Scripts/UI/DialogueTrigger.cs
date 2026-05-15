using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("References")]
    public DialogueUI uiScript;
    public GameObject interactCanvas;

    [Header("NPC Info")]
    public string characterName = "NPC";

    [Header("Dialogue Variations")]
    // Changed from string[] to DialogueLine[] to fix error in image_f9065c.png
    public DialogueLine[] introLines;
    public DialogueLine[] questActiveLines;
    public DialogueLine[] questDoneLines;

    [Header("Quest Settings")]
    public bool givesQuest = false;
    public int stateToSetOnTalk = 1;

    [Header("Repetition Handling")]
    public DialogueLine[] reminderLines;

    [HideInInspector] public bool hasTalkedInCurrentState = false;
    private bool isInRange = false;

    public void ResetTalkStatus() => hasTalkedInCurrentState = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = true;
            if (interactCanvas != null) interactCanvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
            if (interactCanvas != null) interactCanvas.SetActive(false);
            if (uiScript.isDialogueActive) uiScript.EndDialogue();
        }
    }

    void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.E) && !uiScript.isDialogueActive)
        {
            HandleInteraction();
        }
    }

    void HandleInteraction()
    {
        int currentState = GameManager.instance.storyState;

        if (!hasTalkedInCurrentState)
        {
            DialogueLine[] lines = GetLinesForState(currentState);

            int nextState = currentState;
            if (givesQuest && currentState < stateToSetOnTalk)
            {
                nextState = stateToSetOnTalk;
            }

            // Now sending the correct DialogueLine[] type
            uiScript.TriggerDialogue(characterName, lines, nextState);
            hasTalkedInCurrentState = true;
        }
        else
        {
            uiScript.TriggerDialogue(characterName, reminderLines, currentState);
        }

        if (interactCanvas != null) interactCanvas.SetActive(false);
    }

    DialogueLine[] GetLinesForState(int state)
    {
        if (state == 1) return questActiveLines;
        if (state >= 2) return questDoneLines;
        return introLines;
    }
}