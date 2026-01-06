using System;
using TMPro;
using UnityEngine;

public class TimerRenderGraphic : MonoBehaviour
{
    TMP_Text testo;
    [SerializeField] Timer timer;
    [SerializeField] Restart restart;

    bool aspettaCliclPerRigenerareDaCapo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        testo = GetComponent<TMP_Text>();
        timer.TimerFinito += OnTimerFinito;
        testo.gameObject.SetActive(false);  //lo cancella dalla scena
        aspettaCliclPerRigenerareDaCapo = false;    //prima di ricominciare aspetta che si clicki nello schermo
    }


    private void OnTimerFinito(object sender, EventArgs e)
    {
        int minuti = (int)Math.Floor(timer.tempo/60);
        int secondi = (int)Math.Floor(timer.tempo % 60);

        //forma formattazione con 2 cifre quindi sarebbe tipo 03:04 e non 3;4
        testo.text = $"Tempo totale: {minuti:00}:{secondi:00}";
        testo.gameObject.SetActive(true);   //lo include nella scena
        aspettaCliclPerRigenerareDaCapo =true;
    }

    // Update is called once per frame
    void Update()
    {
        if (aspettaCliclPerRigenerareDaCapo)    //quando si arriva alla fine e quindi aspettaper... è true allora aspetta il click sx
        {
            if (Input.GetMouseButtonDown(0))    //prima di rigenerare tutto (e chiedere se si vuole rigiocare)
                                                //aspetta un click del mouse
            {
                restart.ScenaSuccessiva();
            }
        }
    }
}
