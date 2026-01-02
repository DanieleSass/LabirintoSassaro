using UnityEngine;

public class CameraClipping : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] LayerMask ostacoliLayer;
    [SerializeField] Camera terzaPersona;
    [SerializeField] Camera primaPersona;

    Vector3 offsetLocale;

    public bool inClipping { get; set; }

    bool eraInTerzaPersonaPrimaDelClipping;

    public bool primaPersonaForzata {  get; set; }

    void Start()
    {
        offsetLocale = transform.localPosition;
    }

    void Update()
    {
        //Vector3 targetPos = player.TransformPoint(offsetLocale);

        //if (Physics.Linecast(player.position, targetPos, out RaycastHit hit))
        //{
        //    Vector3 dir = (targetPos - player.position).normalized;
        //    transform.position = hit.point - dir * 0.2f;
        //    //targetPos = hit.point - dir * 0.2f;
        //}
        //else
        //{
        //    // torna rapidamente alla posizione ideale
        //    transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 15f);
        //}

        //// clipping = c’è un ostacolo tra player e posizione ideale   
        //inClipping = Physics.Linecast(player.position, targetPos);

        //Debug.Log("inClipping = " + inClipping);



        Vector3 targetPos = player.TransformPoint(offsetLocale);
        //if (Physics.Linecast(player.position, targetPos, out RaycastHit hit))
        //{
        //    float distanza = Vector3.Distance(hit.point, player.position);
        //    inClipping = distanza < 1;
        //}
        //else
        //    inClipping = false;

        //Debug.Log("clipping" + inClipping);
        float distanza;
        if (Physics.Linecast(player.position, targetPos, out RaycastHit hit, ostacoliLayer))
        {
            //Debug.Log("Ostacolo rilevato: " + hit.collider.name);
            distanza = Vector3.Distance(hit.point, player.position);


            if (distanza < 2)
            {

                primaPersonaForzata = true;
                //eraInTerzaPersonaPrimaDelClipping = true;

                ////terzaPersona.gameObject.SetActive(false);
                ////primaPersona.gameObject.SetActive(true);

                //terzaPersona.enabled = false;
                //primaPersona.enabled = true;

                return;
            }
            //else
            //{
            //    Debug.Log("Riattiva 3p");
            //    if (eraInTerzaPersonaPrimaDelClipping)
            //    {
            //        eraInTerzaPersonaPrimaDelClipping = false;
            //        primaPersona.gameObject.SetActive(false);
            //        terzaPersona.gameObject.SetActive(true);
            //    }
            //}


        }
        else
        {
            Debug.Log("Riattiva 3p");
            primaPersonaForzata = false;
            ////if (eraInTerzaPersonaPrimaDelClipping)
            ////{
            //    eraInTerzaPersonaPrimaDelClipping = false;

            //primaPersona.enabled=false;
            //terzaPersona.enabled = true;
               // primaPersona.gameObject.SetActive(false);
                //terzaPersona.gameObject.SetActive(true);
            //}
        }
        //else
        //{
        //    terzaPersona.gameObject.SetActive(true);
        //    Debug.Log("Nessun ostacolo rilevato");
        //    //inClipping = false;

        //}


    }
}
