using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class EndScreenText : MonoBehaviour
{
    [SerializeField]
    private SeasonsSystemURP system;

    [SerializeField]
    private TextMeshProUGUI springText;
    [SerializeField]
    private TextMeshProUGUI summerText;
    [SerializeField]
    private TextMeshProUGUI autumnText;
    [SerializeField]
    private TextMeshProUGUI winterText;

    private void OnEnable()
    {
        LoadText();
    }

    public void LoadText()
    {
        float[] times = system.GetTimes();
        springText.text = $"Spring {Mathf.RoundToInt(times[0])} s";
        summerText.text = $"Summer {Mathf.RoundToInt(times[1])} s";
        autumnText.text = $"Autumn {Mathf.RoundToInt(times[2])} s";
        winterText.text = $"Winter {Mathf.RoundToInt(times[3])} s";
    }
}

[CustomEditor(typeof(EndScreenText))]
public class EndScreenTextEditor : Editor
{
    private EndScreenText end;

    public void OnEnable()
    {
        end = (EndScreenText)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Load text"))
        {
            end.LoadText();
        }
    }
}