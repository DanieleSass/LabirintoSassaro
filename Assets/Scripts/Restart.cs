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

    private void Start()    //non serve awake perchè sicura arriva alla fine dopo un bel po'
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
        SceneManager.LoadScene(2);  //carica se si vuole giocare ancora oppure no

    }

}
