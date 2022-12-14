using System;
using UnityEngine;

public abstract class AutoSaveAttach : MonoBehaviour
{
    [SerializeField] protected SaveChannel m_saveChannel;

    public Action<SaveDataAuto> OnDataLoaded;
}

[System.Serializable] public class SaveDataAuto
{
    public string ID;
    public bool New = true;
}