using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class setPositionOnY : MonoBehaviour
{
    Vector3 overlapBoxDim; //me ne serve più di uno.
    Vector3 overlapBoxCen;

    //bool positioning = false;
    [System.NonSerialized]
    public bool TESTpositioning = false;

    positionType pt = positionType.defPos;
    public positionType Pt
    {
        get { return pt; }
        set { pt = value; }
    }

    LayerMask m_LayerMask;
    public LayerMask M_LayerMask
    {
        get { return m_LayerMask; }
        set { m_LayerMask = value; }
    }

    Collider coll;
    Vector3 size;

    PositionableObject po;

    float prova = -3.4f;

    GameManager gm;
    void Start()//set position in space based on the kind of position
    {
        gm = GameManager.GetInstance;

        coll = GetComponent<Collider>(); //potrei usare anche lo spriteRenderer
        size = coll.bounds.size;

        //e se cambia per qualche motivo la dimensione? tipo il mio test pre asset?
        overlapBoxDim = new Vector3(size.x, size.y, Camera.main.farClipPlane);
        overlapBoxCen = new Vector3(transform.position.x, transform.position.y + size.y / 2, Camera.main.farClipPlane / 2);// + transform.position.z);
        //

        prova = prova + 5 -3 +1 *2 /4;
        //Debug.Log(Mathf.Min(10f, 2.4f));//test

        po = GetComponent<PositionableObject>();

        m_LayerMask = ~8;


        //if (transform.parent != null)
        //{
        //    pt = positionType.childrenPos;
        //}
    }

    public void setPosition()//set position in space based on the kind of position
    //public IEnumerator setPosition()
    {
        //positioning = true;
        TESTpositioning = true;
        switch (pt)
        {
            case positionType.defPos:
                defaultPositioning();
                //yield return StartCoroutine(defaultPositioning());
                break;
            case positionType.draggingPos:
                break;
            case positionType.positionedPos:
                positionedPositioning();
                break;
            case positionType.dontMove:
                NoYMove();
                break;
            case positionType.childrenPos:
                positionedPositioning();
                break;
            default:
                defaultPositioning();
                break;
        }
        //if (transform.childCount > 0 && childrenPositioning != null)
        //    childrenPositioning(sprite);
        //positioning = false;

        //StartCoroutine(waitY());
        //  OLTRE ALLA Z DOVREI DISABILITARE IL DRAG FINO AL POSIZIONAMENTO DI Y

        //while(TESTpositioning == true)
        //    Debug.Log("waiting Y");
    }

    //private void defaultPositioning()
    //private async IEnumerator defaultPositioning()
    private async void defaultPositioning() //test
    {
        if (transform.position.y <= gm.MaxYavailable) //obj released on ground
        {            
            GameObject gOb = takeCollision();

            if (gOb != null)
            {
                if (po != null)
                {
                    PlaceableSurface plSur = takePlaceableSurface(gOb);
                    if (plSur != null)
                    {
                        float y;
                        //return false; //cade al lato sinistro
                        interactionResult ir = plSur.passiveInteractor(gameObject); //oppure movimento fino alla parte superiore del boxCollider poggiabile della superficie e figliamento. cose che comunque potrei mettere nell'interazione
                        //if (ir == interactionResult.occurred)
                        //{
                        //    y = plSur.Coll.bounds.max.y;
                        //}
                        //else
                        //{
                        //    //y = plSur.transform.position.y - 0.1f;
                        //    y = gm.MaxYavailable;
                        //}

                        //if (transform.position.y > plSur.Coll.bounds.max.y)
                        //{ //può esse che siamo sopra l'orizzonte perchè la superficie parte da sotto ma và oltre, quindi potrebbe esse che comunque la base sta sotto il max
                        //    //Tween myTween = transform.DOMoveY(plSur.Coll.bounds.max.y, 1, false).SetEase(Ease.OutBounce);
                        //    Tween myTween = transform.DOMoveY(y, 1, false).SetEase(Ease.OutBounce);
                        //    await myTween.AsyncWaitForCompletion();
                        //}
                        TESTpositioning = false;//test
                    }
                    else
                    {
                        //funzione che controlla ydrag e yfermo e chiama il verde o lascia la y invariata
                        checkBehind(gOb);
                        //se il check non rileva coperture, lascia la y invariata e serve mettere il false apposto
                        //TESTpositioning = false;//test //credo buggasse, perchè deve esse fatto dentro check behind
                    }
                }
                else
                {
                    //funzione che controlla ydrag e yfermo e chiama il verde o lascia la y invariata
                    checkBehind(gOb);
                    //se il check non rileva coperture, lascia la y invariata e serve mettere il false apposto
                    //TESTpositioning = false;//test //credo buggasse, perchè deve esse fatto dentro check behind
                }
            }

            //test
            else
            {
                TESTpositioning = false;//test
            }

        }
        else //obj released above ground
        {

            //checkPlaceableSufacebelow(); //overlapBox, itera sui collider fino a trovare placeablesSurface altrimenti ritorna l'ultimo collider
            //  if(plSur != null){
            //      //SI RIQUADRO BLU
            //      figliaConPoggiabile();
            //  }
            /// else
            /// {
            ///    //RIQUADRO VERDE COMUNE TRA QUESTO IF ENORME E IL PROSSIMO
            ///     
            ///
            /// }
            /// 
            //GameObject gOb = takeCollisionBelow();
            //if (gOb != null)
            Collider[] collBelow = checkCollisionBelow();
            if (collBelow != null)
            {
                if (po != null)
                {
                    PlaceableSurface plSur = takePlaceableSurfaceBelow(collBelow);
                    if (plSur != null)
                    {
                        float y;
                        //return false; //cade al lato sinistro
                        interactionResult ir = plSur.passiveInteractor(gameObject); //oppure movimento fino alla parte superiore del boxCollider poggiabile della superficie e figliamento. cose che comunque potrei mettere nell'interazione
                        if(ir == interactionResult.occurred)
                        {
                            y = plSur.Coll.bounds.max.y;
                        }
                        else
                        {
                            //y = plSur.transform.position.y - 0.1f;
                            y = gm.MaxYavailable;

                            //dovrei fare ricorsione su questa stessa funzione, in realtà
                            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            //non è detto che sia questa la destinazione, dipende se dopo che non c'è stata l'interazione ci sia il coverage (come sotto nell'else)
                            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            //collBelow = collBelow.ToList().Where(c => c.transform.position.y <= gm.MaxYavailable).OrderBy(c => c.transform.position.y).ToArray();
                            collBelow = checkCollisionBelow().ToList().Where(c => c.transform.position.y <= gm.MaxYavailable).OrderBy(c => c.transform.position.y).ToArray(); ;
                            if (collBelow.Length > 0)
                            {
                                checkCompleteCoverage(collBelow[0].gameObject);
                                return;
                            }
                        }

                        if (transform.position.y > plSur.Coll.bounds.max.y) { //può esse che siamo sopra l'orizzonte perchè la superficie parte da sotto ma và oltre, quindi potrebbe esse che comunque la base sta sotto il max
                            //Tween myTween = transform.DOMoveY(plSur.Coll.bounds.max.y, 1, false).SetEase(Ease.OutBounce);
                            Tween myTween = transform.DOMoveY(y, 1, false).SetEase(Ease.OutBounce);
                            await myTween.AsyncWaitForCompletion();
                        }
                        TESTpositioning = false;//test
                    }
                    else
                    {
                        //funzione che controlla ydrag e yfermo e chiama il verde o lascia la y invariata
                        //checkBehind(collBelow[0].gameObject);
                        collBelow = collBelow.ToList().Where(c => c.transform.position.y <= gm.MaxYavailable).OrderBy(c => c.transform.position.y).ToArray();
                        if (collBelow.Length > 0)
                            checkCompleteCoverage(collBelow[0].gameObject);
                        ///ci vuole modifica a questo metodo:
                        ///non posso usare il max, che dà la coordinata globale di dove si trova il mio max (utile). ma devo usare Y (=min tra la Ydrag e la Ymax) + sizeY
                        else
                        {
                            Tween myTween = transform.DOMoveY(gm.MaxYavailable, 1, false).SetEase(Ease.OutBounce);
                            await myTween.AsyncWaitForCompletion();
                        }

                    }
                }
                else
                {
                    //funzione che controlla ydrag e yfermo e chiama il verde o lascia la y invariata
                    //checkBehind(collBelow[0].gameObject);
                    //checkCompleteCoverage(collBelow[0].gameObject); //se nessuno ha il plSur ma ce ne stanno altri prima della Ymax? devo riordinare collBelow...
                    collBelow = collBelow.ToList().Where(c => c.transform.position.y <= gm.MaxYavailable).OrderBy(c => c.transform.position.y).ToArray();
                    if(collBelow.Length>0)
                        checkCompleteCoverage(collBelow[0].gameObject);
                    else
                    {
                        Tween myTween = transform.DOMoveY(gm.MaxYavailable, 1, false).SetEase(Ease.OutBounce);
                        await myTween.AsyncWaitForCompletion();
                    }
                }
            }
            else //qui necessario perchè comunque devo cambiargliela la Y
            {
                //transform.position = new Vector3(transform.position.x, gm.MaxYavailable, transform.position.z);
                /*
                 * DOMoveX/DOMoveY/DOMoveZ(float to, float duration, bool snapping)
                    Moves the target's position to the given value, tweening only the chosen axis.
                    snapping If TRUE the tween will smoothly snap all values to integers.

                .SetEase(easeOutBounce);//https://easings.net/#
                 * */

                //StartCoroutine(fallObjectAndWait());

                Tween myTween = transform.DOMoveY(gm.MaxYavailable, 1, false).SetEase(Ease.OutBounce);
                await myTween.AsyncWaitForCompletion();
                //yield return myTween.WaitForCompletion();

                TESTpositioning = false;//test
            }
        }
    }

    IEnumerator fallObjectAndWait()
    {

        while (true)
        {
            while (true)
            {
                yield return StartCoroutine(fallObject());
                break;
            }
            break;
        }

        TESTpositioning = false;
    }

    IEnumerator fallObject()
    {
        Tween myTween = transform.DOMoveY(gm.MaxYavailable, 1, false).SetEase(Ease.OutBounce);
        while (true)
        {
            while (true)
            {
                yield return myTween.WaitForCompletion();
                break;
            }
            break;
        }


    }

    private IEnumerator waitY()
    {
        while (true)
        {
            yield return new WaitUntil(() => TESTpositioning == false);
            break;
        }
    }

    private GameObject takeCollision()
    {
        //OrderByDescending(
        //forse è meglio se la collisione è tra il collider fermo e la metà inferiore di quello draggato??? -> non c'è però il pulse....
        overlapBoxCen = new Vector3(transform.position.x, transform.position.y + coll.bounds.size.y / 2, Camera.main.farClipPlane / 2 + Camera.main.transform.position.z);//transform.position.z); //è sbagliato, lo devo traslare della pos della camera, la quale sarebbe meglio salvarla nel GM e non fare sempre così che equivale ad un GetComponent//coll... per far fronte ai cambi di dimensione
        overlapBoxDim = new Vector3(coll.bounds.size.x, coll.bounds.size.y, Camera.main.farClipPlane); //idem a sopra
        Collider[] hitColliders = Physics.OverlapBox(overlapBoxCen, overlapBoxDim / 2, Quaternion.identity, m_LayerMask).OrderBy(c => c.transform.position.z).Where(c => c.transform.position.z > transform.position.z).ToArray();

        //collisione è tra il collider fermo e la metà inferiore di quello draggato -> non c'è però il pulse....
        //overlapBoxCen = new Vector3(transform.position.x, transform.position.y + coll.bounds.size.y / 4, Camera.main.farClipPlane / 2 + Camera.main.transform.position.z);//transform.position.z); //è sbagliato, lo devo traslare della pos della camera, la quale sarebbe meglio salvarla nel GM e non fare sempre così che equivale ad un GetComponent//coll... per far fronte ai cambi di dimensione
        //overlapBoxDim = new Vector3(coll.bounds.size.x, coll.bounds.size.y/2, Camera.main.farClipPlane); //idem a sopra
        //Collider[] hitColliders = Physics.OverlapBox(overlapBoxCen, overlapBoxDim / 2, Quaternion.identity, m_LayerMask).OrderBy(c => c.transform.position.z).Where(c => c.transform.position.z > transform.position.z).ToArray();

        return hitColliders.Length > 0 ? hitColliders[0].gameObject : null; //make an fi nulla etc..
    }

    private PlaceableSurface takePlaceableSurface(GameObject gOb)
    {
        PlaceableSurface ps = gOb.GetComponent<PlaceableSurface>();
        return ps;
            
    }

    private void checkBehind(GameObject gOb)
    {
        if (transform.position.y > gOb.transform.position.y) //solo in questo caso perchè se sta sotto dovrebbe avere la z e il sortting layer per essere davanti
        {
            checkCompleteCoverage(gOb);
        }
        else
        {
            TESTpositioning = false;
        }
    }

    private Collider[] checkCollisionBelow()
    {
        //OrderByDescending(

        //overlapBoxCen = new Vector3(transform.position.x, (transform.position.y - gm.MaxYavailable) / 2, Camera.main.farClipPlane / 2 + transform.position.z); //coll... per far fronte ai cambi di dimensione
        overlapBoxCen = new Vector3(transform.position.x, ((coll.bounds.max.y - gm.MaxYavailable) / 2) + gm.MaxYavailable, Camera.main.farClipPlane / 2 + Camera.main.transform.position.z);//se rilascio sopra/addosso un oggetto con BoxCollider, che parte però appena sopra la y del draggato
        //overlapBoxDim = new Vector3(coll.bounds.size.x, transform.position.y - gm.MaxYavailable, Camera.main.farClipPlane); //idem a sopra
        overlapBoxDim = new Vector3(coll.bounds.size.x, coll.bounds.max.y - gm.MaxYavailable, Camera.main.farClipPlane);//se rilascio sopra/addosso un oggetto con BoxCollider, che parte però appena sopra la y del draggato
        //idealmente, l'odine non dovrebbe esse sulla pos y, ma sulla bounds.max del collider...
        //Collider[] hitColliders = Physics.OverlapBox(overlapBoxCen, overlapBoxDim / 2, Quaternion.identity, m_LayerMask).OrderBy(c => c.transform.position.y).Where(c => c.transform.position.y < transform.position.y).ToArray();
        //Collider[] hitColliders = Physics.OverlapBox(overlapBoxCen, overlapBoxDim / 2, Quaternion.identity, m_LayerMask).OrderBy(c => c.bounds.max.y).Where(c => c.transform.position.y < transform.position.y).ToArray();
        Collider[] hitColliders = Physics.OverlapBox(overlapBoxCen, overlapBoxDim / 2, Quaternion.identity, m_LayerMask).OrderByDescending(c => c.bounds.max.y).Where(c => c.transform.position.y < transform.position.y).ToArray();
        //Where(c => c.transform.position.z > transform.position.z)

        return hitColliders.Length > 0 ? hitColliders : null; //make an if null a etc..
    }

    private PlaceableSurface takePlaceableSurfaceBelow(Collider[] collBelow)
    {
        //throw new NotImplementedException();

        foreach(Collider cB in collBelow)
        {
            PlaceableSurface ps = cB.GetComponent<PlaceableSurface>();
            if (ps != null)
                return ps;
        }

        return null;
    }

    //private void checkCompleteCoverage(GameObject gOb) //dovrei fare anche un check sulla larghezza... //-> anche degli offset di margine sarebbero adeguati.
    //private IEnumerator checkCompleteCoverage(GameObject gOb)
    private async void checkCompleteCoverage(GameObject gOb) //add also if is covered on all x size?
    {
        float y = Mathf.Min(transform.position.y, gm.MaxYavailable);
        //should use SpriteRenderer instead of BoxCollider because collider could be smaller than sprite
        //if (coll.bounds.max.y < gOb.GetComponent<BoxCollider>().bounds.max.y)//-> different from flowchart because i was not thinking about to get directly the heigher point of each object.//-y+gOb.transform.position.y) //if the height of the released object is completely covered by the height of the other object
        if (coll.bounds.size.y + y < gOb.GetComponent<BoxCollider>().bounds.max.y) //perchè per oggetti che vengono rilasciato sopra l'orizzonte non funzionerebbe mai
        {
            //transform.position = new Vector3(transform.position.x, gOb.transform.position.y - 0.1f, transform.position.z); //need an animation that left fall the objet till the new y. like DOMove
            /*
                 * DOMoveX/DOMoveY/DOMoveZ(float to, float duration, bool snapping)
                    Moves the target's position to the given value, tweening only the chosen axis.
                    snapping If TRUE the tween will smoothly snap all values to integers.

                    .SetEase(easeOutBounce);//https://easings.net/#
                 * */
            //StartCoroutine(fallObjectAndWait());

            Tween myTween = transform.DOMoveY(gOb.transform.position.y - 0.1f, 1, false).SetEase(Ease.OutBounce);
            await myTween.AsyncWaitForCompletion();
            //yield return myTween.WaitForCompletion();

            TESTpositioning = false;//test
        }
        else
        {
            //transform.position = new Vector3(transform.position.x, y, transform.position.z); //not the best solution, if drag is below YmaxAvailable should not change at all, this version is general to use this same function also with drag above Ymax //-> should be a movement like DOmove and not only a changing of a variable, for testing purpose i am using this way at the moment
            /*
                 * DOMoveX/DOMoveY/DOMoveZ(float to, float duration, bool snapping)
                    Moves the target's position to the given value, tweening only the chosen axis.
                    snapping If TRUE the tween will smoothly snap all values to integers.

                    .SetEase(easeOutBounce);//https://easings.net/#
                 * */

            //StartCoroutine(fallObjectAndWait());

            Tween myTween = transform.DOMoveY(y, 1, false).SetEase(Ease.OutBounce);
            await myTween.AsyncWaitForCompletion();
            //yield return myTween.WaitForCompletion();

            TESTpositioning = false;//test
        }
    }
    private void positionedPositioning()//da cambiare nome
    {
        TESTpositioning = false;//test
    }

    private void NoYMove()//da cambiare nome
    {
        TESTpositioning = false;//test
    }

    //void OnDrawGizmos()
    //{
    //    //Gizmos.color = Color.red;
    //    //Gizmos.DrawWireCube(overlapBoxCen, overlapBoxDim);

    //    //test dito
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireCube(overlapBoxCenDito, overlapBoxDimDito);

    //}

}

