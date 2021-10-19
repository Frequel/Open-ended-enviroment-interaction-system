using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
public class PulseEffect : MonoBehaviour
{
    //Pulse Prize Effect Parameters
    private float speed = 1f;
    Vector3 originalSize;
    private Vector3 pulseSize;
    Sequence pulseSequence;

    public void StartTween()
    {
        originalSize = transform.localScale;
        pulseSize = originalSize * 1.15f;
        pulseSequence = DOTween.Sequence();
        pulseSequence.Append(transform.DOScale(pulseSize, speed));
        pulseSequence.Append(transform.DOScale(originalSize, speed));
        pulseSequence.Play().SetLoops(-1);
    }
    public void StopSequence()
    {
        pulseSequence.Kill(); //invece di killare e basta sarebbe bello da vedere che ritorna alla sua dimensione originale gradualmente come durante il Tween
        transform.localScale = originalSize;
    }
}