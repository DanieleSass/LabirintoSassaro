using UnityEngine;

public class Contapassi : MonoBehaviour
{

    public int passiPercorsi {  get; private set; }

    Vector3 posPrecedente;
    float distanza;

    bool primoAvvio;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //passiPercorsi = 0;
        posPrecedente = transform.position; //posizione player
        posPrecedente.y = 0;        //fissa a 0 per evitare che vengano contati anche i salti come passi
        distanza = 0;

        primoAvvio = false;
    }
    private void LateUpdate()
    {
        if (!primoAvvio)
        {
            primoAvvio = true;
            passiPercorsi = 0;  //serve per resettare i passi a 0, perchè sennò sarebbero a 1
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        //dichiarata globalmente perchè ad ogni frame si resetta

        Vector3 posAttuale  = transform.position;
        posAttuale.y = 0;   //per evitare che conta come distanza anche quella verticale/ salto

        distanza += Vector3.Distance(posAttuale, posPrecedente);
        if (distanza >= 1.5)    //passo di 1.5
        {
            //passo validato e resetta la distanza accumulata
            passiPercorsi++;
            distanza = 0;
        }
        posPrecedente=posAttuale;
    }
}
