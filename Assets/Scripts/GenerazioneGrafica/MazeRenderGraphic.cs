using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class MazeRenderGraphic : MonoBehaviour
{
    [SerializeField] MazeGenerator generatoreLogica;    //labirinto logico
    [SerializeField] SpawnCollezzionabili collezzionabili;  //monete logiche

    [SerializeField] Cella cellaPrefab;
    [SerializeField] GameObject entrataUscitaPrefab;
    [SerializeField] GameObject pressurePlatePrefab;
    [SerializeField] GameObject collezionabilePrefab;

    public event EventHandler GraficaPronta;
    public Vector3 posizioneEntrata { get; set; }   //serve nello spawn quando viene invocato questo evento (del rendere grafico)
    public Vector3 direzioneEntrata { get; set; }   //idem

    void Awake()
    {
        collezzionabili.CollezzionabiliGenerati += ImpostaGrafica;
    }

    public void ImpostaGrafica(object sender, EventArgs e)
    {
        //cancella vecchia grafica
        foreach (Transform t in transform)
            Destroy(t.gameObject);

        //LOGICA
        GeneratoreCella[,] percorso = generatoreLogica.percorso;
        int lunghezza = generatoreLogica.lunghezza;
        int larghezza = generatoreLogica.larghezza;

        // *** FIX: usa la scala del prefab, non del parent ***
        float cellSize = cellaPrefab.transform.localScale.x;

        // COSTRUZIONE CELLE GRAFICHE
        for (int x = 0; x < larghezza; x++)
        {
            for (int z = 0; z < lunghezza; z++)
            {
                Vector3 posizione = new Vector3(x * cellSize, 0, z * cellSize);

                Cella cellaGrafica = Instantiate(cellaPrefab, posizione, Quaternion.identity, transform);

                //copia dati logici e dopo grafica
                GeneratoreCella logica = percorso[x, z];

                cellaGrafica.x = logica.x;
                cellaGrafica.z = logica.z;

                cellaGrafica.Visitata = logica.Visitata;
                cellaGrafica.Muro_Sinistro = logica.Muro_Sinistro;
                cellaGrafica.Muro_Destro = logica.Muro_Destro;
                cellaGrafica.Muro_Anteriore = logica.Muro_Anteriore;
                cellaGrafica.Muro_Posteriore = logica.Muro_Posteriore;

                cellaGrafica.AggiornaGrafica();
            }
        }

        // --- ENTRATA ---
        GeneratoreCella entrata = generatoreLogica.CellaEntrata;
        posizioneEntrata = new Vector3(entrata.x * cellSize, 0, entrata.z * cellSize);

        // calcola direzione verso l'esterno
        //direzioneEntrata = Vector3.zero;
        if (entrata.x == 0)
            direzioneEntrata = Vector3.left;
        else if (entrata.x == larghezza - 1)
            direzioneEntrata = Vector3.right;
        else if (entrata.z == 0) 
            direzioneEntrata = Vector3.back;
        else if (entrata.z == lunghezza - 1) 
            direzioneEntrata = Vector3.forward;

        // sposta di una cella verso l'esterno
        posizioneEntrata += direzioneEntrata * cellSize;

        GameObject CellaEntrata = Instantiate(entrataUscitaPrefab, posizioneEntrata, Quaternion.identity, transform);

        CellaEntrataUscita graficaEntrata = CellaEntrata.GetComponent<CellaEntrataUscita>();
        graficaEntrata.TogliMuro(direzioneEntrata);



        // --- USCITA ---
        GeneratoreCella uscita = generatoreLogica.CellaUscita;
        Vector3 posUscita = new Vector3(uscita.x * cellSize, 0, uscita.z * cellSize);

        Vector3 dirUscita = Vector3.zero;
        if (uscita.x == 0) dirUscita = Vector3.left;
        else if (uscita.x == larghezza - 1) dirUscita = Vector3.right;
        else if (uscita.z == 0) dirUscita = Vector3.back;
        else if (uscita.z == lunghezza - 1) dirUscita = Vector3.forward;

        posUscita += dirUscita * cellSize;

        GameObject uscitaMuroDaTogliere = Instantiate(entrataUscitaPrefab, posUscita, Quaternion.identity, transform);

        CellaEntrataUscita graficaUscita = uscitaMuroDaTogliere.GetComponent<CellaEntrataUscita>();
        graficaUscita.TogliMuro(dirUscita);



        // pressure plate sopra l’uscita

        Vector3 posPressurePlate = posUscita;
        posPressurePlate.y = 0.10f;

        Instantiate(pressurePlatePrefab, posPressurePlate, Quaternion.identity, transform);
        
            //PRESSURE PLATE HA COLLIDER MAGGIORE DI QUELLO REALMENTE VISIBILE VIA GRAFICA PERCHè SENNò CC NON TOCCA E NON PARTE TRIGGER


        if (collezzionabili.celleGiaOccupate == null || collezzionabili.celleGiaOccupate.Count == 0)
        {
            Debug.Log("nessuna cella occupata no monete da disegnare");
            return;
        }

        //monete da raccogliere
        Vector3 posCollezzionabili;
        foreach (GeneratoreCella cella in collezzionabili.celleGiaOccupate)
        {
            posCollezzionabili = new Vector3(cella.x * cellSize, 0.7f, cella.z * cellSize);
            GameObject moneta = Instantiate(collezionabilePrefab, posCollezzionabili, Quaternion.identity, transform);
        }


        GraficaPronta?.Invoke(this, EventArgs.Empty);

    }
}
