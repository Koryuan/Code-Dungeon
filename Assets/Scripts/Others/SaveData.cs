using System.Collections.Generic;

public class SaveData
{
    public string lastSceneName = "";
    public List<Item> _itemList = new List<Item>();

    public SaveDataTutorialScene TutorialScene = new SaveDataTutorialScene();
}

public class SaveDataTutorialScene
{
    public bool JustAwake;
    public bool InteractionQuideInteracted = false;
    public bool TakeTablet = false;
    public bool UseNextSceneDoor = false;

    public SaveDataTutorialScene()
    {
        JustAwake = true;
    }
}