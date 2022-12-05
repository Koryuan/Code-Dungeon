using UnityEngine;

[DisallowMultipleComponent]
public abstract class CompileObject : MonoBehaviour
{
    public virtual string CompileCodeReturnString(string[] CodeList)
    {
        throw new System.NotImplementedException();
    }

    public virtual bool CompileCodeReturnBool(string[] CodeList)
    {
        throw new System.NotImplementedException();
    }
}