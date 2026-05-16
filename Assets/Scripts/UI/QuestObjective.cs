using UnityEngine;

public class QuestObjective : MonoBehaviour
{
    [Header("References")]
    public GameObject interactPrompt; // The [E] UI prompt over the item

    [Header("Quest Settings")]
    public bool isMultiItemQuest = false;
    public int targetStateNeeded = 1;     // What state must the game be in to pick this up?
    public int nextStoryState = 2;        // What state to set when completely finished?

    [Header("Multi-Item Counter Settings")]
    private static int itemsCollected = 0; // Shared across all items
    public int totalItemsNeeded = 5;

    private bool isPlayerInRange = false;

    void Start()
    {
        if (interactPrompt != null) interactPrompt.SetActive(false);

        // Reset counter when the scene loads
        if (!isMultiItemQuest) itemsCollected = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only let the player interact if the game is in the correct narrative state
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
        // Fixes your requirement: Must press E to pick up
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
            // Quest 1: Lantern (Single Item)
            Debug.Log("Picked up the Lantern!");
            GameManager.instance.storyState = nextStoryState;
            Destroy(gameObject); // Remove item from world
        }
        else
        {
            // Quest 2: 5 Items
            itemsCollected++;
            Debug.Log("Collected item " + itemsCollected + "/" + totalItemsNeeded);

            if (itemsCollected >= totalItemsNeeded)
            {
                Debug.Log("All 5 items collected! Head back to NPC B.");
                GameManager.instance.storyState = nextStoryState;
            }

            Destroy(gameObject); // Remove this specific item instance
        }
    }
}