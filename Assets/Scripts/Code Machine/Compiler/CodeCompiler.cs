﻿using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent] public class CodeCompiler : MonoBehaviour
{
    [System.Serializable] public struct TargetOutput
    {
        [SerializeField] private string[] m_targetTexts;
        [SerializeField] private int m_occurence;
        [SerializeField] private GameEvent[] m_onTargetEvent;

        public int OccurenceNumber => m_occurence;
        public GameEvent[] OnTarget => m_onTargetEvent;
        public void Occur() => m_occurence -= 1;
        public void LoadData(int Occurence) => m_occurence = Occurence;
        public (bool, string[]) IsTargetUpdateColor(string[] TextOutput)
        {
            //Debug.Log(TextOutput.Length);
            // Validation Check
            int breakNumber = 0; bool isTrue = false;
            for (int i = 0; i < m_targetTexts.Length; i++)
            {
                if (isTrue = TextOutput.Length <= (breakNumber = i)) break;
                if (isTrue = (TextOutput[i] != m_targetTexts[i])) break;
                else TextOutput[i] = StringList.ColorStringNoBack(TextOutput[i],StringList.Color_LightGreen);
            }
            isTrue = !isTrue;

            // Check the last data
            List<string> NoOutput = new List<string>();
            for(int i = breakNumber; i< m_targetTexts.Length; i++)
            {
                if (TextOutput.Length <= i) NoOutput.Add(StringList.ColorStringNoBack("NO INPUT", StringList.Color_Red));
                else if (TextOutput[i] != m_targetTexts[i]) TextOutput[i] = StringList.ColorStringNoBack(TextOutput[i], StringList.Color_Red);
                else TextOutput[i] = StringList.ColorStringNoBack(TextOutput[i], StringList.Color_LightGreen);
                //Debug.Log($"{i}: {TextOutput[i]}");
            }
            //Debugging(TextOutput);
            return (isTrue,TextOutput);
        }
        private void Debugging(string[] Output)
        {
            foreach (string text in Output) Debug.Log(text);
        }
    }
    [SerializeField] protected TargetOutput[] m_targetList;
    [SerializeField] protected GameStateChannel m_channel;
    public CompilerSaveData SaveData { get; private set; } = new CompilerSaveData();
    public System.Action<CompilerSaveData> OnUpdate;
    public void Initialize()
    {
        for(int i = 0;i < m_targetList.Length;i++)
        {
            CompilerSaveData.OccurenceData Data = new CompilerSaveData.OccurenceData();
            Data.Index = i;
            Data.Occurence = m_targetList[i].OccurenceNumber;
        }
    }
    public void LoadData(CompilerSaveData LoadedData)
    {
        if (LoadedData == null) return;
        
        SaveData = LoadedData;
        for(int i = 0;i < m_targetList.Length;i++)
        {
            int Occurence = 0;
            if (SaveData.OccurenceList.Count < i) continue;

            if (SaveData.OccurenceList.Count > i) Occurence = SaveData.OccurenceList[i].Occurence;
            m_targetList[i].LoadData(Occurence);
        }
    }

    public virtual string[] PrintCompile(string[] InputFieldData)
    {
        string[] output = {"ERROR"};
        return output;
    }
    public virtual string[] ScanCompile(string[] InputFieldData, string[] ScanData)
    {
        string[] output = {"ERROR"};
        return output;
    }
    public string[] CheckText(string[] Output)
    {
        if (Output == null || Output.Length == 0 || m_targetList.Length == 0) 
            return new[] { StringList.ColorStringNoBack("ERROR", StringList.Color_Red) };
        for (int i = 0;i < m_targetList.Length;i++)
        {
            (bool IsTarget, string[] StringList) Result = m_targetList[i].IsTargetUpdateColor(Output);
            if (Result.IsTarget)
            {
                Debug.Log($"Compile: It was target {Result.StringList}");
                if (m_targetList[i].OccurenceNumber == 0) return Result.StringList;

                m_targetList[i].Occur();
                if (SaveData.OccurenceList.Count > i)
                {
                    SaveData.OccurenceList[i].Occurence = m_targetList[i].OccurenceNumber;
                    OnUpdate?.Invoke(SaveData);
                }
                if (m_channel) m_channel.RaiseGameEventPassed(m_targetList[i].OnTarget);
                return Result.StringList;
            } 
            else if (i == m_targetList.Length - 1) return Result.StringList;
        }

        Debug.Log("Something doesn't right with the compiler");
        return new[] { StringList.ColorStringNoBack("ERROR",StringList.Color_Red) };
    }
    protected virtual string GetArithmaticValueINT(int Value1, int Value2, string Input) => (Input) switch
    {
        "+" => (Value1 + Value2).ToString(),
        "-" => (Value1 - Value2).ToString(),
        "*" => (Value1 * Value2).ToString(),
        "/" => (Value1 / Value2).ToString(),
        "MOD" => (Value1 % Value2).ToString(),
        _ => "ERROR"
    };
}

[System.Serializable] public class CompilerSaveData
{
    [System.Serializable] public class OccurenceData
    {
        public int Index;
        public int Occurence;
    }
    public List<OccurenceData> OccurenceList = new List<OccurenceData>();
}