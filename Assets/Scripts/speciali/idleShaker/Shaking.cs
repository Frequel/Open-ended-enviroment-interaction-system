using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Shaking : MonoBehaviour
{

    float rotationDuration = 0.5f;
    //float pulseDuration = 0.2f;
    float delayBetweenFeedbacks = 5f;
    float rotationGrade = 15f;
    //Ease pulseEase = Ease.OutQuad;
    Ease rotationEase = Ease.OutQuad;
    Sequence sequence;

    //Vector3 shakeStrenght = originalPosition - transform.localPosition;
    //shakeStrenght = (shakeStrenght / shakeStrenghtDivisor);
    //public static float shakeStrenghtDivisor = 30.0f;
    //Vector3 shakeStrenght;

    public static float shakeStrenghtDivisor = 30.0f;

    Quaternion originalRotation;

    //private float nextActionTime = 0.0f;
    public float period = 0.1f;
    float timer = 0.0f;
    private float waitTime = 3.0f;
    bool shakera = true;

    Collider coll;
    GameManager gm;
    shakingManager sm;

    // Start is called before the first frame update
    void Start()
    {
        originalRotation = transform.localRotation;
        coll = GetComponent<Collider>();
        gm = GameManager.GetInstance;
        sm = shakingManager.GetInstance;

        if (sm == null)
            Debug.Log("Shaking Manager is missing. Please add it from the gameobject provided by the framework");

        //shakeStrenght = transform.position - Vector3.up;
        //shakeStrenght = shakeStrenght / shakeStrenghtDivisor;
        sm.ShakeAll += IdleBehaviour;
        //iscrizione a evento contrario
        sm.StopShakeAll += StopShake;
    }

    void StopShake()
    {
        sequence.Kill();
    }
    // Update is called once per frame
    //void Update()
    //{
    //    //IdleBehaviour();

    //    //if (Time.time > nextActionTime)
    //    //{
    //    //    nextActionTime += period;
    //    //    // execute block of code here
    //    //    sequence.Kill();
    //    //}

    //    gm.IdleTimer += Time.deltaTime;

    //    if (gm.IdleTimer > gm.IdleTime)
    //    {
    //        timer += Time.deltaTime;

    //        // Check if we have reached beyond 2 seconds.
    //        // Subtracting two is more accurate over time than resetting to zero.
    //        //if (timer == 0)

    //        if (shakera & gm.IdleState)
    //        {
    //            IdleBehaviour();
    //            //Shake(transform, shakeStrenght);
    //            shakera = false;
    //            //gm.IdleState = false;
    //        }
    //        else if (timer > waitTime & gm.IdleState)
    //        {
    //            timer = 0;
    //            sequence.Kill();
    //            shakera = true;
    //            gm.IdleState = true;
    //        }
    //    }
    //}
    void IdleBehaviour()
    {
        sequence = DOTween.Sequence();
        sequence.Append(gameObject.transform.DOLocalRotate(new Vector3(0, coll.bounds.size.y / 2, originalRotation.z + rotationGrade), rotationDuration)).SetEase(rotationEase);
        sequence.Append(gameObject.transform.DOLocalRotate(new Vector3(0, coll.bounds.size.y / 2, originalRotation.z - rotationGrade), rotationDuration)).SetEase(rotationEase);
        sequence.Append(gameObject.transform.DOLocalRotate(new Vector3(0, coll.bounds.size.y / 2, originalRotation.z + rotationGrade), rotationDuration)).SetEase(rotationEase);
        sequence.Append(gameObject.transform.DOLocalRotate(new Vector3(0, coll.bounds.size.y / 2, originalRotation.z), rotationDuration)).SetEase(rotationEase);
        sequence.SetEase(rotationEase);
        sequence.AppendInterval(delayBetweenFeedbacks);
        //sequence.SetLoops(-1); //non mi serve se chiamo sta funzione ogni tot secondi *vedi shakerManager), però potrei trovare un'altra solutione e cioè che lascio il loop e che lo killo se premo uno di loro, quindi nel SM ci và un controllo non sul tempo ma su una variabile che gli dice di lanciare StopALL e questa variabile và settata nell'OnMouse di questo script
        sequence.Play();
    }

    ///// <summary>
    ///// Performs a shake (4 movements) on a given object
    ///// </summary>
    ///// <param name=“target”>transform of the target object</param>
    ///// <param name=“strenght”>strength (and direction) of the shake</param>
    ///// <param name=“onComplete”>delegate called at the end of the animation</param>
    //private void Shake(Transform target, Vector3 strenght, System.Action onComplete = null)
    //{
    //    target.DOMove(target.position + strenght, 0.1f).SetEase(Ease.OutCubic).OnComplete(delegate
    //    {
    //        Vector3 secondMove = new Vector3(strenght.x / 2, strenght.y / 2, 0.0f);
    //        target.DOMove(target.position - strenght - secondMove, 0.1f).SetEase(Ease.InOutCubic).OnComplete(delegate
    //        {
    //            Vector3 thirdMove = new Vector3(secondMove.x / 2, secondMove.y / 2, 0.0f);
    //            target.DOMove(target.position + secondMove + thirdMove, 0.1f).SetEase(Ease.InOutCubic).OnComplete(delegate
    //            {
    //                target.DOMove(target.position - thirdMove, 0.1f).SetEase(Ease.InOutCubic).OnComplete(delegate { onComplete?.Invoke(); });
    //            });
    //        });
    //    });
    //}

    private void OnMouseDown()
    {
        gm.IdleState = false;
        gm.IdleTimer = 0;
    }

    private void OnMouseUp()
    {
        gm.IdleState = true;
    }
}
