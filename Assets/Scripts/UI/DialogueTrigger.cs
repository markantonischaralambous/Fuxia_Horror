using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("References")]
    public DialogueUI uiScript;

    [Header("Dialogue Settings")]
    public string characterName = "Mysterious NPC";

    [TextArea(3, 10)]
    public string[] dialogueLines;

    [Header("Logic")]
    public bool triggerOnce = true;
    private bool hasTriggered = false;

    private void OnCollisionEnter(Collision collision)
    {
        CheckForPlayer(collision);
    }

    // This acts as a backup in case the first bump was too fast or missed
    private void OnCollisionStay(Collision collision)
    {
        CheckForPlayer(collision);
    }

    private void CheckForPlayer(Collision collision)
    {
        if (!hasTriggered && collision.gameObject.CompareTag("Player"))
        {
            if (uiScript != null)
            {
                uiScript.TriggerDialogue(characterName, dialogueLines);
                if (triggerOnce) hasTriggered = true;
            }
        }
    }
}