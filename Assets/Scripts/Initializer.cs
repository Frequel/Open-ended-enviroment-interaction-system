using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Initializer : MonoBehaviour
{
    public void Reset()
    {
        BoxCollider source = GetComponent<BoxCollider>();

        if (source == null)
        {
            #if UNITY_EDITOR
            if (UnityEditor.EditorUtility.DisplayDialog("Choose a Component", "You are missing one of the required componets. Please choose one to add", "BoxCollider"))//, "SphereCollider"))
            {
                gameObject.AddComponent<BoxCollider>();
            }
            #endif
        }
    }

}
