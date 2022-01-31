using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
using System.Linq;
using TMPro;

[CustomEditor(typeof(Initializer))]
public class InitializerEditor : Editor
{
    private static readonly Type[] interactorsType = new Type[]
    {
        typeof(toColorInteractor),
        typeof(toWeighInteractor),
        typeof(doubleHeightInteractor),
        typeof(doubleWidthInteractor),
        typeof(widthDivisorInteractor),
        typeof(heightDivisorInteractor),
        typeof(toColorText),
        typeof(toColorBackground),
        typeof(penInteractor),
        typeof(cabinInteractor),
        typeof(LiquidDensityInteractor),
        typeof(PlaceableSurface),
        typeof(ObjectInteractor)
    };
    private static string[] interactors;

    //to add special component that requires more action than to only be added (se methods below) 
    private static readonly Type[] specialInteractable = new Type[]
    {
        typeof(TextMeshPro),
        typeof(FerrisWheelManager)
    };

    private void OnEnable()
    {
        interactors = Array.ConvertAll(interactorsType, t => t.Name);
    }

    static FerrisWheelManager fwmRef;
    static TextMeshPro testo = null; //new
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Initializer script = (Initializer)target;

        //its possibile to disable options depending on interactors and interactables but it would be to complex growing the posibilities, the artist that will build the scene could have the brain to not mix things

        if (fwmRef == null)
        {   //should be useful create a way to use a for/foreach but triyng to do it we could rewrite a lot of code
            DrawToggleComponent(script.gameObject, out bgColorable bgc, onAdd: mr => Debug.Log("bgColorable added"), onRemove: ma => Debug.Log("bgColorable removed"));
            DrawToggleComponent(script.gameObject, out weighable w, onAdd: mr => Debug.Log("weighable added"), onRemove: ma => Debug.Log("weighable removed"));
            DrawToggleComponent(script.gameObject, out Doubled dd, onAdd: mr => Debug.Log("Doubled added"), onRemove: ma => Debug.Log("Doubled removed"));
            DrawToggleComponent(script.gameObject, out Divisible dv, onAdd: mr => Debug.Log("Divisible added"), onRemove: ma => Debug.Log("Divisible removed"));

            DrawToggleComponent(script.gameObject, out specificBgColorable sbc, onAdd: mr => Debug.Log("specificBgColorable added"), onRemove: ma => Debug.Log("specificBgColorable removed"));
            DrawToggleComponent(script.gameObject, out penWritable pw, onAdd: mr => Debug.Log("penWritable added"), onRemove: ma => Debug.Log("penWritable removed"));

            DrawToggleComponent(script.gameObject, out CabinPositionable cp, onAdd: mr => Debug.Log("penWritable added"), onRemove: ma => Debug.Log("penWritable removed"));

            DrawToggleComponent(script.gameObject, out LiquidDensityInteractable ldi, onAdd: mr => Debug.Log("LiquidDensityInteractable added"), onRemove: ma => Debug.Log("LiquidDensityInteractable removed"));

            DrawToggleComponent(script.gameObject, out DragObject dg, onAdd: mr => Debug.Log("dragObject added"), onRemove: ma => Debug.Log("dragObject removed"));

            DrawToggleComponent(script.gameObject, out PositionableObject po, onAdd: mr => Debug.Log("PositionableObject added"), onRemove: ma => Debug.Log("PositionableObject removed"));

            DrawToggleComponent(script.gameObject, out TextMeshPro txt, onAdd: mr => Debug.Log("TextMeshPro added"), onRemove: ma => Debug.Log("TextMeshPro removed"));

            if (testo != null)
                DrawToggleComponent(script.gameObject, out textColorable tc, onAdd: mr => Debug.Log("textColorable added"), onRemove: ma => Debug.Log("textColorable removed"));
        }

        //is not convenient use DrawToggleComponent for special specific components, because you always need to add adding and dertroying if and relatives methods -> gives a standard
        if(script.GetComponents(typeof(Component)).Length <= 4 && !specialChecker(script.gameObject, typeof(TextMeshPro))) //4 is the maximum required component for the Ferris Wheel Base Object, text add a child, so this if should be updated in case of pother special components in use
        {
            DrawToggleComponent(script.gameObject, out FerrisWheelManager fwm, onAdd: mr => Debug.Log("FerrisWheelManager added"), onRemove: ma => Debug.Log("FerrisWheelManager removed"));
        }        

