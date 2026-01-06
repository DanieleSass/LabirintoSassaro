using UnityEngine;

public class CollezzionabileCollisione : MonoBehaviour


    //PREFAB DI MONETE, PORTALI, AMBIENTAZIONE SONO TUTTI SCARICATI DA INTERNET, ANCHE SAGOMA PLAYER
{
    InventarioCollezzionabili player;   //è un prefab quindi prendo il riferimento via codice (nello start)
    private void OnTriggerEnter(Collider other)
    {

        //il box collider sta fermo mentre la moneta (prog scaricato dall' assets stoer) si muove, ruota, scala ecc.
        //non infulenza troppo visto che comunque il player ci deve andare addosso in ogni caso

        if (other.CompareTag("Player")) //se player va addosso allora la toglie
        {
            player.CatturaCollezzionabile();
            Destroy(this.gameObject);   //toglie la moneta dal gioco
            Debug.Log("moneta raccoltta");

        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //non serializzabile perchè prefab e non in scena come il player
        player = FindAnyObjectByType<InventarioCollezzionabili>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
