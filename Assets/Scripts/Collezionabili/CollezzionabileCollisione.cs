using UnityEngine;

public class CollezzionabileCollisione : MonoBehaviour
{
    InventarioCollezzionabili player;
    private void OnTriggerEnter(Collider other)
    {

        //il box collider sta fermo mentre la moneta (prog scaricato dall' assets stoer) si muove, ruota, scala ecc.
        //non infulenza troppo visto che comunque il player ci deve andare addosso in ogni caso

        if (other.CompareTag("Player")) //se player va addosso allora la toglie
        {
            player.CatturaCollezzionabile();
            Destroy(this.gameObject);
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
