using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[CustomEditor(typeof(MapManager))]
public class MapEditor : Editor
{
    MapManager map;
    int F_ButtonIndex = 0;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (Application.isPlaying)
        {
            map = target as MapManager;
            EditorGUILayout.BeginHorizontal();
            var ButtonIndex = EditorGUILayout.Popup(F_ButtonIndex, map.LN);
            if (F_ButtonIndex != ButtonIndex)
            {
                F_ButtonIndex = ButtonIndex;
            }
            if (GUILayout.Button("读取列表"))
            {
                map.LoadMap(map.LN[F_ButtonIndex]);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("清除塔点"))
            {
                map.ClearTowrn();
            }
            if (GUILayout.Button("清空路径"))
            {
                map.ClearPath();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("保存地图"))
            {
                map.SaveMap(map.LN[F_ButtonIndex], (LevelName)F_ButtonIndex);
            }
        }
    }

   

}
