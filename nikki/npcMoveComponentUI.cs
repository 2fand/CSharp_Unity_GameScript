using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(npcMove))]
public class npcMoveComponentUI : Editor
{
    private npcMove npcMove;
    private Vector2Int pos;
    private void OnEnable()
    {
        npcMove = (npcMove)target;
    }
    public override void OnInspectorGUI()
    {
        npcMove.waitTime = EditorGUILayout.FloatField("npc�ƶ�����ȴ", npcMove.waitTime);
        npcMove.speed = EditorGUILayout.FloatField("npc�ƶ��ٶ�", npcMove.speed);
        npcMove.canTurn = EditorGUILayout.Toggle("�ƶ����Ƿ�ı䷽��", npcMove.canTurn);
        npcMove.moveMode = (npcMove.move)EditorGUILayout.EnumPopup("npc�ƶ���ʽ", npcMove.moveMode);
        if (npcMove.move.Catch == npcMove.moveMode) {
            npcMove.canOver = EditorGUILayout.Toggle("�Ƿ��ܱ�����", npcMove.canOver);
            npcMove.hurtSound = (AudioClip)EditorGUILayout.ObjectField("��ץǰ��Ч", npcMove.hurtSound, typeof(AudioClip), false);
            npcMove.catchSound = (AudioClip)EditorGUILayout.ObjectField("��ץ����Ч", npcMove.catchSound, typeof(AudioClip), false);
            npcMove.enterMode = (change.enterMode)EditorGUILayout.EnumPopup("��ץǰת��", npcMove.enterMode);
            npcMove.enterTime = EditorGUILayout.FloatField("��ץǰת��ʱ��", npcMove.enterTime);
            npcMove.exitMode = (change.exitMode)EditorGUILayout.EnumPopup("��ץ��ת��", npcMove.exitMode);
            npcMove.exitTime = EditorGUILayout.FloatField("��ץ��ת��ʱ��", npcMove.exitTime);
            npcMove.teleWorldName = EditorGUILayout.TextField("��ץ���͵���������", npcMove.teleWorldName);
            pos = new Vector2Int(npcMove.teleWorldX, npcMove.teleWorldY);
            pos = EditorGUILayout.Vector2IntField("��ץ���͵�������", pos);
            npcMove.teleWorldX = pos.x;
            npcMove.teleWorldY = pos.y;
            npcMove.teleWorldHigh = EditorGUILayout.FloatField("��ץ���͸߶�", npcMove.teleWorldHigh);
            npcMove.teleYouFront = (wasd)EditorGUILayout.EnumPopup("��ץ���ҵķ���", npcMove.teleYouFront); 
        }
    }
}
