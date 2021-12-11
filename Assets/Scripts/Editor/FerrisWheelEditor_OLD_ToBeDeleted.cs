using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
using TMPro;

[CustomEditor(typeof(FerrisWheelManager_OLS_ToBeDeleted))]
[CanEditMultipleObjects]
public class FerrisWheelEditor_OLD_ToBeDeleted : Editor
{
    private string[] drag;

    //static int numeroCabine;
    
    //static int numeroSequenza = 2 ;

    Sprite[] spriteArray;
    GameObject[] gmObArray;
    private static string[] sequences;
    private static string[] cabine;
    private static int[] DaDecidere;

    
    static bool inizialized = false; //per non far resettare ogni volta gli array quando li inizializzo all'inizio

    //testing
    SerializedProperty m_provaPD;
    SerializedProperty m_EmissionColor;
    SerializedProperty m_EmitRed;
    SerializedProperty m_EmitGreen;
    SerializedProperty m_EmitBlue;

    SerializedProperty m_indiceSpritePerSequenza;
    SerializedProperty m_numeroCabine;
    SerializedProperty m_numeroSequenza;


    private void OnEnable()
    {
        spriteArray = Resources.LoadAll<Sprite>("Sprites/Cabine");
        cabine = Array.ConvertAll(spriteArray, t => t.name);


        /////testing
        m_provaPD = serializedObject.FindProperty("provaPD");
        m_EmissionColor = this.serializedObject.FindProperty("emissionColor");
        m_EmitRed = this.serializedObject.FindProperty("emitRed");
        m_EmitGreen = this.serializedObject.FindProperty("emitGreen");
        m_EmitBlue = this.serializedObject.FindProperty("emitBlue");

        //m_indiceSpritePerSequenza = this.serializedObject.FindProperty("indiceSpritePerSequenza");
        //m_numeroCabine = this.serializedObject.FindProperty("numeroCabine");
        //m_numeroSequenza = this.serializedObject.FindProperty("numeroSequenza");

        FerrisWheelManager_OLS_ToBeDeleted script = (FerrisWheelManager_OLS_ToBeDeleted)target;

        if (!inizialized && FerrisWheelManager_OLS_ToBeDeleted.indiceSpritePerSequenza == null) //script.indiceSpritePerSequenza == null)
        {
            inizializzaArray(script);
            Undo.RecordObject(target, "Changed inizialized data");
            inizialized = true;
        }

    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        this.serializedObject.Update ();

        FerrisWheelManager_OLS_ToBeDeleted script = (FerrisWheelManager_OLS_ToBeDeleted)target;

        //if (!inizialized && FerrisWheelManager.indiceSpritePerSequenza == null) //script.indiceSpritePerSequenza == null)
        //{
        //    inizializzaArray(script);
        //    Undo.RecordObject(target, "Changed inizialized data");
        //    inizialized = true;
        //}

        //numeroCabine = EditorGUILayout.IntField("Numero Cabine", numeroCabine);
        //numSeqChange(ref numeroSequenza, script, numeroCabine, "boh");

        //for (int i = 0; i < numeroSequenza; i++)
        //    DrawComponentsPopup(script, cabine, i, "Cabina " + i);

        //EditorGUILayout.PropertyField(m_provaPD, new GUIContent("Int Field"), GUILayout.Height(20));
        //EditorGUILayout.PropertyField(m_EmissionColor);

        //bool emitRed, emitGreen, emitBlue;
        //colorStringToBools(m_EmissionColor.stringValue, out emitRed, out emitGreen, out emitBlue);
        //m_EmitRed.boolValue = emitRed;
        //m_EmitGreen.boolValue = emitGreen;
        //m_EmitBlue.boolValue = emitBlue;

        Undo.RecordObject(target, "Changed numero cabine data");
        script.numeroCabine = EditorGUILayout.IntField("Numero Cabine", script.numeroCabine);
        numSeqChange(ref script.numeroSequenza, script, script.numeroCabine, "boh");

        for (int i = 0; i < script.numeroSequenza; i++)
            DrawComponentsPopup(script, cabine, i, "Cabina " + i);

        this.serializedObject.ApplyModifiedProperties();
    }

