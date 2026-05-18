using UnityEngine;

public class QuestPlacementZone : MonoBehaviour
{
    [Header("References")]
    public GameObject interactPrompt;
    public GameObject visualAsset;

    [Header("Quest Settings")]
    public int targetStateNeeded = 3;
    public int nextStoryState = 4;

    // GLOBAL STATICS
    private static int totalTablesFilled = 0;
    public int totalItemsNeeded = 5;

    private bool isPlayerInRange = false;
    private bool isAlreadyPlaced = false;

    void Start()
    {
        if (interactPrompt != null) interactPrompt.SetActive(false);
        if (visualAsset != null) visualAsset.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if game is in State 3, table is empty, AND player has an item in hand
        if (other.CompareTag("Player") && GameManager.instance.storyState == targetStateNeeded && !isAlreadyPlaced)
        {
            if (QuestObjective.playerInventoryCount > 0)
            {
                isPlayerInRange = true;
                if (interactPrompt != null) interactPrompt.SetActive(true);
            }
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
        // Only allow placement if player is holding something
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !isAlreadyPlaced)
        {
            if (QuestObjective.playerInventoryCount > 0)
            {
                PlaceItem();
            }
        }
    }

    void PlaceItem()
    {
        isAlreadyPlaced = true;
        isPlayerInRange = false;
        if (interactPrompt != null) interactPrompt.SetActive(false);

        // Take 1 item out of player's hands
        QuestObjective.playerInventoryCount--;

        // Show the item on the table
        if (visualAsset != null) visualAsset.SetActive(true);

        totalTablesFilled++;
        Debug.Log($"Placed on table! Total filled: {totalTablesFilled} / {totalItemsNeeded}. Remaining in pockets: {QuestObjective.playerInventoryCount}");

        // If this was the final table, trigger NPC B's turn-in state
        if (totalTablesFilled >= totalItemsNeeded)
        {
            Debug.Log("All 5 tables filled one-by-one! Head back to NPC B.");
            GameManager.instance.storyState = nextStoryState;
        }
    }
}