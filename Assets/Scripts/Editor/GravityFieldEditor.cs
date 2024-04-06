using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

[CustomEditor(typeof(GravityField))]
public class GravityFieldEditor : Editor
{
    BoxBoundsHandle _boxBoundsHandle = new BoxBoundsHandle();
    
    private void OnSceneGUI()
    {
        GravityField field = target as GravityField;
        _boxBoundsHandle.size = field.Extent;
        _boxBoundsHandle.center = field.transform.position;
        Handles.color = Color.blue;
        EditorGUI.BeginChangeCheck();

        _boxBoundsHandle.DrawHandle();
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(field, "Changed Area");
            field.Extent = _boxBoundsHandle.size;
        }
    }
    
    public override void OnInspectorGUI()
    {
        GravityField field = target as GravityField;
        DrawDefaultInspector();
        if (GUILayout.Button("Recalculate acceleration")) 
        {
            field.CalculateAcceleration();
            EditorUtility.SetDirty(field);
        }
    }
}

