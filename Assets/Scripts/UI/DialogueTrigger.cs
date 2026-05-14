using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("References")]
    public DialogueUI uiScript;        // Reference to the UI manager
    public GameObject interactCanvas; // The [E] prompt UI

    [Header("NPC Info")]
    public string characterName = "NPC";

    [Header("Dialogue Variations")]
    [TextArea(3, 10)] public string[] introLines;      // State 0
    [TextArea(3, 10)] public string[] questActiveLines; // State 1
    [TextArea(3, 10)] public string[] questDoneLines;   // State 2

    [Header("Quest Settings")]
    public bool givesQuest = false;
    public int stateToSetOnTalk = 1;

    [Header("Repetition Handling")]
    [TextArea(2, 5)] public string[] reminderLines = { "I've told you all I know for now." };

    // Hidden because GameManager manages this via ResetTalkStatus()
    [HideInInspector] public bool hasTalkedInCurrentState = false;

    private bool isInRange = false;

    // This fixes error CS1061 from image_7600b8.png
    public void ResetTalkStatus()
    {
        hasTalkedInCurrentState = false;
    }

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

            // If they walk away, we should make sure the UI closes too
            if (uiScript.isDialogueActive) uiScript.EndDialogue();
        }
    }

    void Update()
    {
        // Check for 'E' press, range, and ensure UI isn't already busy
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
            // Determine which lines to show based on the Brain (GameManager)
            string[] lines = GetLinesForState(currentState);

            // Determine the next state. 
            // If this NPC gives a quest and we are in state 0, move to state 1.
            int nextState = currentState;
            if (givesQuest && currentState < stateToSetOnTalk)
            {
                nextState = stateToSetOnTalk;
            }

            // Call UI with 3 arguments (Fixes error CS1501 from image_7607c3.png)
            uiScript.TriggerDialogue(characterName, lines, nextState);
            hasTalkedInCurrentState = true;
        }
        else
        {
            // If we already talked, just show the short reminder
            uiScript.TriggerDialogue(characterName, reminderLines, currentState);
        }

        // Hide the [E] prompt while talking
        if (interactCanvas != null) interactCanvas.SetActive(false);
    }

    string[] GetLinesForState(int state)
    {
        switch (state)
        {
            case 1: return questActiveLines;
            case 2: return questDoneLines;
            default: return introLines;
        }
    }
}