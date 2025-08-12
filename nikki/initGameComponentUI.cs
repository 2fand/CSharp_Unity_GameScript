using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using System;
using System.ComponentModel;
using UnityEditor.UIElements;
using static funcsForComponentUI;

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
    
    public override void OnInspectorGUI()
    {
        effectItem.effectCount = EditorGUILayout.IntField("Ч������", effectItem.effectCount);
        updateArray(ref initGame.effects, effectItem.effectCount + 1);
        updateArray(ref initGame.effectWalkSounds, effectItem.effectCount + 1);
        updateArray(ref initGame.closeUps, 3 * (effectItem.effectCount + 1));
        initGame.effects[0] = (GameObject)EditorGUILayout.ObjectField("Ĭ������", initGame.effects[0], typeof(GameObject), false);
        initGame.effectWalkSounds[0] = (AudioClip)EditorGUILayout.ObjectField("Ĭ����·��Ч", initGame.effectWalkSounds[0], typeof(AudioClip), false);
        showDefaultCloseUp = EditorGUILayout.BeginFoldoutHeaderGroup(showDefaultCloseUp, "Ĭ����д");
        EditorGUI.indentLevel++;
        EditorGUILayout.HelpBox("�ڶ���Ĭ�ϵĲ����£�ͼ���С���Ϊ90x90", MessageType.Info);
        initGame.closeUps[1] = (Sprite)EditorGUILayout.ObjectField("�߿�ͼ��", initGame.closeUps[1], typeof(Sprite), false);
        initGame.closeUps[0] = (Sprite)EditorGUILayout.ObjectField("����ͼ��", initGame.closeUps[0], typeof(Sprite), false);
        initGame.closeUps[2] = (Sprite)EditorGUILayout.ObjectField("����ͼ��", initGame.closeUps[2], typeof(Sprite), false);
        EditorGUI.indentLevel--;
        EditorGUILayout.EndFoldoutHeaderGroup();

        initGame.effectEqiupSound = (AudioClip)EditorGUILayout.ObjectField("Ĭ��Ч��װ����Ч", initGame.effectEqiupSound, typeof(AudioClip), false);
        initGame.effectCancelEqiupSound = (AudioClip)EditorGUILayout.ObjectField("Ĭ��Ч��ȡ��װ����Ч", initGame.effectCancelEqiupSound, typeof(AudioClip), false);
        Game.enterTime = EditorGUILayout.FloatField("Ĭ�Ͻ���ת��ʱ��", Game.enterTime);
        Game.exitTime = EditorGUILayout.FloatField("Ĭ���˳�ת��ʱ��", Game.exitTime);

        if (effectItem.effectCount > 0)
        {
            showEffects = EditorGUILayout.BeginFoldoutHeaderGroup(showEffects, "Ч������");
            EditorGUI.indentLevel++;
            for (int i = 1; showEffects && i < initGame.effects.Length; i++)
            {
                initGame.effects[i] = (GameObject)EditorGUILayout.ObjectField("Ԫ�� " + (i - 1), initGame.effects[i], typeof(GameObject), false);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUI.indentLevel--;
            showWalkSounds = EditorGUILayout.BeginFoldoutHeaderGroup(showWalkSounds, "Ч��������Ч");
            EditorGUI.indentLevel++;
            for (int i = 1; showWalkSounds && i < initGame.effectWalkSounds.Length; i++)
            {
                initGame.effectWalkSounds[i] = (AudioClip)EditorGUILayout.ObjectField("Ԫ�� " + (i - 1), initGame.effectWalkSounds[i], typeof(AudioClip), false);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUI.indentLevel--;
            showCloseUps = EditorGUILayout.BeginFoldoutHeaderGroup(showCloseUps, "Ч��������д");
            EditorGUI.indentLevel++;
            for (int i = 1; showCloseUps && i < initGame.closeUps.Length / 3; i++)
            {
                EditorGUILayout.LabelField("Ԫ�� " + (i - 1));
                EditorGUI.indentLevel++;
                initGame.closeUps[i * 3 + 1] = (Sprite)EditorGUILayout.ObjectField("�߿�ͼ��", initGame.closeUps[i * 3 + 1], typeof(Sprite), false);
                initGame.closeUps[i * 3] = (Sprite)EditorGUILayout.ObjectField("����ͼ��", initGame.closeUps[i * 3], typeof(Sprite), false);
                initGame.closeUps[i * 3 + 2] = (Sprite)EditorGUILayout.ObjectField("����ͼ��", initGame.closeUps[i * 3 + 2], typeof(Sprite), false);
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
        EditorGUILayout.Space();
        initGame.gameFont = (Font)EditorGUILayout.ObjectField("Ĭ����Ϸ����", initGame.gameFont, typeof(Font), false);
        initGame.openMenuSound = (AudioClip)EditorGUILayout.ObjectField("Ĭ�ϴ򿪲˵���Ч", initGame.openMenuSound, typeof(AudioClip), false);
        initGame.closeMenuSound = (AudioClip)EditorGUILayout.ObjectField("Ĭ�Ϲرղ˵���Ч", initGame.closeMenuSound, typeof(AudioClip), false);
        initGame.changeSelectSound = (AudioClip)EditorGUILayout.ObjectField("Ĭ���ƶ������Ч", initGame.changeSelectSound, typeof(AudioClip), false);
        initGame.CursorHorizontalMoveMode = (Cursor.HorizontalMoveMode)EditorGUILayout.EnumPopup("Ĭ�Ϲ��ˮƽ�ƶ���ʽ", initGame.CursorHorizontalMoveMode);
        initGame.CursorVerticalMoveMode = (Cursor.VerticalMoveMode)EditorGUILayout.EnumPopup("Ĭ�Ϲ�괹ֱ�ƶ���ʽ", initGame.CursorVerticalMoveMode);
        initGame.clearMenuSound = (AudioClip)EditorGUILayout.ObjectField("Ĭ����ղ˵���Ч", initGame.clearMenuSound, typeof(AudioClip), false);
        initGame.wakeUpSound = (AudioClip)EditorGUILayout.ObjectField("Ĭ��������Ч", initGame.wakeUpSound, typeof(AudioClip), false);
        initGame.useHealth = EditorGUILayout.Toggle("�Ƿ�ʹ��hp", initGame.useHealth);
        if (initGame.useHealth)
        {
            EditorGUI.indentLevel++;
            initGame.hpUnit = EditorGUILayout.TextField("Ĭ��hp��λ", initGame.hpUnit);
            Game.defaultHp = EditorGUILayout.IntField("Ĭ������hp", Game.defaultHp);
            Game.defaultMaxHp = EditorGUILayout.IntField("Ĭ�����hp", Game.defaultMaxHp);
            if (Game.defaultHp > Game.defaultMaxHp)
            {
                Game.defaultHp = Game.defaultMaxHp;
            }
            EditorGUI.indentLevel--;
        }
        initGame.useMp = EditorGUILayout.Toggle("�Ƿ�ʹ��mp", initGame.useMp);
        if (initGame.useMp) {
            EditorGUI.indentLevel++;
            initGame.mpUnit = EditorGUILayout.TextField("Ĭ��mp��λ", initGame.mpUnit);
            Game.defaultMp = EditorGUILayout.IntField("Ĭ������mp", Game.defaultMp);
            Game.defaultMaxMp = EditorGUILayout.IntField("Ĭ�����mp", Game.defaultMaxMp);
            if (Game.defaultMp > Game.defaultMaxMp)
            {
                Game.defaultMp = Game.defaultMaxMp;
            }
            EditorGUI.indentLevel--;
        }
        initGame.useExp = EditorGUILayout.Toggle("�Ƿ�ʹ�þ���", initGame.useExp);
        if (initGame.useExp)
        {
            EditorGUI.indentLevel++;
            initGame.expUnit = EditorGUILayout.TextField("Ĭ�Ͼ��鵥λ", initGame.expUnit);
            Game.defaultExp = EditorGUILayout.IntField("Ĭ�����ﾭ��", Game.defaultExp);
            EditorGUI.indentLevel--;
        }
        initGame.useLevel = EditorGUILayout.Toggle("�Ƿ�ʹ�õȼ�", initGame.useLevel);
        if (initGame.useLevel)
        {
            EditorGUI.indentLevel++;
            initGame.levelUnit = EditorGUILayout.TextField("Ĭ�ϵȼ���λ", initGame.levelUnit);
            Game.defaultLevel = EditorGUILayout.IntField("Ĭ������ȼ�", Game.defaultLevel);
            Game.defaultMaxLevel = EditorGUILayout.IntField("Ĭ�����ȼ�", Game.defaultMaxLevel);
            if (Game.defaultLevel > Game.defaultMaxLevel)
            {
                Game.defaultLevel = Game.defaultMaxLevel;
            }
            EditorGUI.indentLevel--;
        }
    }
}
