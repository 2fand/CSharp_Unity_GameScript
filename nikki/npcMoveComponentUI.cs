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
        npcMove.isTurn = EditorGUILayout.Toggle("npc�Ƿ�ת��", npcMove.isTurn);
        npcMove.moveMode = (npcMove.move)EditorGUILayout.EnumPopup("npc�ƶ���ʽ", npcMove.moveMode);
        if (npcMove.move.Catch == npcMove.moveMode) {
            npcMove.canOver = EditorGUILayout.Toggle("�Ƿ��ܱ�����", npcMove.canOver);
            npcMove.hurtSound = (AudioClip)EditorGUILayout.ObjectField("��ץǰ��Ч", npcMove.hurtSound, typeof(AudioClip), false);
            npcMove.catchSound = (AudioClip)EditorGUILayout.ObjectField("��ץ����Ч", npcMove.catchSound, typeof(AudioClip), false);
            npcMove.enterMode = (change.enterMode)EditorGUILayout.EnumPopup("��ץǰת��", npcMove.enterMode);
            npcMove.exitMode = (change.exitMode)EditorGUILayout.EnumPopup("��ץ��ת��", npcMove.exitMode);
            npcMove.teleWorldName = EditorGUILayout.TextField("��ץ���͵���������");
            pos = new Vector2Int(npcMove.teleWorldX, npcMove.teleWorldY);
            pos = EditorGUILayout.Vector2IntField("��ץ���͵�������", pos);
            npcMove.teleWorldX = pos.x;
            npcMove.teleWorldY = pos.y;
            npcMove.teleWorldHigh = EditorGUILayout.FloatField("��ץ���͸߶�", npcMove.teleWorldHigh);
            npcMove.teleYouFront = (npcMove.wasd)EditorGUILayout.EnumPopup("��ץ���ҵķ���", npcMove.teleYouFront); 
        }
    }
}
