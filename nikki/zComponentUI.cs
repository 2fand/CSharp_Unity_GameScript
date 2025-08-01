using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(z))]
public class zComponentUI : Editor
{
    z z;
    private Vector2Int pos;
    private int textsize;
    private void OnEnable()
    {
        z = (z)target;
    }
    public override void OnInspectorGUI()
    {
        z.haveFront = EditorGUILayout.Toggle("�Ƿ����ӷ���", z.haveFront);
        z.mod = (z.mode)EditorGUILayout.EnumPopup("����ģʽ", z.mod);
        switch (z.mod) {
            case z.mode.tele:
                z.teleWaitTime = EditorGUILayout.FloatField("����ʱ�ȴ�ʱ��", z.teleWaitTime);
                z.worldName = EditorGUILayout.TextField("������������", z.worldName);
                z.openSound = (AudioClip)EditorGUILayout.ObjectField("���Ϳ�ʼǰ��Ч", z.openSound, typeof(AudioClip), false);
                z.closeSound = (AudioClip)EditorGUILayout.ObjectField("���ͽ�������Ч", z.closeSound, typeof(AudioClip), false);
                pos = new Vector2Int(z.teleX, z.teleY);
                pos = EditorGUILayout.Vector2IntField("��������", pos);
                z.teleX = pos.x;
                z.teleY = pos.y;
                z.teleHigh = EditorGUILayout.FloatField("���͸߶�", z.teleHigh);
                break;
            case z.mode.effect:
                z.getEffect = (effect)EditorGUILayout.EnumPopup("��ȡЧ��", z.getEffect);
                z.canvas = (Canvas)EditorGUILayout.ObjectField("Canvas", z.canvas, typeof(Canvas), false);
                z.getHintPrefab = (GameObject)EditorGUILayout.ObjectField("��ȡЧ��ʱ����ʾ��", z.getHintPrefab, typeof(GameObject), false);
                z.keySound = (AudioClip)EditorGUILayout.ObjectField("��ȡЧ��ǰ����Ч", z.keySound, typeof(AudioClip), false);
                z.getSound = (AudioClip)EditorGUILayout.ObjectField("��ȡЧ��ʱ����Ч", z.getSound, typeof(AudioClip), false);
                break;
            case z.mode.talk:
                textsize = EditorGUILayout.IntField("�Ի���", textsize >= 0 ? textsize : 0);
                z.talkTexts = new string[textsize];
                z.TalkPoses = new z.talkPos[textsize];
                EditorGUI.indentLevel++;
                for (int i = 0; i < textsize; i++)
                {
                    EditorGUILayout.LabelField("Ԫ�� " + i);
                    EditorGUI.indentLevel++;
                    z.talkTexts[i] = EditorGUILayout.TextArea("�Ի�����...");
                    z.TalkPoses[i] = (z.talkPos)EditorGUILayout.EnumPopup("�Ի���λ", z.TalkPoses[i]);
                    EditorGUI.indentLevel--;
                }
                EditorGUI.indentLevel--;
                break;
            default:
                break;
        }
    }
}
