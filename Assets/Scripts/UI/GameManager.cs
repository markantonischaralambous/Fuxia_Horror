using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // Track the 5 narrative states (0 to 5)
    private int _storyState = 0;

    public int storyState
    {
        get { return _storyState; }
        set
        {
            if (_storyState != value)
            {
                _storyState = value;
                Debug.Log("Story State Updated to: " + _storyState);
                NotifyNPCsOfStateChange();
            }
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Loops through the scene and refreshes NPCs when the state moves forward
    void NotifyNPCsOfStateChange()
    {
        DialogueTrigger[] npcs = FindObjectsByType<DialogueTrigger>(FindObjectsSortMode.None);
        foreach (DialogueTrigger npc in npcs)
        {
            npc.ResetTalkStatus();
        }
    }
}