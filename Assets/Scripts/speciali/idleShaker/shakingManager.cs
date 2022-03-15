using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shakingManager : MonoBehaviour
{
    public float period = 0.1f;
    float timer = 0.0f;
    private float waitTime = 3.0f;
    bool shakera = true;

    Collider coll;
    GameManager gm;

    public delegate void Shaker();
    public event Shaker ShakeAll;

    public delegate void StopShaker();
    public event StopShaker StopShakeAll;

    public static shakingManager sm;

    public static shakingManager GetInstance
    {
        get { return sm; }
    }

    void Awake()
    {
        if (sm == null)
        {
            sm = this;
        }
        else
        {
            Debug.Log("Warning: multiple " + this + " in scene!");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.GetInstance;
    }

    void Update()
    {
        if (gm.IdleState)
        {
            gm.IdleTimer += Time.deltaTime;

            if (gm.IdleTimer > gm.IdleTime)
            {
                timer += Time.deltaTime;

                if (shakera & gm.IdleState)
                {
                    //IdleBehaviour(); //-evento
                    ShakeAll();
                    //Shake(transform, shakeStrenght);
                    shakera = false;
                    //gm.IdleState = false;
                }
                else if (timer > waitTime & gm.IdleState)
                {
                    timer = 0;
                    //sequence.Kill(); il contrario di ShakeAll qualche evento al contrario (?)
                    StopShakeAll();
                    shakera = true;
                    gm.IdleState = true;
                }
            }
        }
    }
}
