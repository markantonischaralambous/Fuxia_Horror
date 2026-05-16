using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("References")]
    public DialogueUI uiScript;
    public GameObject interactCanvas; // The [E] UI Prompt to talk

    [Header("NPC Settings")]
    public string characterName = "NPC"; // FIXED: Re-added this missing variable!

    [Header("NPC Role (Pick One Only)")]
    public bool isNPC_A = true;       // True for NPC A (Lantern)
    public bool isNPC_B = false;      // True for NPC B (5 Items)
    public bool isFlavorNPC = false;  // True for the 3rd NPC (No Quests)

    [Header("Dialogue Arrays (5 States)")]
    public DialogueLine[] state0Lines; // Game Start / Intro
    public DialogueLine[] state1Lines; // Quest 1 Active (Lantern hunting)
    public DialogueLine[] state2Lines; // Quest 1 Turn-in / Quest 2 Setup
    public DialogueLine[] state3Lines; // Quest 2 Active (5 items hunting)
    public DialogueLine[] state4Lines; // Quest 2 Turn-in
    public DialogueLine[] state5Lines; // Game Complete / Ending Banter

    private DialogueLine[] lastSpokenLines;
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
        DialogueLine[] lines = GetLinesForState(currentState);

        // Safety check to prevent IndexOutOfRangeException (image_604cff.png)
        if (lines == null || lines.Length == 0)
        {
            if (lastSpokenLines != null && lastSpokenLines.Length > 0)
            {
                lines = lastSpokenLines;
            }
            else
            {
                Debug.LogWarning($"{gameObject.name} has nothing to say during State {currentState}. Skip.");
                return;
            }
        }

        lastSpokenLines = lines;
        int nextState = currentState;

        // --- EXPLICIT NARRATIVE PROGRESSION ---
        if (!hasTalkedInCurrentState)
        {
            if (isNPC_A)
            {
                if (currentState == 0) nextState = 1; // Start Quest 1
                if (currentState == 2) nextState = 3; // Turn in Quest 1 -> Unlock Quest 2
                hasTalkedInCurrentState = true;
            }
            else if (isNPC_B)
            {
                if (currentState == 4) nextState = 5; // Turn in Quest 2 -> Finish Game
                hasTalkedInCurrentState = true;
            }
            else if (isFlavorNPC)
            {
                nextState = currentState; // Doesn't alter states
            }
        }

        uiScript.TriggerDialogue(characterName, lines, nextState);

        if (interactCanvas != null) interactCanvas.SetActive(false);
    }

    DialogueLine[] GetLinesForState(int state)
    {
        switch (state)
        {
            case 0: return state0Lines;
            case 1: return state1Lines;
            case 2: return state2Lines;
            case 3: return state3Lines;
            case 4: return state4Lines;
            case 5: return state5Lines;
            default: return null;
        }
    }
}