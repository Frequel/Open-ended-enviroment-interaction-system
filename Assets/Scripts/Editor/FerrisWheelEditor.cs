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
    private static readonly Type[] interactorsType = new Type[]
    {
        typeof(toColorInteractor),
        typeof(toWeighInteractor),
        typeof(doubleHeightInteractor),
        typeof(doubleWidthInteractor),
        typeof(widthDivisorInteractor),
        typeof(heightDivisorInteractor),
        typeof(toColorText), //questo ? un test per fare un oggetto con azione specifica, per fare le cose per bene sarebbe daimpedire di coesistere con altri (ma credo che già con il Popup riesco nell'intento)
        typeof(toColorBackground),
        typeof(penInteractor),
        typeof(ObjectInteractor)
    };
    private static string[] interactors;

    //provvisorio, in realtà mi serve qualcosa di grande quanto i prefab che carico o l'array di prefab
    [SerializeField]
    [Range(0, 5)] //bib posso selezionare cose fuori range -> se aggiungo sprite disponibili => aumento il range a destra
    //lo puoi popolare, anziche come range d'interi, come popup di prefab e ti fai restituire semplicemente l'index, senza fare altro con in itialializer
    int[] indiceSpritePerSequenza;// = new int[numeroSequenza]; //ci vorrebbe pure una maniera per farlo grande quanto numeroSequenza
    

    private string[] drag;

    [SerializeField]
    int numeroCabine;
    [SerializeField]
    int numeroSequenza = 2 ;

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

    private void OnEnable()
    {
        //gmObArray = Resources.LoadAll<GameObject>("Prefab/FerrisWheelSequences");
        //sequences = Array.ConvertAll(gmObArray, t => t.name);

        spriteArray = Resources.LoadAll<Sprite>("Sprites/Cabine");
        cabine = Array.ConvertAll(spriteArray, t => t.name);

        //interactors = Array.ConvertAll(interactorsType, t => t.Name);
        //drag = Array.ConvertAll(dragType, t => t.Name);

        FerrisWheelManager script = (FerrisWheelManager)target;
        script.indiceSpritePerSequenza = new int[numeroSequenza];

        for (int i = 0; i < numeroSequenza; i++)
            script.indiceSpritePerSequenza[i] = 0;
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FerrisWheelManager script = (FerrisWheelManager)target;

        //script.xDebug();

        //numeroSequenza = EditorGUILayout.IntField("Numero Sequenza", numeroSequenza);
        numeroCabine = EditorGUILayout.IntField("Numero Cabine", numeroCabine);
        numSeqChange(ref numeroSequenza, script, numeroCabine, "boh");

        //int numeroDelPorcoDio;
        //int pd=0;
        ////iteri per il numero sequenza e ci passi dentro invece che interactors
        //DrawComponentsPopup(script.gameObject, cabine, spriteArray, ref script.indiceSpritePerSequenza[0], "cabina1");
        //DrawComponentsPopup(script.gameObject, cabine, spriteArray, pd, out numeroDelPorcoDio, "Interactor");
        //cosa contorta ma che dovrebbe funzionare
        //numeroDelPorcoDio = script.indiceSpritePerSequenza[0];
        //pd = numeroDelPorcoDio;
        //pd =  script.indiceSpritePerSequenza[0];
        //DrawComponentsPopup(script.gameObject, cabine, spriteArray, pd, out numeroDelPorcoDio, "cabina1");
        //script.indiceSpritePerSequenza[0] = numeroDelPorcoDio;

        for (int i = 0; i < numeroSequenza; i++)
            DrawComponentsPopup(script.gameObject, cabine, spriteArray, ref script.indiceSpritePerSequenza[i], "Cabina " + i);

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
            }
        }
    }

    private void DrawComponentsPopup(GameObject targetObject, string[] options, Sprite[] sprites, ref int seqIndex,string label = "cabina")
    //private void DrawComponentsPopup(GameObject targetObject, string[] options, Sprite[] sprites, int pd, out int seqIndex, string label = "Component")
    {
        EditorGUI.BeginChangeCheck();
        {
            ////int oldTypeIndex = GetExistingComponentIndex(targetObject, out Component component, sprites);
            //int oldTypeIndex = GetExistingComponentIndex(targetObject, out Component component, sprites); 
            ////IndexArray.FindIndex(sprites, t => t.name == );
            //int typeIndex = EditorGUILayout.Popup(label, oldTypeIndex, options);
            //seqIndex = EditorGUILayout.Popup(label, seqIndex, options);
            //seqIndex = -1;
            //int oldTypeIndex = pd;
            //int typeIndex = EditorGUILayout.Popup(label, oldTypeIndex, options);
            //hasComponent = GUILayout.Toggle(hadComponent, label ?? typeof(TextMeshPro).Name);

            int oldTypeIndex = seqIndex;
            int typeIndex = EditorGUILayout.Popup(label, oldTypeIndex, options);
            if (EditorGUI.EndChangeCheck())
            {
                //if (oldTypeIndex >= 0)
                //    DestroyImmediate(component);
                if (typeIndex >= 0)
                    //Debug.Log("in fase di test");//targetObject.AddComponent(sprites[typeIndex]);
                    seqIndex = typeIndex;
                targetObject.GetComponent<FerrisWheelManager>().xDebug();
            }
        }
    }

    //modificare con l'ultimo argomento di tipo gameobject o quello che serve a me
    private int GetExistingComponentIndex(GameObject targetObject, out Component component, Sprite[] sprites)
    {
        Component c = null;
        int index = 0;// Array.FindIndex(sprites, t => t.name == );
        component = c;
        return index;
    }
}
