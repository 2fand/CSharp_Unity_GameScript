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
        z.haveFront = EditorGUILayout.Toggle("是否不无视方向", z.haveFront);
        z.mod = (z.mode)EditorGUILayout.EnumPopup("交互模式", z.mod);
        switch (z.mod) {
            case z.mode.tele:
                z.teleWaitTime = EditorGUILayout.FloatField("传送时等待时间", z.teleWaitTime);
                z.worldName = EditorGUILayout.TextField("传送世界名字", z.worldName);
                z.openSound = (AudioClip)EditorGUILayout.ObjectField("传送开始前音效", z.openSound, typeof(AudioClip), false);
                z.closeSound = (AudioClip)EditorGUILayout.ObjectField("传送结束后音效", z.closeSound, typeof(AudioClip), false);
                pos = new Vector2Int(z.teleX, z.teleY);
                pos = EditorGUILayout.Vector2IntField("传送坐标", pos);
                z.teleX = pos.x;
                z.teleY = pos.y;
                z.teleHigh = EditorGUILayout.FloatField("传送高度", z.teleHigh);
                break;
            case z.mode.effect:
                z.getEffect = (effect)EditorGUILayout.EnumPopup("获取效果", z.getEffect);
                z.canvas = (Canvas)EditorGUILayout.ObjectField("Canvas", z.canvas, typeof(Canvas), false);
                z.getHintPrefab = (GameObject)EditorGUILayout.ObjectField("获取效果时的提示物", z.getHintPrefab, typeof(GameObject), false);
                z.keySound = (AudioClip)EditorGUILayout.ObjectField("获取效果前的音效", z.keySound, typeof(AudioClip), false);
                z.getSound = (AudioClip)EditorGUILayout.ObjectField("获取效果时的音效", z.getSound, typeof(AudioClip), false);
                break;
            case z.mode.talk:
                textsize = EditorGUILayout.IntField("对话数", textsize >= 0 ? textsize : 0);
                z.talkTexts = new string[textsize];
                z.TalkPoses = new z.talkPos[textsize];
                EditorGUI.indentLevel++;
                for (int i = 0; i < textsize; i++)
                {
                    EditorGUILayout.LabelField("元素 " + i);
                    EditorGUI.indentLevel++;
                    z.talkTexts[i] = EditorGUILayout.TextArea("对话内容...");
                    z.TalkPoses[i] = (z.talkPos)EditorGUILayout.EnumPopup("对话框方位", z.TalkPoses[i]);
                    EditorGUI.indentLevel--;
                }
                EditorGUI.indentLevel--;
                break;
            default:
                break;
        }
    }
}
