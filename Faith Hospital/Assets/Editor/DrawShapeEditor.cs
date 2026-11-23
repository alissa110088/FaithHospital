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

        SpriteRenderer sprite = null;
        if (drawShape.visualFeedBack.Length > 0)
        {
            sprite = drawShape.visualFeedBack[0];
        }

        if (m_Event.type == EventType.MouseDown && m_Event.button == 0 && m_Event.control)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(m_Event.mousePosition);
            Plane plane = new Plane();

            if (sprite != null)
            {
                Vector3 normal = -sprite.transform.forward;
                plane = new Plane(normal, sprite.transform.position);
            }
            else
            {
                plane = new Plane(drawShape.transform.forward, drawShape.transform.position);
                Debug.Log("WKJGHDSUKYHJGDJSHKGHIUFDS");
            }

            if (plane.Raycast(ray, out float enter))
            {
                Vector3 pos = ray.GetPoint(enter);
                Undo.RecordObject(drawShape, "Add Point");
                drawShape.points.Add(pos);
                EditorUtility.SetDirty(drawShape);
                m_Event.Use();
            }
        }
    }
}