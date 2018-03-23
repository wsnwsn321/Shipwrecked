using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EntityType))]
public class TypeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var entityType = target as EntityType;

        entityType.type = (EntityTypes)EditorGUILayout.EnumPopup("Entity Type", entityType.type);

        if (entityType.type == EntityTypes.Monster)
        {
            entityType.monsterType = (MonsterTypes)EditorGUILayout.EnumPopup("Monster Type", entityType.monsterType);
        }
        else if (entityType.type == EntityTypes.Teammate)
        {
            entityType.teammateType = (TeammateTypes)EditorGUILayout.EnumPopup("Teammate Type", entityType.teammateType);
        }
    }

}
