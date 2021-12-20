using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
using System.IO;
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

    string seqPath = "Assets/Resources/Prefab/FerrisWheelSequences/";
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

        ///1. fare le funzioni con begin e end change per sicurezza
        ///2. dovrei aggiungere il fatto che se cambio il numero, dovrei cambiare automaticamente anche numero sequenza -> fare punto 1 e in più implementare il numero di sequanza come le cose di geek4geek oppure lo setto semplicemente ad 1
        ///3. fare in modo che il numero sia sempre pari (se riesco mettere anche il popup overlay che lo specifica) -> forse è meglio così, anzichè fare vari reset perchè ci sono anche i numeri primi in ballo -> oppure sbattertene perchè tanto non faccio cambiare variabile per un numero che non divide per intero....
        EditorGUILayout.PropertyField(m_numeroCabine, new GUIContent("Numero di Cabine"), GUILayout.Height(20));

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
                if (numCab % nsq == 0) //ti permette di non cambiare se la lunghezza di sequnza non divide il numero di cabine
                {
                    lunghezzaSeq = nsq;
                }
            }
        }

        return lunghezzaSeq;
    }

    void savePrefab(FerrisWheelManager script)
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
        PrefabUtility.SaveAsPrefabAssetAndConnect(script.gameObject, localPath, InteractionMode.UserAction);
    }
}
