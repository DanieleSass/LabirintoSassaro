using UnityEngine;

public class SwitchTelecamere : MonoBehaviour
{
    [SerializeField] Camera primaPersona;

    [SerializeField] Camera terzaPersona;

    [SerializeField] GameObject player;

    bool prima_persona;
    //Camera CameraAttiva;

    float rotazione = 0;

    //CONSAPEVOLE DELL' ESISTENZA DI CINEMACHINE MA VOLEVO PROVARE A FARLO MANUALMENTE  
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        prima_persona = true;
        primaPersona.gameObject.SetActive(true);
        terzaPersona.gameObject.SetActive(false);
        player.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))    //al premere di F switcha 1/3 persona
        {
            prima_persona = !prima_persona;
            if (prima_persona)
            {
                primaPersona.gameObject.SetActive(true);
                terzaPersona.gameObject.SetActive(false);
                player.SetActive(false);
            }
            else
            {
                primaPersona.gameObject.SetActive(false);
                terzaPersona.gameObject.SetActive(true);
                player.SetActive(true);
            }
        }

        float y = Input.GetAxis("Mouse Y");
        rotazione-= y;
        rotazione = Mathf.Clamp(rotazione, -80, 80);  
        primaPersona.transform.localRotation = Quaternion.Euler(rotazione, 0f, 0f);





    }
}
