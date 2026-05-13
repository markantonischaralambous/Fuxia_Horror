using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueUI uiScript;
    public GameObject interactCanvas; // NEW: Drag your [E] InteractCanvas here
    public string characterName = "Mysterious NPC";
    [TextArea(3, 10)]
    public string[] dialogueLines;

    private bool isInRange = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = true;
            Debug.Log("Player is in range to talk!");

            // Show the [E] prompt when entering range
            if (interactCanvas != null)
            {
                interactCanvas.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
            Debug.Log("Player left the range.");

            // Hide the [E] prompt when leaving range
            if (interactCanvas != null)
            {
                interactCanvas.SetActive(false);
            }
        }
    }

    void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (uiScript != null)
            {
                uiScript.TriggerDialogue(characterName, dialogueLines);

                // Optional: Hide the [E] prompt while the actual dialogue box is open
                if (interactCanvas != null)
                {
                    interactCanvas.SetActive(false);
                }
            }
        }
    }
}