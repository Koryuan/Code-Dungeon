using UnityEngine;

[CreateAssetMenu(menuName = "Dialog", order = 99)]
public class DialogSetting : ScriptableObject
{
    [SerializeField] private Dialog[] dialogs;

    public Dialog[] Dialogs => dialogs;

    public void AddItemName(string ItemName)
    {
        foreach (Dialog dialog in dialogs) dialog.ItemName = ItemName;
    }
}

[System.Serializable] public class Dialog
{
    [SerializeField] private string name;
    [SerializeField] [TextArea(10,50)] private string text;

    public string ItemName { get; set; } = "No Name";

    public (string, string) DialogDetail => (name, Text);
    public string Text
    {
        get
        {
            string newText = text;
            newText.Replace("<item>",ItemName);
            return newText;
        }
    }
}