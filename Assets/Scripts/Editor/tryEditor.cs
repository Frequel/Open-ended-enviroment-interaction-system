using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;

[CustomEditor(typeof(TestEditorScripting))]
public class TrackEditor : Editor
{
    SerializedProperty m_provaPD;
    SerializedProperty m_provaArray;
    SerializedProperty m_provaArrayString;

    SerializedProperty m_numeroCabine;
    SerializedProperty m_numeroSequenza;
    SerializedProperty m_indiceSpritePerSequenza;

    Sprite[] spriteArray;
    private static string[] cabine;

    private void OnEnable()
    {
        spriteArray = Resources.LoadAll<Sprite>("Sprites/Cabine");
        cabine = Array.ConvertAll(spriteArray, t => t.name);

        m_provaPD = serializedObject.FindProperty("provaPD");
        m_provaArray = serializedObject.FindProperty("provaArray");
        m_provaArrayString = serializedObject.FindProperty("provaArrayString");

        m_numeroCabine = serializedObject.FindProperty("numeroCabine");
        m_numeroSequenza = serializedObject.FindProperty("numeroSequenza");
        m_indiceSpritePerSequenza = serializedObject.FindProperty("indiceSpritePerSequenza");

    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        //this.serializedObject.Update ();
        TestEditorScripting script = (TestEditorScripting)target;


        EditorGUILayout.PropertyField(m_provaPD, new GUIContent("Prova"), GUILayout.Height(20));

        for (int x = 0; x < m_provaArray.arraySize; x++)
        {
            SerializedProperty property = m_provaArray.GetArrayElementAtIndex(x); // get array element at x
            property.intValue = Mathf.Max(0, property.intValue); // Edit this element's value, in this case limit the float's value to a positive value.
        }
        EditorGUILayout.PropertyField(m_provaArray, true); // draw property with it's children

        for (int x = 0; x < m_provaArrayString.arraySize; x++)
        {
            SerializedProperty property = m_provaArrayString.GetArrayElementAtIndex(x); // get array element at x
            property.stringValue = property.stringValue; // Edit this element's value, in this case limit the float's value to a positive value.
        }
        EditorGUILayout.PropertyField(m_provaArrayString, true); // draw property with it's children


        EditorGUILayout.PropertyField(m_numeroCabine, new GUIContent("Numero di Cabine"), GUILayout.Height(20));
        //EditorGUILayout.PropertyField(m_numeroSequenza, new GUIContent("Lunghezza Sequenza"), GUILayout.Height(20));
        m_numeroSequenza.intValue = EditorGUILayout.IntSlider("Numero Sequenza", m_numeroSequenza.intValue, 1, m_numeroCabine.intValue);

        m_indiceSpritePerSequenza.arraySize = m_numeroSequenza.intValue;



        for (int i = 0; i < script.numeroSequenza; i++)
            DrawComponentsPopup(script, cabine, i, "Cabina " + i);






        this.serializedObject.ApplyModifiedProperties();

    }

    private void DrawComponentsPopup(TestEditorScripting fwm, string[] options, int i, string label = "cabina")
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