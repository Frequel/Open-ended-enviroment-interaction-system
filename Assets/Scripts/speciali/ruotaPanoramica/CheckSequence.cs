using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckSequence : MonoBehaviour
{
    SpriteRenderer m_SpriteRenderer; //ma mi serve successivamente? sennò non me lo salvo e lo metto solo nello start
    Sprite[] spriteArray;
    Sprite centralSprite;

    //per come vedo io il funzionamento della cabina, direi che per il centro, creerò tanti prefab quante le sprite delle sequenze da riprodurre e che il centro ogni volta che resetto switcha ad una randomica di queste prefab. ognuno dis ti rpefab è ad-hoc per quella sequenza di sprite e quindi setterà sti due paramentri qui serializzabili al loro interno e questo script recupererà quei parametri. questo script sarà attaccato a quei prefab o sarà sempre aggiunto qualora dall'èditor si selezioni la ruota panoramica
    //magari per rendere compatibile questo script con quelli dei prefab che settano tali paramentri, utilizzerò un'interfaccia perchè così generalizzo che componente devono avere e gli costringo ad avere certi metodi
    //[SerializeField]
    //int numeroCabine;
    //[SerializeField]
    //int numeroSequenza;

    //ci vorrebbe un serialized field di enumerator che non ricordo come si fà.... -> così seleziono solo la sequenza, tipo rosso giallo verde, che sarebbe quella iniziale, in quanto poi la faccio random usando i numeri direttamente
    //[SerializeField]
    //[Range(0, 5)] //bib posso selezionare cose fuori range -> se aggiungo sprite disponibili => aumento il range a destra
    //int[] indiceSpritePerSequenza;// = new int[numeroSequenza]; //ci vorrebbe pure una maniera per farlo grande quanto numeroSequenza


    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        //centralSprite = GetComponent<SpriteRenderer>().sprite;
        spriteArray = Resources.LoadAll<Sprite>("Sprites/Cabine");
        //scegliere una sprite random tramite uno switch generico che dato un numero mette una determinata sprite -> per come ho impostato adesso, non serve
        //la seguente logica la spostiamo in diversi script che si differenziano per la sequenza da mettere, tipo blu_Giallo -> la roba randomica si potrebbe usare per decidere uno script diverso, ma non saprei come fare
        //TUTTA LA DINAMICA DI SCELTA E RANDOMIZZAZIONE DELLA SPRITE CENTRALE DIPENDE DAL GAME DESIGN... 
        int i = Random.Range(0, 5);
        m_SpriteRenderer.sprite = spriteArray[i];
        centralSprite = spriteArray[i];
    }

    void changeSprite(int i)
    {
        switch (i)
        {
            case 0:
                m_SpriteRenderer.sprite = spriteArray[i];
                break;
            case 1:
                m_SpriteRenderer.color = Color.blue;
                break;
            case 2:
                m_SpriteRenderer.color = Color.cyan;
                break;
            case 3:
                m_SpriteRenderer.color = Color.green;
                break;
            case 4:
                m_SpriteRenderer.color = Color.magenta;
                break;
            case 5:
                m_SpriteRenderer.color = Color.yellow;
                break;
        }
    }

    void startRotation() //aggiungere il fatto che si disabilita il change cabin finche non si ferma la ruota e si resetta tutto
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<RotateCabin>().correctSequenceRotation();
        }
    }

    //non funziona con 1 -> cioè tutte uguali -> da testare

    public void checkSequenceNew() //funziona ed è generalizzato per dove inizia la sequenza -> da testare con sequenza più lunga -> testato, funziona -> da modificare per funzionare sia in senso orario che anti-orario
    {
        int k;
        bool ok = true;// false;
        FerrisWheelManager by = GetComponent<FerrisWheelManager>(); //posso farlo anche solo allo start, no?
        int check = by.numeroCabine / by.numeroSequenza; //8 //2
        
        LinkedList<SpriteRenderer> childrens = new LinkedList<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>());
        

        childrens.RemoveFirst(); //il primo era il padre stesso

        //permette di iniziare la sequenza da qualsiasi punto
        for (k = 0; k < by.numeroSequenza; k++)
        {
            ok = nonSoComeChiamartiNew(ok, check, childrens, by);

            if (ok)
                break;

            SpriteRenderer tmp = childrens.ElementAt(0);
            childrens.RemoveFirst();
            childrens.AddLast(tmp);
        }

        Debug.Log("sono il metodo con le liste");
        if (ok)
            startRotation();
    }
    bool nonSoComeChiamartiNew(bool ok, int check, LinkedList<SpriteRenderer> childrens, FerrisWheelManager by)
    {
        int i = 0, j = 0;//, k = 0;
        for (i = 0; i < by.numeroSequenza; i++) //si salva anche se stesso (parent) non solo i figli per questo ci sta il +1, perchè sta come primo elemento -> ora rimosso, quindi parto da 0
        {
            if (childrens.ElementAt(i).sprite == by.spriteSequence[i]) //si salva anche se stesso (parent) non solo i figli per questo o metto +1 al childrens o -1 alla sequenza, perchè sta come primo elemento  //aspectedSeq[i]) //spriteArray
                                                                       //in teoria, non sò secondo il giocatore qual è la cabina iniziale e quella finale, quindi potrebbe fare che i colori siano traslati in base al suo punto di vista, questo sarebbe complesso da trattare...
            {
                for (j = 1; j <= check - 1; j++) //-1 perchè sè stesso già è comparato
                {
                    if (childrens.ElementAt(i).sprite != childrens.ElementAt(i + j * by.numeroSequenza).sprite)
                        return false;
                }
            }
            else
            {
                return false;
            }

        }

        return true;
    }
}