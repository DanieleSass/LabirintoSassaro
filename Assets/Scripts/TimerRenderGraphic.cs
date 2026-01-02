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
        testo.gameObject.SetActive(false);
        aspettaCliclPerRigenerareDaCapo = false;
    }


    private void OnTimerFinito(object sender, EventArgs e)
    {
        
        testo.text = $"Tempo totale: {timer.tempo}";
        testo.gameObject.SetActive(true);
        aspettaCliclPerRigenerareDaCapo =true;
    }

    // Update is called once per frame
    void Update()
    {
        if (aspettaCliclPerRigenerareDaCapo)
        {
            if (Input.GetMouseButtonDown(0))    //prima di rigenerare tutto (e chiedere se si vuole rigiocare)
                                                //aspetta un click del mouse
            {
                restart.ScenaSuccessiva();
            }
        }
    }
}
