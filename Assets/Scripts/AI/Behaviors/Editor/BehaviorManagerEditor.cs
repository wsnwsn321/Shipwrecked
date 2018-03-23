using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BehaviorManager))]
public class BehaviorManagerEditor : Editor {

    bool fold = true;

    public override void OnInspectorGUI()
    {
        var bm = target as BehaviorManager;
        var entityType = bm.GetComponent<EntityType>();

        if (entityType.type == EntityTypes.Monster || entityType.type == EntityTypes.Teammate)
        {
            EditorGUILayout.LabelField("Wander Ahead Chance");
            bm.wanderAheadChance = EditorGUILayout.Slider(bm.wanderAheadChance, 0, 1);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Wander Ahead Distance");
            bm.wanderAheadDistance = EditorGUILayout.Slider(bm.wanderAheadDistance, 0, 30);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Wander Ahead Angle");
            bm.wanderAheadAngle = EditorGUILayout.Slider(bm.wanderAheadAngle, 0, 180);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Wander Ahead Radius");
            bm.wanderAheadRadius = EditorGUILayout.Slider(bm.wanderAheadRadius, 0, 20);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Wander Randomly Radius");
            bm.wanderRandomlyRadius = EditorGUILayout.Slider(bm.wanderRandomlyRadius, 0, 30);
        }
    }
}
