using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues; //used for "Special Operations" fade group

[CustomEditor(typeof(LiquidDensityInteractor))]
[CanEditMultipleObjects]
public class LiquidContainerEditor : Editor
{
    SerializedProperty m_bottomPosition;
    Collider m_Collider;

    private void OnEnable()
    {
        m_bottomPosition = serializedObject.FindProperty("bottomPosition");
    }

    LiquidDensityInteractor script;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        script = (LiquidDensityInteractor)target;

        m_Collider = script.GetComponent<Collider>();



        SetBottomPosition();

        this.serializedObject.ApplyModifiedProperties();
    }

    private void SetBottomPosition()
    {

        EditorGUI.BeginChangeCheck();
        {
            //float bp = EditorGUILayout.FloatField(m_bottomPosition.floatValue); // EditorGUILayout.IntSlider("Numero Cabine", m_cabNum.intValue, 1, 100);
            float bp = EditorGUILayout.Slider(m_bottomPosition.floatValue, 0, m_Collider.bounds.size.y);
            if (EditorGUI.EndChangeCheck())
            {
                m_bottomPosition.floatValue = bp;
            }
        }
    }

    void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        Gizmos.color = Color.yellow;

        //m_Collider = script.GetComponent<Collider>();
        Vector3 from = new Vector3(m_Collider.bounds.min.x, m_bottomPosition.floatValue, script.transform.position.z);
        Vector3 to = new Vector3(m_Collider.bounds.max.x, script.transform.position.y + m_bottomPosition.floatValue, script.transform.position.z);
        Gizmos.DrawLine(from, to);
    }
}
