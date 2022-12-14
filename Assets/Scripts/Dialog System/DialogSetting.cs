using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Dialog/Dialog Setting")]
public class DialogSetting : ScriptableObject
{
    [SerializeField] private Dialog[] dialogs;

    public Dialog[] Dialogs => dialogs;

    public void AddItemName(string ItemName)
    {
        foreach (Dialog dialog in dialogs) dialog.ItemName = ItemName;
    }
    public void AddHelpname(string HelpName)
    {
        foreach (Dialog dialog in dialogs) dialog.HelpName = HelpName;
    }
}

[System.Serializable] public class Dialog
{
    [SerializeField] private string name;
    [SerializeField] [TextArea(10,50)] private string text;
    [SerializeField] private DialogImage m_image;

    public string ItemName { get; set; } = "No Name";
    public string HelpName { get; set; } = "No Name";

    public Sprite Image => m_image.Image;
    public (string Name, string Text) DialogDetail => (name, Text);
    public Vector2 ImageScale => new Vector2(m_image.Width, m_image.Height);
    public float Y_Offset => m_image.Y_Offset;
    public string Text
    {
        get
        {
            string newText = text.Replace(StringList.DialogItemCode, ItemName);
            return newText.Replace(StringList.DialogHelpCode, HelpName);
        }
    }
}

[System.Serializable] public class DialogImage
{
    public Sprite Image;
    public float Width;
    public float Height;
    public float Y_Offset;
}