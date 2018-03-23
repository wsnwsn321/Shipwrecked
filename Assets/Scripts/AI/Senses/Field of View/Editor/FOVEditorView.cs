using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FieldOfView))]
public class FOVEditorView : Editor {

    void OnSceneGUI()
    {
        FieldOfView fov = (FieldOfView)target;
        Vector3 offset = fov.offset;
        float angleOffset = fov.angleOffset;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position + offset, Vector3.up, Vector3.forward, 360, fov.viewRadius);
        Vector3 viewAngleA = fov.DirectionFromAngle(-fov.viewAngle / 2 + angleOffset, false);
        Vector3 viewAngleB = fov.DirectionFromAngle(fov.viewAngle / 2 + angleOffset, false);

        Handles.DrawLine(fov.transform.position + offset, fov.transform.position + offset + viewAngleA * fov.viewRadius);
        Handles.DrawLine(fov.transform.position + offset, fov.transform.position + offset + viewAngleB * fov.viewRadius);

        Handles.color = Color.green;
        foreach (Transform visibleTarget in fov.visibleTargets)
        {
            Handles.DrawLine(fov.transform.position + offset, visibleTarget.position);
        }
    }
}
