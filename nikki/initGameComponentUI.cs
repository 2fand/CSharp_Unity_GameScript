using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using System;
using System.ComponentModel;
using UnityEditor.UIElements;

[CustomEditor(typeof(InitGame))]
public class initGameComponentUI : Editor
{
    InitGame initGame;
    private bool showEffects = true;
    private bool showWalkSounds = true;
    private bool showCloseUps = true;
    private bool showDefaultCloseUp = true;
    private void OnEnable()
    {
        initGame = (InitGame)target;
    }

    public void updateArray<T>(ref T[] array, int newLength)
    {
        if (null == array || array.Length != newLength)
        {
            T[] ts = new T[newLength];
            if (null != array)
            {
                Array.Copy(array, ts, Mathf.Min(array.Length, newLength));
            }
            array = ts;
        }
    }
    
    public override void OnInspectorGUI()
    {
        effectItem.effectCount = EditorGUILayout.IntField("效果数量", effectItem.effectCount);
        updateArray(ref initGame.effects, effectItem.effectCount + 1);
        updateArray(ref initGame.effectWalkSounds, effectItem.effectCount + 1);
        updateArray(ref initGame.closeUps, 3 * (effectItem.effectCount + 1));
        Sprite aaaa = initGame.closeUps[0];
        initGame.effects[0] = (GameObject)EditorGUILayout.ObjectField("默认形象", initGame.effects[0], typeof(GameObject), false);
        initGame.effectWalkSounds[0] = (AudioClip)EditorGUILayout.ObjectField("默认走路音效", initGame.effectWalkSounds[0], typeof(AudioClip), false);
        showDefaultCloseUp = EditorGUILayout.BeginFoldoutHeaderGroup(showDefaultCloseUp, "默认特写");
        EditorGUI.indentLevel++;
        initGame.closeUps[0] = (Sprite)EditorGUILayout.ObjectField("人物图像", initGame.closeUps[0], typeof(Sprite), false);
        initGame.closeUps[1] = (Sprite)EditorGUILayout.ObjectField("边框图像", initGame.closeUps[1], typeof(Sprite), false);
        initGame.closeUps[2] = (Sprite)EditorGUILayout.ObjectField("背景图像", initGame.closeUps[2], typeof(Sprite), false);
        EditorGUI.indentLevel--;
        EditorGUILayout.EndFoldoutHeaderGroup();

        initGame.effectEqiupSound = (AudioClip)EditorGUILayout.ObjectField("默认效果装备音效", initGame.effectEqiupSound, typeof(AudioClip), false);
        initGame.effectCancelEqiupSound = (AudioClip)EditorGUILayout.ObjectField("默认效果取消装备音效", initGame.effectCancelEqiupSound, typeof(AudioClip), false);

        if (effectItem.effectCount > 0)
        {
            showEffects = EditorGUILayout.BeginFoldoutHeaderGroup(showEffects, "效果形象");
            EditorGUI.indentLevel++;
            for (int i = 1; showEffects && i < initGame.effects.Length; i++)
            {
                initGame.effects[i] = (GameObject)EditorGUILayout.ObjectField("元素 " + (i - 1), initGame.effects[i], typeof(GameObject), false);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUI.indentLevel--;
            showWalkSounds = EditorGUILayout.BeginFoldoutHeaderGroup(showWalkSounds, "效果行走音效");
            EditorGUI.indentLevel++;
            for (int i = 1; showWalkSounds && i < initGame.effectWalkSounds.Length; i++)
            {
                initGame.effectWalkSounds[i] = (AudioClip)EditorGUILayout.ObjectField("元素 " + (i - 1), initGame.effectWalkSounds[i], typeof(AudioClip), false);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUI.indentLevel--;
            showCloseUps = EditorGUILayout.BeginFoldoutHeaderGroup(showCloseUps, "效果人物特写");
            EditorGUI.indentLevel++;
            for (int i = 1; showCloseUps && i < initGame.closeUps.Length / 3; i++)
            {
                EditorGUILayout.LabelField("元素 " + (i - 1));
                EditorGUI.indentLevel++;
                initGame.closeUps[i * 3] = (Sprite)EditorGUILayout.ObjectField("人物图像", initGame.closeUps[i * 3], typeof(Sprite), false);
                initGame.closeUps[i * 3 + 1] = (Sprite)EditorGUILayout.ObjectField("边框图像", initGame.closeUps[i * 3 + 1], typeof(Sprite), false);
                initGame.closeUps[i * 3 + 2] = (Sprite)EditorGUILayout.ObjectField("背景图像", initGame.closeUps[i * 3 + 2], typeof(Sprite), false);
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
        initGame.effectCloseUp = new closeUp[initGame.closeUps.Length / 3];
        for (int i = 0; i < initGame.closeUps.Length / 3; i++)
        {
            initGame.effectCloseUp[i] = new closeUp(initGame.closeUps[i * 3], initGame.closeUps[i * 3 + 1], initGame.closeUps[i * 3 + 2]);
        }
        EditorGUILayout.Space();
        initGame.gameFont = (Font)EditorGUILayout.ObjectField("默认游戏字体", initGame.gameFont, typeof(Font), false);
        initGame.openMenuSound = (AudioClip)EditorGUILayout.ObjectField("默认打开菜单音效", initGame.openMenuSound, typeof(AudioClip), false);
        initGame.closeMenuSound = (AudioClip)EditorGUILayout.ObjectField("默认关闭菜单音效", initGame.closeMenuSound, typeof(AudioClip), false);
        initGame.changeSelectSound = (AudioClip)EditorGUILayout.ObjectField("默认移动光标音效", initGame.changeSelectSound, typeof(AudioClip), false);
        initGame.CursorHorizontalMoveMode = (Cursor.HorizontalMoveMode)EditorGUILayout.EnumPopup("默认光标水平移动方式", initGame.CursorHorizontalMoveMode);
        initGame.CursorVerticalMoveMode = (Cursor.VerticalMoveMode)EditorGUILayout.EnumPopup("默认光标垂直移动方式", initGame.CursorVerticalMoveMode);
        initGame.clearMenuSound = (AudioClip)EditorGUILayout.ObjectField("默认清空菜单音效", initGame.clearMenuSound, typeof(AudioClip), false);
        initGame.wakeUpSound = (AudioClip)EditorGUILayout.ObjectField("默认醒来音效", initGame.wakeUpSound, typeof(AudioClip), false);
        EditorGUILayout.BeginHorizontal();
        initGame.useHealth = EditorGUILayout.Toggle("是否使用hp", initGame.useHealth);
        if (initGame.useHealth)
        {
            initGame.hpUnit = EditorGUILayout.TextField("默认hp单位", initGame.hpUnit);
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        initGame.useMp = EditorGUILayout.Toggle("是否使用mp", initGame.useMp);
        if (initGame.useMp) {
            initGame.mpUnit = EditorGUILayout.TextField("默认mp单位", initGame.mpUnit);
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        initGame.useExp = EditorGUILayout.Toggle("是否使用tp", initGame.useExp);
        if (initGame.useExp)
        {
            initGame.expUnit = EditorGUILayout.TextField("默认tp单位", initGame.expUnit);
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        initGame.useLevel = EditorGUILayout.Toggle("是否使用等级", initGame.useLevel);
        if (initGame.useLevel)
        {
            initGame.levelUnit = EditorGUILayout.TextField("默认等级单位", initGame.levelUnit);
        }
        EditorGUILayout.EndHorizontal();
    }
}
