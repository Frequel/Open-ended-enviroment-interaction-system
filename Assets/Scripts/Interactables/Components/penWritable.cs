using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class penWritable : MonoBehaviour, IWriteable
{
    [SerializeField]
    public GameObject foglioScritto;
    public void write(GameObject pen)
    {
        GameObject fsCopy = Instantiate(foglioScritto, transform.position, Quaternion.identity);
        fsCopy.GetComponent<SpriteRenderer>().color = gameObject.GetComponent<SpriteRenderer>().color;
        TextMeshPro bcText = fsCopy.GetComponentInChildren<TMPro.TextMeshPro>();
        bcText.color = pen.GetComponent<SpriteRenderer>().color;
        Destroy(pen);
        Destroy(gameObject);
    }
}
