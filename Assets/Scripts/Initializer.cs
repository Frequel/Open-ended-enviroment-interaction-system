using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(setPositionInSpace))]
public class Initializer : MonoBehaviour
{

    //    public void OnEnable()
    //    {
    //        BoxCollider coll = GetComponent<BoxCollider>();

    //        if (coll == null)
    //        {
    //#if UNITY_EDITOR
    //            if (UnityEditor.EditorUtility.DisplayDialog("Choose a Component", "You are missing one of the required componets. Please choose one to add", "BoxCollider"))//, "SphereCollider"))
    //            {
    //                gameObject.AddComponent<BoxCollider>();
    //            }
    //#endif
    //        }

    //        setPositionInSpace sPiS = GetComponent<setPositionInSpace>();

    //        if (sPiS == null)
    //        {
    //#if UNITY_EDITOR
    //            if (UnityEditor.EditorUtility.DisplayDialog("Choose a Component", "You are missing one of the required componets. Please choose one to add", "BoxCollider"))//, "SphereCollider"))
    //            {
    //                gameObject.AddComponent<setPositionInSpace>();
    //            }
    //#endif
    //        }
    //    }

    //    public void Reset()
    //    {
    //        BoxCollider coll = GetComponent<BoxCollider>();

    //        if (coll == null)
    //        {
    //#if UNITY_EDITOR
    //            if (UnityEditor.EditorUtility.DisplayDialog("Choose a Component", "You are missing one of the required componets. Please choose one to add", "BoxCollider"))//, "SphereCollider"))
    //            {
    //                gameObject.AddComponent<BoxCollider>();
    //            }
    //#endif
    //        }

    //        setPositionInSpace sPiS = GetComponent<setPositionInSpace>();

    //        if (sPiS == null)
    //        {
    //#if UNITY_EDITOR
    //            if (UnityEditor.EditorUtility.DisplayDialog("Choose a Component", "You are missing one of the required componets. Please choose one to add", "BoxCollider"))//, "SphereCollider"))
    //            {
    //                gameObject.AddComponent<setPositionInSpace>();
    //            }
    //#endif
    //        }
    //    }

}
