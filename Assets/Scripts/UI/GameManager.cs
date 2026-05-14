using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // This makes it show up in your Inspector (Screenshot 2026-05-14 214918.png)
    [SerializeField] private int _storyState = 0;

    public int storyState
    {
        get { return _storyState; }
        set
        {
            _storyState = value;
            NotifyNPCsOfStateChange();
        }
    }

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void NotifyNPCsOfStateChange()
    {
        DialogueTrigger[] npcs = FindObjectsOfType<DialogueTrigger>();
        foreach (DialogueTrigger npc in npcs)
        {
            npc.ResetTalkStatus();
        }
    }
}