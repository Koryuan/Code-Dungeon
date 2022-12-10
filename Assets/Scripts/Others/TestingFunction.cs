using UnityEngine;

public  class TestingFunction: MonoBehaviour
{
    [SerializeField] private bool m_withoutForeach = false;

    private delegate string Testing();
    private Testing OnTest;

    public void Awake()
    {
        OnTest += test;
        OnTest += test2;

        if (m_withoutForeach) Debug.Log(name + " : " + OnTest.Invoke());
        else foreach(Testing tested in OnTest.GetInvocationList()) Debug.Log(name + tested());
    }

    private string test()
    {
        Debug.Log($"{name} Run this test");
        return "0001";
    }
    private string test2() => "0002";
}
