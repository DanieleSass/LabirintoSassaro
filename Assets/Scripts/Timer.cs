using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    //consapevole dell' esistenza della classe timer e simili

    [SerializeField] Spawn player;
    [SerializeField] MazeRenderGraphic mazeRenderGraphic;

    public event EventHandler TimerFinito;

    bool primoMovimentoFatto;
    bool timerAttivo;
    public float tempo { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tempo = 0;
        primoMovimentoFatto = false;
        timerAttivo = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerAttivo)    //se non è stato fermato =arrivato all'arrivo
        {
            if (primoMovimentoFatto)
            {
                tempo += Time.deltaTime;
            }
            else    // al primo movimento ancora da fare
            {
                if(player.transform.position != mazeRenderGraphic.posizioneEntrata) //si è spostato allora parte tutto
                primoMovimentoFatto = true;
            }
        }
    }

    public void FermATimer()
    {
        timerAttivo = false;
        TimerFinito?.Invoke(this, EventArgs.Empty);
    }

    
}
