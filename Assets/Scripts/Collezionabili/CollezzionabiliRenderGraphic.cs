using System;
using TMPro;
using UnityEngine;

public class CollezzionabiliRenderGraphic : MonoBehaviour
{
    TMP_Text testo;
    [SerializeField] InventarioCollezzionabili inventario;
    [SerializeField] SpawnCollezzionabili spawner;


    string testoDaMostrare;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        testo = GetComponent<TMP_Text>();
        inventario.MonetaRaccolta += OnMonetaRaccolta;
        spawner.CollezzionabiliGenerati += OnCollezzionabiliGenerati;   //riutilizzo evento per il testo iniziale


    }

    private void OnMonetaRaccolta(object sender, EventArgs e)
    {
        testoDaMostrare = $"{inventario.ContatoreCollezzionabili}/{spawner.numeroMoneteDaSpawnare}";
        testo.text= testoDaMostrare;
        testo.ForceMeshUpdate();
        //Debug.Log("Testo aggiornato a: " + testo.text);
    }

    public void MostraMessaggioDiPrendereTutteLeMonete()
    {
        testo.text = "Raccogli prima tutte le monete";
    }

    public void MostraMessaggioContatoreMonete()
    {
        testo.text = testoDaMostrare;
    }

    private void OnCollezzionabiliGenerati(object sender, EventArgs e)
    {
        testoDaMostrare = $"{inventario.ContatoreCollezzionabili}/{spawner.numeroMoneteDaSpawnare}";
        //sarà 0/x
        testo.text = testoDaMostrare;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
