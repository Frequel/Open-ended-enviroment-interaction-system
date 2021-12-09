using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
using TMPro;

[CustomEditor(typeof(FerrisWheelManager))]
public class FerrisWheelEditor : Editor
{
    ////provvisorio, in realtà mi serve qualcosa di grande quanto i prefab che carico o l'array di prefab
    //[SerializeField]
    //[Range(0, 5)] //bib posso selezionare cose fuori range -> se aggiungo sprite disponibili => aumento il range a destra
    ////lo puoi popolare, anziche come range d'interi, come popup di prefab e ti fai restituire semplicemente l'index, senza fare altro con in itialializer
    //int[] indiceSpritePerSequenza;// = new int[numeroSequenza]; //ci vorrebbe pure una maniera per farlo grande quanto numeroSequenza
    

    private string[] drag;

    //[SerializeField]
    static int numeroCabine;
    //[SerializeField]
    static int numeroSequenza = 2 ;

    //come caricare le resources

    //void Start()
    //{
    //    m_SpriteRenderer = GetComponent<SpriteRenderer>();
    //    ckSeq = gameObject.GetComponentInParent<CheckSequence>();
    //    spriteArray = Resources.LoadAll<Sprite>("Sprites/Cabine");
    //    i = Random.Range(0, 5);
    //    m_SpriteRenderer.sprite = spriteArray[i];
    //}

    Sprite[] spriteArray;
    GameObject[] gmObArray;
    private static string[] sequences;
    private static string[] cabine;
    private static int[] DaDecidere;


    //testing
    static bool inizialized = false;
    private void OnEnable()
    {
        //gmObArray = Resources.LoadAll<GameObject>("Prefab/FerrisWheelSequences");
        //sequences = Array.ConvertAll(gmObArray, t => t.name);

        spriteArray = Resources.LoadAll<Sprite>("Sprites/Cabine");
        cabine = Array.ConvertAll(spriteArray, t => t.name);

        //interactors = Array.ConvertAll(interactorsType, t => t.Name);
        //drag = Array.ConvertAll(dragType, t => t.Name);

        //FerrisWheelManager script = (FerrisWheelManager)target;
        //script.indiceSpritePerSequenza = new int[numeroSequenza];

        //for (int i = 0; i < numeroSequenza; i++)
        //    script.indiceSpritePerSequenza[i] = 0;

        //DaDecidere = new int[numeroSequenza];

        //for (int i = 0; i < numeroSequenza; i++)
        //    DaDecidere[i] = 0;
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FerrisWheelManager script = (FerrisWheelManager)target;

        if (!inizialized)
        {
            inizializzaArray(script);
            inizialized = true;
        }

        //script.xDebug();

        //numeroSequenza = EditorGUILayout.IntField("Numero Sequenza", numeroSequenza);
        numeroCabine = EditorGUILayout.IntField("Numero Cabine", numeroCabine);
        numSeqChange(ref numeroSequenza, script, numeroCabine, "boh");

        //int i = 0;
        for (int i = 0; i < numeroSequenza; i++)
            //DrawComponentsPopup(script, cabine, i, ref DaDecidere, "Cabina " + i);
            DrawComponentsPopup(script, cabine, i, "Cabina " + i);

        //DrawComponentsPopup(script, cabine, i, "Cabina " + i);

    }

    void inizializzaArray(FerrisWheelManager fwm)
    {
        fwm.indiceSpritePerSequenza = new int[numeroSequenza];

        for (int i = 0; i < numeroSequenza; i++)
            fwm.indiceSpritePerSequenza[i] = 0;


        DaDecidere = new int[numeroSequenza];

        for (int i = 0; i < numeroSequenza; i++)
            DaDecidere[i] = 0;
    }

    private void numSeqChange(ref int seqArr, FerrisWheelManager fwm, int maxCab, string label = "proviamo sta pazzia")
    {
        EditorGUI.BeginChangeCheck();
        {
            //int oldNumSeq = seqArr;
            //int act = EditorGUILayout.IntField("Numero Sequenza", seqArr);

            //seqArr = EditorGUILayout.IntField("Numero Sequenza", seqArr);
            seqArr = EditorGUILayout.IntSlider("Numero Sequenza", seqArr,1, maxCab); //da testare l'uno
            if (EditorGUI.EndChangeCheck())
            {
                //oppure usi list o arraylist che dovrebbe aggiungere direttamente alla fine
                fwm.indiceSpritePerSequenza = new int[numeroSequenza];

                for (int i = 0; i < numeroSequenza; i++)
                    fwm.indiceSpritePerSequenza[i] = 0;


                DaDecidere = new int[numeroSequenza];

                for (int i = 0; i < numeroSequenza; i++)
                    DaDecidere[i] = 0;
            }
        }
    }

    //private void DrawComponentsPopup(FerrisWheelManager fwm, string[] options, int i, ref int [] seqIndex,string label = "cabina")
    //private void DrawComponentsPopup(GameObject targetObject, string[] options, Sprite[] sprites, int pd, out int seqIndex, string label = "Component")
    private void DrawComponentsPopup(FerrisWheelManager fwm, string[] options, int i, string label = "cabina")
    {
        EditorGUI.BeginChangeCheck();
        {
            //int oldTypeIndex = seqIndex[i];
            //int typeIndex = EditorGUILayout.Popup(label, oldTypeIndex, options);
            DaDecidere[i] = EditorGUILayout.Popup(label, DaDecidere[i], options);
            if (EditorGUI.EndChangeCheck())
            {
                //if (oldTypeIndex >= 0)
                //    DestroyImmediate(component);
                //if (typeIndex >= 0)
                //    //Debug.Log("in fase di test");//targetObject.AddComponent(sprites[typeIndex]);
                //    seqIndex[i] = typeIndex;
                //    fwm.indiceSpritePerSequenza[i] = typeIndex;
                fwm.indiceSpritePerSequenza[i] = DaDecidere[i];
            }
        }
    }
}
