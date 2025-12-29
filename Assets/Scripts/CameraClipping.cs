using UnityEngine;

public class CameraClipping : MonoBehaviour
{

    [SerializeField] Transform player;
    [SerializeField] GameObject playerGrafico;

    Vector3 distazaDaMantere;
    float distanzaInizio;

    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        distazaDaMantere = transform.position-player.position;  //distanza camera-player
        distanzaInizio = distazaDaMantere.magnitude;    
    }

    // Update is called once per frame
    void LateUpdate()       //eseguito dopo tutti gli update degli altri script in un frame, ma prima del rendering grafico
    {
        Vector3 posizioneDaImpostare = player.position + player.rotation * distazaDaMantere;
        RaycastHit hit;

        if(Physics.Linecast(player.position, posizioneDaImpostare, out hit))    //colpisce muro=avvicina cam
        {
            Debug.Log("Murocolpito");
            Vector3 direzione = (posizioneDaImpostare - player.position).normalized; 
            transform.position = hit.point - direzione * 0.2f; //offset di sicurezza
        }
        else
        {
            transform.position = posizioneDaImpostare;
        }

        //nascondi il modello se la camera è troppo vicina
        float distanzaCamera = Vector3.Distance(transform.position, player.position);

        if(distanzaCamera>1)
            playerGrafico.SetActive(true);
        else
            playerGrafico.SetActive(false);

        transform.LookAt(player.position);  //guarda sempre il player
    }
}
