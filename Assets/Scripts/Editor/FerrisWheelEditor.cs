using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
using TMPro;

[CustomEditor(typeof(FerrisWheelManager))]
[CanEditMultipleObjects]
public class FerrisWheelEditord : Editor
{
    SerializedProperty m_numeroCabine;
    SerializedProperty m_numeroSequenza;
    SerializedProperty m_indiceSpritePerSequenza;

    Sprite[] spriteArray;
    private static string[] cabine;

    private void OnEnable()
    {
        spriteArray = Resources.LoadAll<Sprite>("Sprites/Cabine");
        cabine = Array.ConvertAll(spriteArray, t => t.name);

        m_numeroCabine = serializedObject.FindProperty("numeroCabine");
        m_numeroSequenza = serializedObject.FindProperty("numeroSequenza");
        m_indiceSpritePerSequenza = serializedObject.FindProperty("indiceSpritePerSequenza");

    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FerrisWheelManager script = (FerrisWheelManager)target;

        ///fare le funzioni con begin e end change per sicurezza

        EditorGUILayout.PropertyField(m_numeroCabine, new GUIContent("Numero di Cabine"), GUILayout.Height(20));

        m_numeroSequenza.intValue = EditorGUILayout.IntSlider("Numero Sequenza", m_numeroSequenza.intValue, 1, m_numeroCabine.intValue);

        m_indiceSpritePerSequenza.arraySize = m_numeroSequenza.intValue;

        for (int i = 0; i < m_numeroSequenza.intValue; i++)
            DrawComponentsPopup(cabine, i, "Cabina " + i);

        this.serializedObject.ApplyModifiedProperties();

    }

    private void DrawComponentsPopup(string[] options, int i, string label = "Cabina")
    {
        EditorGUI.BeginChangeCheck();
        {
            int dd = EditorGUILayout.Popup(label, m_indiceSpritePerSequenza.GetArrayElementAtIndex(i).intValue, options);
            if (EditorGUI.EndChangeCheck())
            {
                m_indiceSpritePerSequenza.GetArrayElementAtIndex(i).intValue = dd;
            }
        }
    }
}
