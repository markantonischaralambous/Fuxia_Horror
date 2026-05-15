using UnityEngine;

[System.Serializable]
public struct DialogueLine
{
    public string actorName;
    [TextArea(3, 10)]
    public string text;
}