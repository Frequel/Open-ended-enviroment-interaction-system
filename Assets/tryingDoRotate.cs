using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class tryingDoRotate : MonoBehaviour
{
    [SerializeField]
    [Range(0,1)]
    int num;

    Vector3 rotationVector = new Vector3(0, 0, 1);
    Vector3 Hx2 = new Vector3(0, 0.5f, 0);
    // Start is called before the first frame update
    private void Start()
    {
        //StartCoroutine(RotateCube());
        //transform.DORotate(new Vector3(0, 0, 360), 100, RotateMode.LocalAxisAdd);
        //transform.DOLocalRotate(new Vector3(0, 0, 360), 10, RotateMode.LocalAxisAdd);

        //Sequence run = DOTween.Sequence();
        //Tween rot = this.transform.DORotate(new Vector3(0, 0, 360), 10, RotateMode.FastBeyond360).SetEase(Ease.Linear);
        //run.Append(rot).SetLoops(-1);

        //if(num==0)
        //    transform.DORotate(new Vector3(0, 0, 360), 10, RotateMode.LocalAxisAdd);
        //else if (num == 1)
        //{
        //    foreach (Transform child in transform)
        //    {
        //        child.GetComponent<CabinManager>().correctSequenceRotation();
        //    }
        //}
            
    }

    public IEnumerator RotateCube()
    {
        yield return new WaitForSeconds(1);

        transform.DORotate(new Vector3(-6, 792756, 360), 1, RotateMode.FastBeyond360);

        yield return new WaitForSeconds(1);

        transform.DORotate(
            new Vector3(0, 0, 0), 1, RotateMode.FastBeyond360);
    }


}
