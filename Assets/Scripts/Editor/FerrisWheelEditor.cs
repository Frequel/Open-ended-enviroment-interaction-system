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
    SerializedProperty m_rotationDuration;

    Sprite[] spriteArray;
    private static string[] cabine;

    private void OnEnable()
    {
        spriteArray = Resources.LoadAll<Sprite>("Sprites/Cabine");
        cabine = Array.ConvertAll(spriteArray, t => t.name);

        m_numeroCabine = serializedObject.FindProperty("numeroCabine");
        m_numeroSequenza = serializedObject.FindProperty("numeroSequenza");
        m_indiceSpritePerSequenza = serializedObject.FindProperty("indiceSpritePerSequenza");
        m_rotationDuration = serializedObject.FindProperty("rotationDuration");
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FerrisWheelManager script = (FerrisWheelManager)target;

        ///fare le funzioni con begin e end change per sicurezza
        
        EditorGUILayout.PropertyField(m_numeroCabine, new GUIContent("Numero di Cabine"), GUILayout.Height(20)); //fare in modo che il numero sia sempre pari (se riesco mettere anche il popup overlay che lo specifica)

        //m_numeroSequenza.intValue = EditorGUILayout.IntSlider("Numero Sequenza", m_numeroSequenza.intValue, 1, m_numeroCabine.intValue);
        //versione alternativa che resetta se non è divisibile

        m_numeroSequenza.intValue = SetNumSeq(m_numeroSequenza.intValue, m_numeroCabine.intValue, "Numero Sequenza");


        //fare in modo che m_numeroSequenza.intValue sia un divisore di m_numeroCabine.intValue, quindi vedere se sono divisibili e incrementare di conseguenza
        //una cosa fattible sarebbe salvarsi il valore vecchio e risettare quello se il valore inserito non và bene -> usare begin e end change
        m_indiceSpritePerSequenza.arraySize = m_numeroSequenza.intValue;

        EditorGUILayout.PropertyField(m_rotationDuration, new GUIContent("Durata Giro completo"), GUILayout.Height(20));

        for (int i = 0; i < m_numeroSequenza.intValue; i++)
            DrawComponentsPopup(cabine, i, "Cabina " + (i+1));

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

    private int SetNumSeq(int lunghezzaSeq, int numCab, string label= "Numero Sequenza")
    {
        
        EditorGUI.BeginChangeCheck();
        {
            int nsq = EditorGUILayout.IntSlider("Numero Sequenza", lunghezzaSeq, 1, numCab);
            if (EditorGUI.EndChangeCheck())
            {
                if (numCab % nsq == 0)
                {
                    lunghezzaSeq = nsq;
                }
            }
        }

        return lunghezzaSeq;
    }
}
