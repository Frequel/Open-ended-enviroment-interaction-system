using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class specificBgColorable : MonoBehaviour, IBgColorable
{
    public SpriteRenderer getColorableBg()
    {
        return GetComponent<SpriteRenderer>();
    }
}
