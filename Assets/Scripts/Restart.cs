using System;
using UnityEngine;
using UnityEngine.Audio;

public class Restart : MonoBehaviour
{
    [SerializeField] PlayerMovement player;
    [SerializeField] PopUpFine popup;
    [SerializeField] MazeGenerator generatoreLabirinto;
    [SerializeField] Spawn spawn;

    private void Start()
    {        
        player.ArrivatoAllaFine += OnFine;
    }

    private void OnFine(object sender, EventArgs e)
    {
        popup.Mostra();

    }

    public async void EffettuaRestart()
    {
        //generatoreLabirinto.CellaEntrata = null;
        generatoreLabirinto.Rigenera();
        await spawn.EseguiSpawn();
        //Vector3 pos = generatoreLabirinto.CellaEntrata.transform.position; 
        //player.transform.position = pos; 
    }
}
