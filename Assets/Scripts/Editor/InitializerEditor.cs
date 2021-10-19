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
        typeof(toColorText), //questo ? un test per fare un oggetto con azione specifica, per fare le cose per bene sarebbe daimpedire di coesistere con altri (ma credo che già con il Popup riesco nell'intento)
        typeof(toColorBackground),
        typeof(penInteractor),
        typeof(ObjectInteractor)
    };
    private static string[] interactors;

    private static readonly Type[] dragType = new Type[]
    {
        typeof(DragObject),
        typeof(DragObjectWithText)
    };
    //private static string[] drag;

    private string[] drag;


    private void OnEnable()
    {
        interactors = Array.ConvertAll(interactorsType, t => t.Name);
        drag = Array.ConvertAll(dragType, t => t.Name);
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Initializer script = (Initializer)target;

        TextMeshPro testo = script.gameObject.GetComponentInChildren<TMPro.TextMeshPro>();

        DrawToggleComponent(script.gameObject, out bgColorable bgc, onAdd: mr => Debug.Log("bgColorable added"), onRemove: ma => Debug.Log("bgColorable removed"));
        DrawToggleComponent(script.gameObject, out weighable w, onAdd: mr => Debug.Log("weighable added"), onRemove: ma => Debug.Log("weighable removed"));
        DrawToggleComponent(script.gameObject, out Doubled dd, onAdd: mr => Debug.Log("Doubled added"), onRemove: ma => Debug.Log("Doubled removed"));
        DrawToggleComponent(script.gameObject, out Divisible dv, onAdd: mr => Debug.Log("Divisible added"), onRemove: ma => Debug.Log("Divisible removed"));

        if (testo != null)
            DrawToggleComponent(script.gameObject, out textColorable tc, onAdd: mr => Debug.Log("textColorable added"), onRemove: ma => Debug.Log("textColorable removed"));

        DrawToggleComponent(script.gameObject, out specificBgColorable sbc, onAdd: mr => Debug.Log("specificBgColorable added"), onRemove: ma => Debug.Log("specificBgColorable removed"));
        DrawToggleComponent(script.gameObject, out penWritable pw, onAdd: mr => Debug.Log("penWritable added"), onRemove: ma => Debug.Log("penWritable removed"));

        DrawComponentsPopup(script.gameObject, interactors, interactorsType, "Interacotr");
        DrawComponentsPopup(script.gameObject, drag, dragType, "Drag Component");

        //cosa carina sarebbe eliminare il secondo Popup di sopra, per crearne uno solo quando è disponibile il testo (ideale sarebbe mostrando un popup con unica opzione) -> si può pure fare che il popup proprio non c'è e crei objectDrag direttamente (magari se si riesce a scriverlo da qualche parte come se fosse un field sarebbe bello)

        //if (testo != null)
        //    DrawComponentsPopup(script.gameObject, drag, dragType, "Drag Component");
        //else
        //{
        //    Array.Resize(ref drag, drag.Length - 1);
        //}
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

    private void DrawComponentsPopup(GameObject targetObject, string[] options, Type[] types, string label = "Component")
    {
        EditorGUI.BeginChangeCheck();
        {
            int oldTypeIndex = GetExistingComponentIndex(targetObject, out Component component, types);
            int typeIndex = EditorGUILayout.Popup(label, oldTypeIndex, options);
            if (EditorGUI.EndChangeCheck())
            {
                if (oldTypeIndex >= 0)
                    DestroyImmediate(component);
                if (typeIndex >= 0)
                    targetObject.AddComponent(types[typeIndex]);
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