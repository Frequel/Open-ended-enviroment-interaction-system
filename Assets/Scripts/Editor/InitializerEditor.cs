using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
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
        typeof(ObjectInteractor)
    };
    private static string[] interactors;

    private void OnEnable()
    {
        interactors = Array.ConvertAll(interactorsType, t => t.Name);
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Initializer script = (Initializer)target;
        ///
        ///////////////
        //altra cosa da aggiungere potrebbe essere quella di costruire la ruota panoramica da 0, quindi mettere un toggle per ruota panoramica che disabiliti tutti gli altri toggle e viceversa quando usi uno degli altri toggle disabiliti la ruota panoramica
        //una cosa carina sarebbe disabilitare gli interactable se si selezionasse cabinInteractor, perch? non interagisce con nessuno se non con il personaggio che ci metti dentro
        /////////////////
        ///

        ///aggiungere che se metto cabin interactor aggiungo cabin manager se non c'è e sarebbe da aggiungere anche la possibilità di creare la ruota da editor al solo click
        ///

        TextMeshPro testo = null;

        //potrei fare un vect di interactable, tipo quello degli interactors e fare un for, no?

        DrawToggleComponent(script.gameObject, out bgColorable bgc, onAdd: mr => Debug.Log("bgColorable added"), onRemove: ma => Debug.Log("bgColorable removed"));
        DrawToggleComponent(script.gameObject, out weighable w, onAdd: mr => Debug.Log("weighable added"), onRemove: ma => Debug.Log("weighable removed"));
        DrawToggleComponent(script.gameObject, out Doubled dd, onAdd: mr => Debug.Log("Doubled added"), onRemove: ma => Debug.Log("Doubled removed"));
        DrawToggleComponent(script.gameObject, out Divisible dv, onAdd: mr => Debug.Log("Divisible added"), onRemove: ma => Debug.Log("Divisible removed"));

        DrawToggleComponent(script.gameObject, out specificBgColorable sbc, onAdd: mr => Debug.Log("specificBgColorable added"), onRemove: ma => Debug.Log("specificBgColorable removed"));
        DrawToggleComponent(script.gameObject, out penWritable pw, onAdd: mr => Debug.Log("penWritable added"), onRemove: ma => Debug.Log("penWritable removed"));

        DrawToggleComponent(script.gameObject, out CabinPositionable cp, onAdd: mr => Debug.Log("penWritable added"), onRemove: ma => Debug.Log("penWritable removed"));

        DrawToggleComponent(script.gameObject, out DragObject dg, onAdd: mr => Debug.Log("dragObject added"), onRemove: ma => Debug.Log("dragObject removed"));

        ///
        DrawToggleText(script.gameObject, out testo, onAdd: mr => Debug.Log("text added"), onRemove: ma => Debug.Log("text removed"));
        ///
        if (testo != null)
            DrawToggleComponent(script.gameObject, out textColorable tc, onAdd: mr => Debug.Log("textColorable added"), onRemove: ma => Debug.Log("textColorable removed"));

        DrawComponentsPopup(script.gameObject, interactors, interactorsType, "Interactor");
    }

    private bool DrawToggleComponent<T>(GameObject targetObject, out T component, string label = null, Action<T> onAdd = null, Action<T> onRemove = null)
        where T : Component
    {
        bool hadComponent = targetObject.TryGetComponent(out component);
        bool hasComponent = hadComponent;
        EditorGUI.BeginChangeCheck();
        {
            hasComponent = GUILayout.Toggle(hadComponent, label ?? typeof(T).Name);
            if (EditorGUI.EndChangeCheck())
            {
                if (hasComponent && !hadComponent)
                {
                    component = targetObject.AddComponent<T>();
                    onAdd?.Invoke(component);
                }
                else if (!hasComponent && hadComponent)
                {
                    onRemove?.Invoke(component);
                    DestroyImmediate(component);
                }
            }
        }
        return hasComponent;
    }

    private bool DrawToggleText(GameObject targetObject, out TextMeshPro component, string label = null, Action<TextMeshPro> onAdd = null, Action<TextMeshPro> onRemove = null)
    {
        component = targetObject.gameObject.GetComponentInChildren<TMPro.TextMeshPro>();
        bool hadComponent = component != null ? true : false;
        bool hasComponent = hadComponent;
        EditorGUI.BeginChangeCheck();
        {
            hasComponent = GUILayout.Toggle(hadComponent, label ?? typeof(TextMeshPro).Name);
            if (EditorGUI.EndChangeCheck())
            {
                if (hasComponent && !hadComponent)
                {
                    GameObject go = new GameObject("testo");
                    component = go.AddComponent<TextMeshPro>();
                    go.transform.SetParent(targetObject.transform);
                    component.rectTransform.localPosition = new Vector3(0, 0.5f, 0);
                    component.rectTransform.sizeDelta = new Vector2(1,1);
                    component.rectTransform.localScale = new Vector3(1, 1, 1);
                    component.color = Color.black;
                    component.alignment = TextAlignmentOptions.Center;
                    component.enableAutoSizing = true;
                    component.fontSizeMin = 1;
                    component.fontSizeMax = 18;
                }
                else if (!hasComponent && hadComponent)
                {
                    DestroyImmediate(targetObject.transform.GetChild(0).gameObject);  //ora s? che ci sta solo sto figlio o che comunque come primo figlio avr? il testo (per come ? adesso il codice) ma successivamente non lo s?, potrebbe esse utile iterare tra i figli e crecare child.name=="testo" https://answers.unity.com/questions/183649/how-to-find-a-child-gameobject-by-name.html -> il figlio si chiamerà testo di default
                }
            }
        }
        return hasComponent;
    }

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