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
        npcMove.waitTime = EditorGUILayout.FloatField("npc移动后冷却", npcMove.waitTime);
        npcMove.speed = EditorGUILayout.FloatField("npc移动速度", npcMove.speed);
        npcMove.canTurn = EditorGUILayout.Toggle("移动后是否改变方向", npcMove.canTurn);
        npcMove.moveMode = (npcMove.move)EditorGUILayout.EnumPopup("npc移动方式", npcMove.moveMode);
        if (npcMove.move.Catch == npcMove.moveMode) {
            npcMove.canOver = EditorGUILayout.Toggle("是否能被传送", npcMove.canOver);
            npcMove.hurtSound = (AudioClip)EditorGUILayout.ObjectField("被抓前音效", npcMove.hurtSound, typeof(AudioClip), false);
            npcMove.catchSound = (AudioClip)EditorGUILayout.ObjectField("被抓后音效", npcMove.catchSound, typeof(AudioClip), false);
            npcMove.enterMode = (change.enterMode)EditorGUILayout.EnumPopup("被抓前转场", npcMove.enterMode);
            npcMove.enterTime = EditorGUILayout.FloatField("被抓前转场时间", npcMove.enterTime);
            npcMove.exitMode = (change.exitMode)EditorGUILayout.EnumPopup("被抓后转场", npcMove.exitMode);
            npcMove.exitTime = EditorGUILayout.FloatField("被抓后转场时间", npcMove.exitTime);
            npcMove.teleWorldName = EditorGUILayout.TextField("被抓后传送到的世界名", npcMove.teleWorldName);
            pos = new Vector2Int(npcMove.teleWorldX, npcMove.teleWorldY);
            pos = EditorGUILayout.Vector2IntField("被抓后传送到的坐标", pos);
            npcMove.teleWorldX = pos.x;
            npcMove.teleWorldY = pos.y;
            npcMove.teleWorldHigh = EditorGUILayout.FloatField("被抓后传送高度", npcMove.teleWorldHigh);
            npcMove.teleYouFront = (wasd)EditorGUILayout.EnumPopup("被抓后我的方向", npcMove.teleYouFront); 
        }
    }
}
