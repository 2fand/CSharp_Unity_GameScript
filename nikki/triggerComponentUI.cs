using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static funcsForComponentUI;

[CustomEditor(typeof(trigger))]
public class triggerComponentUI : Editor
{
    trigger trigger;
    bool openCommands = true;
    bool openSounds = true;
    bool openSprites = true;
    bool openCommandHelps = true;
    bool openValueHelps = true;
    private void OnEnable()
    {
        trigger = (trigger)target;
    }
    public override void OnInspectorGUI()
    {   
        trigger.x = EditorGUILayout.IntField("��������x����", Mathf.Max(0, trigger.x));
        trigger.y = EditorGUILayout.IntField("��������y����", Mathf.Max(0, trigger.y));
        trigger.ChangeTransform = EditorGUILayout.Toggle("�Ƿ�ı�λ��", trigger.ChangeTransform);
        trigger.triggerPrefab = (GameObject)EditorGUILayout.ObjectField("������������", trigger.triggerPrefab, typeof(GameObject), false);
        trigger.triggerExtendUp = EditorGUILayout.IntField("�����������������", trigger.triggerExtendUp);
        trigger.triggerExtendLeft = EditorGUILayout.IntField("�����������������", trigger.triggerExtendLeft);
        trigger.triggerExtendRight = EditorGUILayout.IntField("�����������������", trigger.triggerExtendRight);
        trigger.triggerExtendDown = EditorGUILayout.IntField("�����������������", trigger.triggerExtendDown);
        openCommandHelps = EditorGUILayout.BeginFoldoutHeaderGroup(openCommandHelps, "��������");
        if (openCommandHelps)
        {
            EditorGUI.indentLevel++;
            string commandsHelpStr = "��������:\n";
            foreach (DictionaryEntry command in trigger.commandHelpStrings)
            {
                commandsHelpStr += "\t" + command.Key + ":\n\t\t" + command.Value + "\n";
            }
            EditorGUILayout.HelpBox(commandsHelpStr, MessageType.Info);
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        openValueHelps = EditorGUILayout.BeginFoldoutHeaderGroup(openValueHelps, "����ֵ");
        if (openValueHelps)
        {
            EditorGUI.indentLevel++;
            string valuesHelpStr = "����ֵ:\n";
            foreach (DictionaryEntry value in trigger.valueHelpStrings)
            {
                valuesHelpStr += "\t" + value.Key + " | " + value.Value + "\n";
            }
            EditorGUILayout.HelpBox(valuesHelpStr, MessageType.Info);
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUILayout.BeginHorizontal();
        openCommands = EditorGUILayout.BeginFoldoutHeaderGroup(openCommands, "����");
        trigger.commandsCount = EditorGUILayout.IntField(Mathf.Max(0, trigger.commandsCount));
        updateArray(ref trigger.commands, trigger.commandsCount);
        EditorGUILayout.EndHorizontal();
        EditorGUI.indentLevel++;
        for (int i = 0; openCommands && i < trigger.commandsCount; i++)
        {
            trigger.commands[i] = EditorGUILayout.TextField(i.ToString(), trigger.commands[i]);
        }
        EditorGUI.indentLevel--;
        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUILayout.BeginHorizontal();
        openSounds = EditorGUILayout.BeginFoldoutHeaderGroup(openSounds, "��Ƶ");
        trigger.soundsCount = EditorGUILayout.IntField(Mathf.Max(0, trigger.soundsCount));
        updateArray(ref trigger.sounds, trigger.soundsCount);
        EditorGUILayout.EndHorizontal();
        EditorGUI.indentLevel++;
        for (int i = 0; openSounds && i < trigger.soundsCount; i++)
        {
            trigger.sounds[i] = (AudioClip)EditorGUILayout.ObjectField("Ԫ�� " + i, trigger.sounds[i], typeof(AudioClip), false);
        }
        EditorGUI.indentLevel--;
        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUILayout.BeginHorizontal();
        openSprites = EditorGUILayout.BeginFoldoutHeaderGroup(openSprites, "ͼƬ");
        trigger.spritesCount = EditorGUILayout.IntField(Mathf.Max(0, trigger.spritesCount));
        updateArray(ref trigger.sprites, trigger.spritesCount);
        EditorGUILayout.EndHorizontal();
        EditorGUI.indentLevel++;
        for (int i = 0; openSprites && i < trigger.spritesCount; i++)
        {
            trigger.sprites[i] = (Sprite)EditorGUILayout.ObjectField("Ԫ�� " + i, trigger.sprites[i], typeof(Sprite), false);
        }
        EditorGUI.indentLevel--;
        EditorGUILayout.EndFoldoutHeaderGroup();
    }
}
