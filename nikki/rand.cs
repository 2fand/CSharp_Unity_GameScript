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
        randomNum.min = EditorGUILayout.IntField("��Сֵ", randomNum.min);
        randomNum.max = EditorGUILayout.IntField("���ֵ", Mathf.Max(randomNum.max, randomNum.min));
        EditorGUILayout.EndHorizontal();
        randomNum.randNum = EditorGUILayout.IntField("���ֵ", randomNum.randNum);
        if (GUILayout.Button("����"))
        {
            randomNum.randNum = Random.Range(randomNum.min, randomNum.max + 1);
        }
    }
}