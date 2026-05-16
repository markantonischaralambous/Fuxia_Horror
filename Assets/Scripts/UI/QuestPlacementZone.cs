using UnityEngine;

public class QuestPlacementZone : MonoBehaviour
{
    [Header("References")]
    public GameObject interactPrompt;   // The [E] UI Canvas or Text above the table
    public GameObject visualAsset;      // The child Cube model that is hidden at start

    [Header("Quest Settings")]
    public int targetStateNeeded = 3;     // Must be on Quest 2 (State 3) to place items
    public int nextStoryState = 4;        // Sets game to State 4 when all 5 are placed

    // SHARED SYSTEM COUNTER
    private static int itemsPlaced = 0;   // 'static' means all tables share this exact counter
    public int totalItemsNeeded = 5;

    private bool isPlayerInRange = false;
    private bool isAlreadyPlaced = false;

    void Start()
    {
        if (interactPrompt != null) interactPrompt.SetActive(false);
        if (visualAsset != null) visualAsset.SetActive(false); // Make sure cube starts hidden

        // Reset the static counter whenever the scene loads fresh
        itemsPlaced = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only trigger if the player walks in, the game is in State 3, and a cube isn't already here
        if (other.CompareTag("Player") && GameManager.instance.storyState == targetStateNeeded && !isAlreadyPlaced)
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
        // Press E to place the item down
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !isAlreadyPlaced)
        {
            PlaceItem();
        }
    }

    void PlaceItem()
    {
        isAlreadyPlaced = true;
        isPlayerInRange = false;

        if (interactPrompt != null) interactPrompt.SetActive(false);

        // VISUAL MAGIC: Make your Blender cube appear on the table!
        if (visualAsset != null)
        {
            visualAsset.SetActive(true);
        }

        itemsPlaced++;
        Debug.Log($"Cube placed! Total on tables: {itemsPlaced} / {totalItemsNeeded}");

        // If this was the 5th and final cube, unlock the turn-in phase for NPC B
        if (itemsPlaced >= totalItemsNeeded)
        {
            Debug.Log("All 5 items placed! Story state updated to 4. Go speak to NPC B.");
            GameManager.instance.storyState = nextStoryState;
        }
    }
}