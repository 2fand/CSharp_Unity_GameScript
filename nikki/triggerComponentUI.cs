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
        trigger.x = EditorGUILayout.IntField("触发区域x坐标", Mathf.Max(0, trigger.x));
        trigger.y = EditorGUILayout.IntField("触发区域y坐标", Mathf.Max(0, trigger.y));
        trigger.ChangeTransform = EditorGUILayout.Toggle("是否改变位置", trigger.ChangeTransform);
        trigger.triggerPrefab = (GameObject)EditorGUILayout.ObjectField("触发器复制体", trigger.triggerPrefab, typeof(GameObject), false);
        trigger.triggerExtendUp = EditorGUILayout.IntField("触发区域上延伸格数", trigger.triggerExtendUp);
        trigger.triggerExtendLeft = EditorGUILayout.IntField("触发区域左延伸格数", trigger.triggerExtendLeft);
        trigger.triggerExtendRight = EditorGUILayout.IntField("触发区域右延伸格数", trigger.triggerExtendRight);
        trigger.triggerExtendDown = EditorGUILayout.IntField("触发区域下延伸格数", trigger.triggerExtendDown);
        openCommandHelps = EditorGUILayout.BeginFoldoutHeaderGroup(openCommandHelps, "可用命令");
        if (openCommandHelps)
        {
            EditorGUI.indentLevel++;
            string commandsHelpStr = "可用命令:\n";
            foreach (DictionaryEntry command in trigger.commandHelpStrings)
            {
                commandsHelpStr += "\t" + command.Key + ":\n\t\t" + command.Value + "\n";
            }
            EditorGUILayout.HelpBox(commandsHelpStr, MessageType.Info);
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        openValueHelps = EditorGUILayout.BeginFoldoutHeaderGroup(openValueHelps, "可用值");
        if (openValueHelps)
        {
            EditorGUI.indentLevel++;
            string valuesHelpStr = "可用值:\n";
            foreach (DictionaryEntry value in command.valueHelps)
            {
                valuesHelpStr += "\t" + value.Key + " | " + value.Value + "\n";
            }
            EditorGUILayout.HelpBox(valuesHelpStr, MessageType.Info);
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUILayout.BeginHorizontal();
        openCommands = EditorGUILayout.BeginFoldoutHeaderGroup(openCommands, "命令");
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
        openSounds = EditorGUILayout.BeginFoldoutHeaderGroup(openSounds, "音频");
        trigger.soundsCount = EditorGUILayout.IntField(Mathf.Max(0, trigger.soundsCount));
        updateArray(ref trigger.sounds, trigger.soundsCount);
        EditorGUILayout.EndHorizontal();
        EditorGUI.indentLevel++;
        for (int i = 0; openSounds && i < trigger.soundsCount; i++)
        {
            trigger.sounds[i] = (AudioClip)EditorGUILayout.ObjectField("元素 " + i, trigger.sounds[i], typeof(AudioClip), false);
        }
        EditorGUI.indentLevel--;
        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUILayout.BeginHorizontal();
        openSprites = EditorGUILayout.BeginFoldoutHeaderGroup(openSprites, "图片");
        trigger.spritesCount = EditorGUILayout.IntField(Mathf.Max(0, trigger.spritesCount));
        updateArray(ref trigger.sprites, trigger.spritesCount);
        EditorGUILayout.EndHorizontal();
        EditorGUI.indentLevel++;
        for (int i = 0; openSprites && i < trigger.spritesCount; i++)
        {
            trigger.sprites[i] = (Sprite)EditorGUILayout.ObjectField("元素 " + i, trigger.sprites[i], typeof(Sprite), false);
        }
        EditorGUI.indentLevel--;
        EditorGUILayout.EndFoldoutHeaderGroup();
    }
}
