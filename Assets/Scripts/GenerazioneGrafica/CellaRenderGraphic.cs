using UnityEngine;

public class Cella : MonoBehaviour
{
    [SerializeField] GameObject MuroDestro;

    [SerializeField] GameObject MuroSinistro;

    [SerializeField]GameObject MuroAnterirore;

    [SerializeField] GameObject MuroPosteriore;

    [SerializeField] GameObject BloccoNONVisitato;


    public int x { get; set; }
    public int z { get; set; }
    public bool Visitata { get; set; }

    public bool Muro_Sinistro { get; set; }
    public bool Muro_Destro { get; set; }
    public bool Muro_Anteriore { get; set; }
    public bool Muro_Posteriore { get; set; }


    //così da tenere separata logica e grafica
    public void AggiornaGrafica()
    {
        MuroDestro.SetActive(Muro_Destro);
        MuroSinistro.SetActive(Muro_Sinistro);
        MuroAnterirore.SetActive(Muro_Anteriore);
        MuroPosteriore.SetActive(Muro_Posteriore);

        BloccoNONVisitato.SetActive(!Visitata);
    }
}
