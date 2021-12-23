using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
using System.IO;
using TMPro;

[CustomEditor(typeof(FerrisWheelStructManager))]
[CanEditMultipleObjects]
public class FerrisWheelStructEditor : Editor
{
    SerializedProperty m_numeroCabine;
    SerializedProperty m_numeroSequenza;
    SerializedProperty m_indiceSpritePerSequenza;
    SerializedProperty m_rotationDuration;

    Sprite[] spriteArray;
    private static string[] cabine;

    string seqPath = "Assets/Resources/Prefab/FerrisWheelSequences/AnnalisaVersion/"; //da modificare con i prefab di asset di annalisa
    private void OnEnable()
    {
        spriteArray = Resources.LoadAll<Sprite>("Sprites/RuotaAnnalisa/Cabine");
        cabine = Array.ConvertAll(spriteArray, t => t.name);

        m_numeroCabine = serializedObject.FindProperty("numeroCabine");
        m_numeroSequenza = serializedObject.FindProperty("numeroSequenza");
        m_indiceSpritePerSequenza = serializedObject.FindProperty("indiceSpritePerSequenza");
        m_rotationDuration = serializedObject.FindProperty("rotationDuration");
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FerrisWheelStructManager script = (FerrisWheelStructManager)target;

        SetNumCab();

        m_numeroSequenza.intValue = SetNumSeq(m_numeroSequenza.intValue, m_numeroCabine.intValue, "Numero Sequenza");

        m_indiceSpritePerSequenza.arraySize = m_numeroSequenza.intValue;

        EditorGUILayout.PropertyField(m_rotationDuration, new GUIContent("Durata Giro completo"), GUILayout.Height(20));

        for (int i = 0; i < m_numeroSequenza.intValue; i++)
            DrawComponentsPopup(cabine, i, "Cabina " + (i+1));

        this.serializedObject.ApplyModifiedProperties();


        GUILayout.Space(20);

        if (GUILayout.Button("Save Prefab"))
        {
            savePrefab(script);
        }
    }

    private void SetNumCab()
    {

        EditorGUI.BeginChangeCheck();
        {
            EditorGUILayout.PropertyField(m_numeroCabine, new GUIContent("Numero di Cabine"), GUILayout.Height(20));
            if (EditorGUI.EndChangeCheck())
            {
                m_numeroSequenza.intValue = 1;
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
                if (numCab % nsq == 0) //ti permette di non cambiare se la lunghezza di sequnza non divide il numero di cabine
                {
                    lunghezzaSeq = nsq;
                }
            }
        }

        return lunghezzaSeq;
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

    void savePrefab(FerrisWheelStructManager script)
    {

        string dirPath = seqPath + m_numeroCabine.intValue;

        if (!Directory.Exists(dirPath))
        {
            //if it doesn't, create it
            Directory.CreateDirectory(dirPath);
        }

        string localPath = dirPath + "/" + script.gameObject.name + ".prefab";

        // Make sure the file name is unique, in case an existing Prefab has the same name.
        localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

        // Create the new Prefab.
        PrefabUtility.SaveAsPrefabAssetAndConnect(script.gameObject, localPath, InteractionMode.UserAction); //sarebbe da cambiare per salvare tutto, anche la base e il centro ma poi devo cambiare altro nello script,per ora lascio così
        //PrefabUtility.SaveAsPrefabAssetAndConnect(script.transform.parent.transform.parent.gameObject, localPath, InteractionMode.UserAction);
    }
}
