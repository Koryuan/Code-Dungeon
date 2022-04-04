using UnityEngine;
using TMPro;

public class ActionListView : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text actionList_Text;

    [Header("References")]
    [SerializeField] private TestObject test_Object;
    [SerializeField] private InputAbleManager input_Manager;

    private void Awake()
    {
        test_Object.OnFunctionActivated += UpdateTestList;
        input_Manager.OnWrongCodeLine += UpdateTestList;
    }

    private void UpdateTestList(string text)
    {
        if (actionList_Text) actionList_Text.text += text + "\n";
    }
}
