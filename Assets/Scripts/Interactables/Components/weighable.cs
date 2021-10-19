using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weighable : MonoBehaviour, IWeighable
{
    [SerializeField]
    float weight = 2f;

    public float getWeight()
    {
        return weight;
    }
}