using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    [SerializeField] PlayerMovement player;
    [SerializeField] PopUpFine popup;
    [SerializeField] MazeGenerator generatoreLabirinto;
    [SerializeField] MazeRenderGraphic rendererGrafico;
    [SerializeField] Spawn spawn;
    [SerializeField] Timer timer;

    private void Start()
    {        
        player.ArrivatoAllaFine += OnFine;
    }

    private void OnFine(object sender, EventArgs e)
    {
        //popup.Mostra();

        timer.FermATimer();

        player.enabled = false;
        //popup.Mostra();
    }

    public void ScenaSuccessiva()
    {
        SceneManager.LoadScene(1);
    }


    //public void EffettuaRestart()
    //{
    //    //generatoreLabirinto.CellaEntrata = null;
    //    generatoreLabirinto.GeneraLogica();

    //    //rendererGrafico.ImpostaGrafica();
    //    spawn.Spawna();
    //    //await spawn.EseguiSpawn();
    //    //Vector3 pos = generatoreLabirinto.CellaEntrata.transform.position; 
    //    //player.transform.position = pos; 
    //}
}
