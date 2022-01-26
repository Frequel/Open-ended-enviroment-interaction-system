using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BackgroundBoundsCalculator))]
[CanEditMultipleObjects]
public class BackGroundManagerEditor : Editor
{
    SerializedProperty m_maxYavailable;
    Vector3 bgSize;


    private void OnEnable()
    {
        m_maxYavailable = serializedObject.FindProperty("maxYavailable");
    }

    BackgroundBoundsCalculator script;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        script = (BackgroundBoundsCalculator)target;

        bgSize = script.GetComponent<SpriteRenderer>().sprite.bounds.size;

        //m_maxYavailable.floatValue = SetOffset(m_maxYavailable.floatValue, 0, bgSize.y);
        m_maxYavailable.floatValue = SetOffset(m_maxYavailable.floatValue, -bgSize.y / 2, bgSize.y/2);

        this.serializedObject.ApplyModifiedProperties();
    }

    private float SetOffset(float value, float min, float max)
    {

        EditorGUI.BeginChangeCheck();
        {
            float bp = EditorGUILayout.Slider(value, min, max);
            if (EditorGUI.EndChangeCheck())
            {
                value = bp;
            }
        }

        return value;
    }
}
