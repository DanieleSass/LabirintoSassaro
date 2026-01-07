using System;
using System.Linq.Expressions;
using UnityEngine;

public class Timer : MonoBehaviour
{
    //consapevole dell' esistenza della classe timer e simili

    [SerializeField] Spawn player;
    [SerializeField] MazeRenderGraphic mazeRenderGraphic;

    public event EventHandler TimerFinito;

    bool primoMovimentoFatto;
    bool timerAttivo;

    Vector3 posPrecedente;
    public float tempo { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tempo = 0;
        primoMovimentoFatto = false;
        timerAttivo = true;
    }

    private void Awake()
    {
        player.PlayerSpawnato += OnPlayerSpawnato;
    }


    private void OnPlayerSpawnato(object sender, EventArgs e)
    {
        posPrecedente = player.transform.position;
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
                //tocca controllare componente X e Z perchè la y è leggermente sfalsata da cc e skin widht


                Vector2 posAttualeXZ = new Vector2(player.transform.position.x, player.transform.position.z);
                Vector2 posPrecedenteXZ = new Vector2(posPrecedente.x, posPrecedente.z);

                Debug.Log(posAttualeXZ);
                Debug.Log(posPrecedenteXZ);

                if (Vector2.Distance(posAttualeXZ, posPrecedenteXZ) > 0.01) //per evitare sfasamenti al millimetro
                {
                    primoMovimentoFatto = true;
                    Debug.Log("TIMER: primo movimento rilevato");
                }

                posPrecedente = player.transform.position;

            }
        }
    }

    public void FermATimer()
    {
        timerAttivo = false;
        TimerFinito?.Invoke(this, EventArgs.Empty);
    }

    
}
