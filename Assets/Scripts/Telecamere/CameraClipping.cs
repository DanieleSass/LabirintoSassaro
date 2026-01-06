using UnityEngine;

public class CameraClipping : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] LayerMask ostacoliLayer;   //in verità tutto è costruito in Default, però la cam può colpire solo i muri perchè non si possono fare acrobazie con le camere

    [SerializeField] float distanzaMinima = 1.2f;   //distanza minima consentita

    Vector3 offsetLocale;

    //public bool inClipping { get; private set; }
    //public bool primaPersonaForzata { get; set; }

    void Start()
    {
        offsetLocale = transform.localPosition;     //distanza player-cam ideale
    }

    void LateUpdate()   //late dopo update del player
    {
        //pos ideale, trasforma da posizione locale (offset di qualche unità in alto e indietro rispetto al player)
        //a vere e proprie coordinate del mondo di gioco, non più relative al player ma iindipendenti
        Vector3 posIdeale = player.TransformPoint(offsetLocale);

       //.normalized lo ridece a scala 1, cioè il valore massimo assunto può essere 1 e gli altri valori sono tra 0<=x<=1
        Vector3 direzione = (posIdeale - player.position).normalized;

        //prende la vera e propria distanza player-cam 
        float distanzaFinale = Vector3.Distance(player.position, posIdeale);

        //linecast controlla se c’è un ostacolo
        //se c'è qualcosa di layer ostacoliLayer(tutto) tra i primi 2 parametri, allora hit viene istanziata e valorizzato con info riguardo la collisione

        if (Physics.Linecast(player.position, posIdeale, out RaycastHit hit, ostacoliLayer))
        {
            //se da true allora allora la aggiusta per evitare collisioni
            //altimenti mantiene quella idelae

            //inClipping = true;

            float distanza = hit.distance;  //punto in cui ha colpito il muro

            //se troppo vicino, allora mette quella di degault per evitare che la cam possa entrare dentro al player per esempio
            if (distanza < distanzaMinima)
            {
                //primaPersonaForzata = true;
                distanza = distanzaMinima;
            }
            //else
            //{
            //    primaPersonaForzata = false;
            //}

            //avvicina la camera al punto di collisione leggermente in avanti per evitare che si veda il muro buggato
            distanzaFinale = distanza - 0.1f;   
        }
        //else
        //{
        //    inClipping = false;
        //    primaPersonaForzata = false;
        //}

        //calcola la nuova posizione finale
        Vector3 nuovaPos = player.position + direzione * distanzaFinale;

        //movimento morbido, il *10 fa da fattore che ammorbidisce e rende più "gentile" il movimento, lo amplifica
        transform.position = Vector3.Lerp(transform.position, nuovaPos, Time.deltaTime * 10);
    }
}


