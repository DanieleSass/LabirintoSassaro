using UnityEngine;

public class PortaleCollisione : MonoBehaviour
{

    public Transform destinazione { get; set; }     //valori assegnati via codice durante il render grafico di tutti i componenti
    public PortaleCollisione altroPortale { get; set;}  //idem

    bool appenaTeletrasportato;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        appenaTeletrasportato = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (appenaTeletrasportato)  //serve per evitare continui tp e dare possibilità di scegliere direzione in cui muoversi
            {
                return;
            }

            //se arriva qua vuol dire che può essere teletrasportato

            CharacterController cc = other.GetComponent<CharacterController>(); //sennò viene bloccato il tp
            cc.enabled = false;
            //other è quello che attiva il collider, quindi il player
            other.transform.position=destinazione.position;
            cc.enabled = true;
            appenaTeletrasportato = true;
            altroPortale.appenaTeletrasportato = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            appenaTeletrasportato = false;
        }
    }
}
