using UnityEngine;

public class SwitchTelecamere : MonoBehaviour
{
    [SerializeField] Camera primaPersona;
    [SerializeField] Camera terzaPersona;

    [SerializeField] CameraClipping cameraClipping;
    [SerializeField] PlayerMovement player;

    bool inPrimaPersona;    //gestisce il cambio di prospettiva vero e proprio
    //bool sceltaUser;        //variabile di appoggio se è utente a voler cambiare prospettiva
    //inPrimaPesona invece viene cambiata anche in maniera forzata per cameraclipping


    float rotazione = 0f;   //da salvare

    void Start()
    {
        inPrimaPersona = true;
        //sceltaUser = true;

        //inPrimaPersonaForzata = false;

        //terzaPersona.Target.TrackingTarget = player.transform;
        AggiornaCamere();
    }

    void LateUpdate()
    {

        if (Input.GetKeyDown(KeyCode.F))    //al premere di F cambia inquadratura
        {
            inPrimaPersona = !inPrimaPersona;   //cambia stato
            AggiornaCamere();
            //AggiornaCamere();
        }

        //bool effettivaPrimaPersona = inPrimaPersona || cameraClipping.primaPersonaForzata;

        //primaPersona.enabled = effettivaPrimaPersona;
        //terzaPersona.enabled =!effettivaPrimaPersona;

        //if (cameraClipping.inClipping)
        //{
        //    terzaPersona.gameObject.SetActive(false);

        //    eraInTerzaPersonaPrimaDelClipping = !inPrimaPersona;

        //    inPrimaPersona = true;
        //    cameraClipping.inClipping = false;
        //    AggiornaCamere();

        //    eraInTerzaPersonaPrimaDelClipping = true;

        //    return;
        //}

        //terzaPersona.gameObject.SetActive(true);
        //if (eraInTerzaPersonaPrimaDelClipping)
        //{
        //    inPrimaPersona = false;
        //    eraInTerzaPersonaPrimaDelClipping = false;
        //    AggiornaCamere();
        //}

        //AggiornaCamere();


        //se è in 1persona allora può guardare più in su e in giu
        if (primaPersona)
        {
            float mouseY = Input.GetAxis("Mouse Y");    //verso alto=positivo, verso basso=negativo
            // -= perchè se mi muovo verso l' alto allora la prospettiva delle scendere verso il basso, invertito
            rotazione -= mouseY;
            rotazione = Mathf.Clamp(rotazione, -70f, 70f);  //limita il valore tra + e - 70

            //applica la rotazione solo su asse X dei gradi stabiliti dal caloclo sopra

            //local rotation tiene conto in maniera relativa del transfrom del parent e quindi non sono rotazioni globali ma basate sul player
            primaPersona.transform.localRotation = Quaternion.Euler(rotazione, 0f, 0f);    
        }

    }

    

    void AggiornaCamere()
    {
        //primaPersona.gameObject.SetActive(inPrimaPersona);
        //terzaPersona.gameObject.SetActive(!inPrimaPersona);

        //non setactive false perchè sennò neanche gli script vanno più
        primaPersona.enabled = inPrimaPersona;
        terzaPersona.enabled = !inPrimaPersona;
    }
}