        DrawComponentsPopup(script.gameObject, interactors, interactorsType, "Interactor");
    }

    private bool DrawToggleComponent<T>(GameObject targetObject, out T component, string label = null, Action<T> onAdd = null, Action<T> onRemove = null)
        where T : Component
    {
        bool hadComponent = targetObject.TryGetComponent(out component);
        if (specialInteractable.Contains(typeof(T)))  //if it is a special component, needs an appropriate verification
            hadComponent = specialChecker(targetObject, typeof(T));

        bool hasComponent = hadComponent;
        EditorGUI.BeginChangeCheck();
        {
            hasComponent = GUILayout.Toggle(hadComponent, label ?? typeof(T).Name);
            if (EditorGUI.EndChangeCheck())
            {
                if (hasComponent && !hadComponent)
                {
                    if (specialInteractable.Contains(typeof(T)))
                    {
                        specialComponentAddiction(targetObject, typeof(T));
                    }
                    else 
                        component = targetObject.AddComponent<T>();

                    onAdd?.Invoke(component);
                }
                else if (!hasComponent && hadComponent)
                {
                    onRemove?.Invoke(component);
                    if (specialInteractable.Contains(typeof(T)))
                    {
                        specialComponentDeletion(targetObject, typeof(T));
                    }
                    else
                        DestroyImmediate(component);
                }
            }
        }
        return hasComponent;
    }

    //adding special components (interactables)
    void specialComponentAddiction(GameObject targetObject, Type t) //add an if to this code and the relative method to add the component
    {
        if (t == typeof(FerrisWheelManager))
            fwmCreator(targetObject);
        else if (t == typeof(TextMeshPro))
            textCreator(targetObject);
    }

    void specialComponentDeletion(GameObject targetObject, Type t)  //add an if to this code and the relative method to delete the component
    {
        if (t == typeof(FerrisWheelManager))
            fwmDestroyer(targetObject);
        else if (t == typeof(TextMeshPro))
            textDestroyer(targetObject);
    }

    bool specialChecker(GameObject targetObject, Type t) //add an if to this code and the relative method to check if is already present or not
    {
        if (t == typeof(FerrisWheelManager))
            return fwmChecker(targetObject);
        else if(t == typeof(TextMeshPro))
            return textChecker(targetObject);

        else return false;
    }

    void fwmCreator(GameObject targetObject) {
        targetObject.name = "FerrisWheelBase";
        if(targetObject.GetComponent<SpriteRenderer>() == null)
            targetObject.AddComponent<SpriteRenderer>();

        GameObject sequence = new GameObject();
        sequence.AddComponent<SpriteRenderer>();
        sequence.name = "SequenceBoard";
        sequence.transform.parent = targetObject.transform;
        sequence.AddComponent<SetBoardInSpace>();

        GameObject shadow = new GameObject();
        shadow.AddComponent<SpriteRenderer>();
        shadow.name = "Shadow";
        shadow.transform.parent = targetObject.transform; //with default sprites by Marshmallow Games -> + (0,-0.07,0.1) and pivot bottom centered in (0.486,0)
        shadow.AddComponent<setShadowDepth>();

        GameObject wheelStruct = new GameObject();
        wheelStruct.AddComponent<SpriteRenderer>();
        fwmRef = wheelStruct.AddComponent<FerrisWheelManager>();
        wheelStruct.name = "wheelStruct";
        //wheelStruct.transform.parent = targetObject.transform; //for a better implementation -> to do in future
        wheelStruct.transform.parent = sequence.transform;
    }

    void fwmDestroyer(GameObject targetObject)
    {
        targetObject.name = "DefaultObject";
        foreach (Transform child in targetObject.transform)
        {
            DestroyImmediate(child.gameObject);
        }

        fwmRef = null;
    }

    bool fwmChecker(GameObject targetObject)
    {
        if (targetObject.transform.childCount>0 && targetObject.transform.GetChild(0).transform.childCount > 0 && targetObject.transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<FerrisWheelManager>() != null)
            return true;
        else
            return false;
    }

    void textCreator(GameObject targetObject)
    {
        GameObject go = new GameObject("testo");
        TextMeshPro component = go.AddComponent<TextMeshPro>();
        go.transform.SetParent(targetObject.transform);
        component.rectTransform.localPosition = new Vector3(0, 0.5f, 0);
        component.rectTransform.sizeDelta = new Vector2(1, 1);
        component.rectTransform.localScale = new Vector3(1, 1, 1);
        component.color = Color.black;
        component.alignment = TextAlignmentOptions.Center;
        component.enableAutoSizing = true;
        component.fontSizeMin = 1;
        component.fontSizeMax = 18;
        testo = component;
    }

    void textDestroyer(GameObject targetObject)
    {
        DestroyImmediate(targetObject.transform.GetChild(0).gameObject);
        testo = null;
        textColorable tc = targetObject.GetComponent<textColorable>();

        //removing dependencies with text -> add other removes if needed 
        if ( tc != null)
            DestroyImmediate(tc);

    }

    bool textChecker(GameObject targetObject)
    {
        TextMeshPro component = targetObject.gameObject.GetComponentInChildren<TMPro.TextMeshPro>();
        return component != null ? true : false;
    }
    //ending special components (interactables)

    //interactors

    private void DrawComponentsPopup(GameObject targetObject, string[] options, Type[] types, string label = "Component")
    {
        EditorGUI.BeginChangeCheck();
        {
            int oldTypeIndex = GetExistingComponentIndex(targetObject, out Component component, types);
            
            int typeIndex = EditorGUILayout.Popup(label, oldTypeIndex, options); //because could exist object in scene that doesn't interact, then the default value is empty
            if (EditorGUI.EndChangeCheck())
            {
                if (oldTypeIndex >= 0)
                    DestroyImmediate(component,true);
                if (typeIndex >= 0)
                    targetObject.AddComponent(types[typeIndex]); //dovrei aggiungere cabin manager se metto cabin interactor
            }
        }
    }

    private int GetExistingComponentIndex(GameObject targetObject, out Component component, Type[] types)
    {
        Component c = null;
        int index = Array.FindIndex(types, t => targetObject.TryGetComponent(t, out c));
        component = c;
        return index;
    }
}