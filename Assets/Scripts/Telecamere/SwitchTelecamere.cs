using Unity.Cinemachine;
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

    //bool inPrimaPersonaForzata;
    bool eraInTerzaPersonaPrimaDelClipping;

    float rotazione = 0f;

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

        if (Input.GetKeyDown(KeyCode.F))    //se è voluto dall' utente cambia la sua idea
                                            //ma non è detto che venga applicata per priorità di cameraclipping
        {
            inPrimaPersona = !inPrimaPersona;
            //AggiornaCamere();
        }

        bool effettivaPrimaPersona = inPrimaPersona || cameraClipping.primaPersonaForzata;

        primaPersona.enabled = effettivaPrimaPersona;
        terzaPersona.enabled =!effettivaPrimaPersona;

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


        //si gira solo in 1persona
        if (effettivaPrimaPersona)
        {
            float mouseY = Input.GetAxis("Mouse Y");
            rotazione -= mouseY;
            rotazione = Mathf.Clamp(rotazione, -70f, 70f);

            primaPersona.transform.localRotation =
                Quaternion.Euler(rotazione, 0f, 0f);    
        }

    }

    

    void AggiornaCamere()
    {
        //primaPersona.gameObject.SetActive(inPrimaPersona);
        //terzaPersona.gameObject.SetActive(!inPrimaPersona);

        primaPersona.enabled = inPrimaPersona;
        terzaPersona.enabled = !inPrimaPersona;
    }
}
