using UnityEngine;
using System.Threading.Tasks;
using System;

public class Spawn : MonoBehaviour
{
    [SerializeField] MazeGenerator GeneratoreLabirinito;
    [SerializeField] MazeRenderGraphic renderGraphic;


    public event EventHandler PlayerSpawnato;
    void Awake()
    {
        //GeneratoreLabirinito.LabirintoGenerato += OnLabirintoGenerato;
        renderGraphic.GraficaPronta += OnGraficaPronta;     //per comodità di generazione (prefab con scale diverse da 1x11x)
                                                            //uso direttamente la posizione reale e non quella logica
        //EseguiSpawn();
    }

    private void OnGraficaPronta(object sender, EventArgs e)
    {
        Spawna();
    }

    public void Spawna()
    {
        Vector3 posizione = renderGraphic.posizioneEntrata;
        // alza un po’ il player
        posizione.y = 1f;
        CharacterController cc = GetComponent<CharacterController>();   //cc blocca il tp
        cc.enabled = false;
        transform.position = posizione;
        // guarda verso l'interno del labirinto
        Vector3 direzione = renderGraphic.direzioneEntrata;
        transform.rotation = Quaternion.LookRotation(-direzione);   //bisogna invertire la posizione
        cc.enabled = true;

        PlayerSpawnato?.Invoke(this, EventArgs.Empty);
    }


}
