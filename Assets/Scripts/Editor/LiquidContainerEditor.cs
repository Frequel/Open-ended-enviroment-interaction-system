using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues; //used for "Special Operations" fade group

[CustomEditor(typeof(LiquidDensityInteractor))]
[CanEditMultipleObjects]
public class LiquidContainerEditor : Editor
{
    SerializedProperty m_bottomPosition;
    SerializedProperty m_centerPosition;
    Collider m_Collider;

    private void OnEnable()
    {
        m_bottomPosition = serializedObject.FindProperty("bottomPosition");
        m_centerPosition = serializedObject.FindProperty("centerPosition");
    }

    LiquidDensityInteractor script;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        script = (LiquidDensityInteractor)target;

        m_Collider = script.GetComponent<Collider>();



        //SetBottomPosition();
        m_bottomPosition.floatValue = SetOffset(m_bottomPosition.floatValue, 0, m_Collider.bounds.size.y);
        m_centerPosition.floatValue = SetOffset(m_centerPosition.floatValue, -m_Collider.bounds.size.x/2, m_Collider.bounds.size.x/2);

        this.serializedObject.ApplyModifiedProperties();
    }

    //private void SetBottomPosition()
    //{

    //    EditorGUI.BeginChangeCheck();
    //    {
    //        //float bp = EditorGUILayout.FloatField(m_bottomPosition.floatValue); // EditorGUILayout.IntSlider("Numero Cabine", m_cabNum.intValue, 1, 100);
    //        float bp = EditorGUILayout.Slider(m_bottomPosition.floatValue, 0, m_Collider.bounds.size.y);
    //        if (EditorGUI.EndChangeCheck())
    //        {
    //            m_bottomPosition.floatValue = bp;
    //        }
    //    }
    //}

    private float SetOffset(float value, float min, float max)
    {

        EditorGUI.BeginChangeCheck();
        {
            //float bp = EditorGUILayout.FloatField(m_bottomPosition.floatValue); // EditorGUILayout.IntSlider("Numero Cabine", m_cabNum.intValue, 1, 100);
            float bp = EditorGUILayout.Slider(value, min, max);
            if (EditorGUI.EndChangeCheck())
            {
                value = bp;
            }
        }

        return value;
    }
}
