using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class rand : MonoBehaviour
{
    public int randNum = 0;
    public int min = 0;
    public int max = 0;
    private void Start()
    {

    }
    private void Update()
    {

    }
}

[CustomEditor(typeof(rand))]
public class randComponentUI : Editor
{
    rand randomNum;
    private void OnEnable()
    {
        randomNum = (rand)target;
    }
    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginHorizontal();
        randomNum.min = EditorGUILayout.IntField("最小值", randomNum.min);
        randomNum.max = EditorGUILayout.IntField("最大值", Mathf.Max(randomNum.max, randomNum.min));
        EditorGUILayout.EndHorizontal();
        randomNum.randNum = EditorGUILayout.IntField("随机值", randomNum.randNum);
        if (GUILayout.Button("生成"))
        {
            randomNum.randNum = Random.Range(randomNum.min, randomNum.max + 1);
        }
    }
}