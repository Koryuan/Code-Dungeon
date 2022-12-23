using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Code Machine/Readline Update")]
public class ReadonlyUpdate : ScriptableObject
{
    public int IndexToUpdate = -1;
    [TextArea(10, 50)] public string UpdatedText = string.Empty;
}