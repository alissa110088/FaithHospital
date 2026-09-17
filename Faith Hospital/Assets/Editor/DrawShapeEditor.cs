using log4net.Util;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DrawShape))]
public class DrawShapeEditor : Editor
{
    private DrawShape drawShape;

    private void OnEnable()
    {
        drawShape = (DrawShape)target;
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.HelpBox(
            "Ctrl+Clic dans la Scene view : ajoute un point.\n",
            MessageType.Info);

        if (GUILayout.Button("Vider les points"))
        {
            Undo.RecordObject(drawShape, "Clear Trigger Points");
            drawShape.points.Clear();
            EditorUtility.SetDirty(drawShape);
        }
    }

    private void OnSceneGUI()
    {
        Event m_Event = Event.current;
        if (m_Event.type == EventType.MouseDown && m_Event.button == 0 && m_Event.control)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(m_Event.mousePosition);
            Vector3 pos = ray.origin + ray.direction * 10f;

            drawShape.points.Add(pos);
            EditorUtility.SetDirty(drawShape);
            m_Event.Use();
        }
    }

}
