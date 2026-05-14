using UnityEngine;

public class QuestObjective : MonoBehaviour
{
    public int stateToSet = 1; // What chapter does this unlock?

    public bool destroyOnPickup = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Update the brain
            GameManager.instance.storyState = stateToSet;

            Debug.Log("Quest Objective Met! Story State is now: " + stateToSet);

            if (destroyOnPickup)
            {
                Destroy(gameObject);
            }
        }
    }
}