using System;
using TMPro;
using UnityEngine;

public class CollezzionabiliRenderGraphic : MonoBehaviour
{
    TMP_Text testo;
    [SerializeField] InventarioCollezzionabili inventario;
    [SerializeField] SpawnCollezzionabili spawner;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        testo = GetComponent<TMP_Text>();
        inventario.MonetaRaccolta += OnMonetaRaccolta;
    }

    private void OnMonetaRaccolta(object sender, EventArgs e)
    {
        testo.text = $"{inventario.ContatoreCollezzionabili}/{spawner.numeroMoneteDaSpawnare}";
        testo.ForceMeshUpdate();
        //Debug.Log("Testo aggiornato a: " + testo.text);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
