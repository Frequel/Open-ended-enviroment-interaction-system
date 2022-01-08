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
public class FerrisWheelEditor : Editor
{
    SerializedProperty m_cabNum;
    SerializedProperty m_seqLenght;
    SerializedProperty m_seqSpriteIndex;
    SerializedProperty m_rotationDuration;

    Sprite[] spriteArray;
    private static string[] cabine;

    string seqPath = "Assets/Resources/Prefab/FerrisWheelSequences/"; //da riscrivere quando ho degli asset definitivi
    private void OnEnable()
    {
        spriteArray = Resources.LoadAll<Sprite>("Sprites/FerrisWheel/Cabine/Cabine_fruit/");
        cabine = Array.ConvertAll(spriteArray, t => t.name);

        m_cabNum = serializedObject.FindProperty("cabNum");
        m_seqLenght = serializedObject.FindProperty("seqLenght");
        m_seqSpriteIndex = serializedObject.FindProperty("seqSpriteIndex");
        m_rotationDuration = serializedObject.FindProperty("rotationDuration");
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FerrisWheelManager script = (FerrisWheelManager)target;

        //SetNumCab(script); //future improvements
        SetNumCab();

        m_seqLenght.intValue = SetNumSeq(m_seqLenght.intValue, m_cabNum.intValue, "Numero Sequenza");

        m_seqSpriteIndex.arraySize = m_seqLenght.intValue;

        EditorGUILayout.PropertyField(m_rotationDuration, new GUIContent("Durata Giro completo"), GUILayout.Height(20));

        for (int i = 0; i < m_seqLenght.intValue; i++)
            DrawComponentsPopup(cabine, i, "Cabina " + (i + 1));

        this.serializedObject.ApplyModifiedProperties();


        GUILayout.Space(20);

        if (GUILayout.Button("Save Prefab"))
        {
            savePrefab(script);
        }
    }

    //possibile prime numbers
    //private void SetNumCab(FerrisWheelManager fwm) //improvement to be made in the future
    //private void SetNumCab()
    //{

    //    EditorGUI.BeginChangeCheck();
    //    {
    //        EditorGUILayout.PropertyField(m_cabNum, new GUIContent("Numero di Cabine"), GUILayout.Height(20));
    //        if (EditorGUI.EndChangeCheck())
    //        {
    //            m_seqLenght.intValue = 1;
    //            //improvement to be made in the future
    //            //per stategia di fare tutta la ruota con tutte le cabine già visibili ma abortita perchè compelssa e richiedeva troppo tempo -> se il numCab è lo stesso lasci stare così, se cambi lo devi reistanziare
    //            //fwm.DestroyChild();
    //            //fwm.InstantiateCabin();
    //        }
    //    }
    //}

    //no prime numbers
    //private void SetNumCab(FerrisWheelManager fwm) //improvement to be made in the future
    private void SetNumCab()
    {

        EditorGUI.BeginChangeCheck();
        {
            int ncb = EditorGUILayout.IntSlider("Numero Cabine", m_cabNum.intValue, 1, 100);
            if (EditorGUI.EndChangeCheck())
            {
                if (ncb % 2 == 0) //change cabin quantity only if even, so i avoid prime numbers
                {
                    m_cabNum.intValue = ncb;
                }

                m_seqLenght.intValue = 1;

                //improvement to be made in the future
                //per stategia di fare tutta la ruota con tutte le cabine già visibili ma abortita perchè compelssa e richiedeva troppo tempo -> se il numCab è lo stesso lasci stare così, se cambi lo devi reistanziare
                //fwm.DestroyChild();
                //fwm.InstantiateCabin();
            }
        }
    }


    private int SetNumSeq(int lunghezzaSeq, int numCab, string label = "Numero Sequenza")
    {
        EditorGUI.BeginChangeCheck();
        {
            int nsq = EditorGUILayout.IntSlider("Numero Sequenza", lunghezzaSeq, 1, numCab);
            if (EditorGUI.EndChangeCheck())
            {
                if (numCab % nsq == 0) //change sequence lenght only if the number of Cabin is divisible by the lenght
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
            int dd = EditorGUILayout.Popup(label, m_seqSpriteIndex.GetArrayElementAtIndex(i).intValue, options);
            if (EditorGUI.EndChangeCheck())
            {
                m_seqSpriteIndex.GetArrayElementAtIndex(i).intValue = dd;
            }
        }
    }

    void savePrefab(FerrisWheelManager script)
    {
        GameObject toSave = script.transform.parent.transform.parent.gameObject;

        string dirPath = seqPath + m_cabNum.intValue;

        if (!Directory.Exists(dirPath))
        {
            //if it doesn't, create it
            Directory.CreateDirectory(dirPath);
        }

        string name = m_cabNum.intValue + "Cab_" + m_seqLenght.intValue + "SeqL_" + script.FerrisWheelRadius + "R_ " + m_rotationDuration.intValue + "dur_"; //a useful thing could be append the sequence at the end of the name but it could became too long

        toSave.name = name;

        for(int i = 0; i < m_seqLenght.intValue; i++)
        {
            name += cabine[m_seqSpriteIndex.GetArrayElementAtIndex(i).intValue];
        }

        //string localPath = dirPath + "/" + toSave.name + ".prefab";
        string localPath = dirPath + "/" + name + ".prefab";


        // Make sure the file name is unique, in case an existing Prefab has the same name.
        localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

        // Create the new Prefab.
        PrefabUtility.SaveAsPrefabAsset(toSave, localPath); //Saving the GrandFather that contains all the wheel //struct son of sequence version
    }
}