    void colorStringToBools(String stringa, out bool emitRed, out bool emitGreen, out bool emitBlue)
    {
        if (stringa.Equals("red"))
        {
            emitRed = true;
            emitGreen = false;
            emitBlue = false;
        }
        else if (stringa.Equals("green"))
        {
            emitRed = false;
            emitGreen = true;
            emitBlue = false;
        }
        else if (stringa.Equals("blue"))
        {
            {
                emitRed = false;
                emitGreen = false;
                emitBlue = true;
            }
        }
        else
        {
            emitRed = false;
            emitGreen = false;
            emitBlue = false;
        }
    }
    void inizializzaArray(FerrisWheelManager_OLS_ToBeDeleted fwm)
    {
        Undo.RecordObject(target, "Changed xseq data");
        //fwm.indiceSpritePerSequenza = new int[fwm.numeroSequenza];
        FerrisWheelManager_OLS_ToBeDeleted.indiceSpritePerSequenza = new int[fwm.numeroSequenza];

        for (int i = 0; i < fwm.numeroSequenza; i++)
        {
            Undo.RecordObject(target, "Changed xseq data");
            //fwm.indiceSpritePerSequenza[i] = 0;
            FerrisWheelManager_OLS_ToBeDeleted.indiceSpritePerSequenza[i] = 0;
        }

        Undo.RecordObject(target, "Changed DaDecidere data");
        DaDecidere = new int[fwm.numeroSequenza];

        for (int i = 0; i < fwm.numeroSequenza; i++)
        {
            Undo.RecordObject(target, "Changed DaDecidere data");
            DaDecidere[i] = 0;
        }
    }

    private void numSeqChange(ref int seqArr, FerrisWheelManager_OLS_ToBeDeleted fwm, int maxCab, string label = "proviamo sta pazzia")
    {
        EditorGUI.BeginChangeCheck();
        {
            //int oldNumSeq = seqArr;
            //int act = EditorGUILayout.IntField("Numero Sequenza", seqArr);

            
            int sqa = EditorGUILayout.IntSlider("Numero Sequenza", seqArr, 1, maxCab);
            //seqArr = EditorGUILayout.IntField("Numero Sequenza", seqArr);
            //seqArr = EditorGUILayout.IntSlider("Numero Sequenza", seqArr,1, maxCab); //da testare l'uno
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Changed numero sequenza data");
                seqArr = sqa;

                //forse sarebbe consono aggiungere un delete dell'array per gestire meglio la memoria
                inizializzaArray(fwm);
                //oppure usi list o arraylist che dovrebbe aggiungere direttamente alla fine
                //Undo.RecordObject(target, "Changed xseq data");
                //fwm.indiceSpritePerSequenza = new int[fwm.numeroSequenza];
                ////FerrisWheelManager.indiceSpritePerSequenza = new int[fwm.numeroSequenza];

                //for (int i = 0; i < fwm.numeroSequenza; i++)
                //{
                //    Undo.RecordObject(target, "Changed xseq data");
                //    fwm.indiceSpritePerSequenza = new int[fwm.numeroSequenza];
                //    //FerrisWheelManager.indiceSpritePerSequenza = new int[fwm.numeroSequenza];
                //}


                //Undo.RecordObject(target, "Changed DaDecidere data");
                //DaDecidere = new int[fwm.numeroSequenza];

                //for (int i = 0; i < fwm.numeroSequenza; i++)
                //{
                //    Undo.RecordObject(target, "Changed DaDecidere data");
                //    DaDecidere[i] = 0;
                //}
            }
        }
    }

    private void DrawComponentsPopup(FerrisWheelManager_OLS_ToBeDeleted fwm, string[] options, int i, string label = "cabina")
    {
        EditorGUI.BeginChangeCheck();
        {
            //int oldTypeIndex = seqIndex[i];
            //int typeIndex = EditorGUILayout.Popup(label, oldTypeIndex, options);
            int dd = EditorGUILayout.Popup(label, DaDecidere[i], options);
            //DaDecidere[i] = EditorGUILayout.Popup(label, DaDecidere[i], options);
            if (EditorGUI.EndChangeCheck())
            {
                //if (oldTypeIndex >= 0)
                //    DestroyImmediate(component);
                //if (typeIndex >= 0)
                //    //Debug.Log("in fase di test");//targetObject.AddComponent(sprites[typeIndex]);
                //    seqIndex[i] = typeIndex;
                //    fwm.indiceSpritePerSequenza[i] = typeIndex;
                //fwm.indiceSpritePerSequenza[i] = DaDecidere[i];
                Undo.RecordObject(target, "Changed DaDecidere data");
                DaDecidere[i] = dd;
                Undo.RecordObject(target, "Changed xseq data");
                //fwm.indiceSpritePerSequenza = new int[fwm.numeroSequenza];
                FerrisWheelManager_OLS_ToBeDeleted.indiceSpritePerSequenza = new int[fwm.numeroSequenza];
            }
        }
    }
}
