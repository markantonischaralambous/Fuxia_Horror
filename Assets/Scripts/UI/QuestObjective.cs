using UnityEngine;

public class QuestObjective : MonoBehaviour
{
    [Header("References")]
    public GameObject interactPrompt;

    [Header("Quest Settings")]
    public bool isMultiItemQuest = false;
    public int targetStateNeeded = 1;

    // THE LIVE INVENTORY: Tracks how many items the player is holding RIGHT NOW
    public static int playerInventoryCount = 0;

    private bool isPlayerInRange = false;

    void Start()
    {
        if (interactPrompt != null) interactPrompt.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.instance.storyState == targetStateNeeded)
        {
            isPlayerInRange = true;
            if (interactPrompt != null) interactPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (interactPrompt != null) interactPrompt.SetActive(false);
        }
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            PickUpItem();
        }
    }

    void PickUpItem()
    {
        if (interactPrompt != null) interactPrompt.SetActive(false);

        if (!isMultiItemQuest)
        {
            // Quest 1: Lantern
            GameManager.instance.storyState = 2; // Move straight to turn-in
            Destroy(gameObject);
        }
        else
        {
            // Quest 2: Single pickup tracking
            playerInventoryCount++;
            Debug.Log($"Picked up a cube! Carrying: {playerInventoryCount}");
            Destroy(gameObject);
        }
    }
}