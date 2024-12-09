using UnityEngine;

[System.Serializable]

public class Dialogue
{
    [TextArea(2, 10)]
    public string[] npcGreetings;

    [TextArea(2, 10)]
    public string[] areYouStillHere;

    [TextArea(2, 10)]
    public string[] wellDone;
}
