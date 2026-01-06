using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class MazeRenderGraphic : MonoBehaviour
{
    [SerializeField] MazeGenerator generatoreLogica;    //labirinto logico  (serve per posizioni)
    [SerializeField] SpawnCollezzionabili collezzionabili;  //monete logiche (serve per posizioni)

    [SerializeField] SpawnPortali portali;  //portali logici (serve per evento + posizioni)

    //prefab da posizionare nella mappa
    [SerializeField] Cella cellaPrefab;
    [SerializeField] CellaEntrataUscita entrataUscitaPrefab;
    [SerializeField] GameObject pressurePlatePrefab;
    [SerializeField] GameObject collezionabilePrefab;
    [SerializeField] PortaleCollisione portaliPrefab;

    public event EventHandler GraficaPronta;    //quando tutta la grafica è pronta, allora giu il player
    //perchè per comodità uso le coordinate grafiche per posizionarlo con le due variabili qua sotto
    public Vector3 posizioneEntrata { get; set; }   //serve nello spawn quando viene invocato questo evento (del rendere grafico)
    public Vector3 direzioneEntrata { get; set; }   //idem

    void Awake()
    {
        portali.PortaliSpawnati += ImpostaGrafica;
        //una volta determinate le posizioni di tutto allora disegna sulla mappa

        //collezzionabili.CollezzionabiliGenerati += ImpostaGrafica;
    }

    public void ImpostaGrafica(object sender, EventArgs e)
    {
        //cancella vecchia grafica, serviva quando gestivo tutto con unica scena puliva tutti i child del generatore di labirinti (tutti i gameobject quindi)
        foreach (Transform t in transform)
            Destroy(t.gameObject);
        
        //comodità lettura/scrittura
        GeneratoreCella[,] percorso = generatoreLogica.percorso;
        int lunghezza = generatoreLogica.lunghezza;
        int larghezza = generatoreLogica.larghezza;
    
        //il fattore per cui moltiplicherò tutto quando istanzio, per avere un' unica scala
        //siccome è un cubo è anche uguale a quella z per esempio ( e anche y)
        float scalaCella = cellaPrefab.transform.localScale.x;

        //celle grafiche
        Vector3 posizione;
        Cella cellaGrafica;
        GeneratoreCella cellaLogica;
        for (int x = 0; x < larghezza; x++)
        {
            for (int z = 0; z < lunghezza; z++)
            {
                posizione = new Vector3(x * scalaCella, 0, z * scalaCella);
                cellaGrafica = Instantiate(cellaPrefab, posizione, Quaternion.identity, transform);

                //copia dati logici e dopo grafica
                cellaLogica = percorso[x, z];


                cellaGrafica.Visitata = cellaLogica.Visitata;
                cellaGrafica.Muro_Sinistro = cellaLogica.Muro_Sinistro;
                cellaGrafica.Muro_Destro = cellaLogica.Muro_Destro;
                cellaGrafica.Muro_Anteriore = cellaLogica.Muro_Anteriore;
                cellaGrafica.Muro_Posteriore = cellaLogica.Muro_Posteriore;

                cellaGrafica.AggiornaGrafica();
            }
        }

        //entrata
        GeneratoreCella entrata = generatoreLogica.CellaEntrata;
        posizioneEntrata = new Vector3(entrata.x * scalaCella, 0, entrata.z * scalaCella);
        //calcola direzione verso l'esterno
        direzioneEntrata = DeterminaDirezioneEntrataUscita(entrata, larghezza,lunghezza);
        //sposta di una cella verso l'esterno, moltiplica per il fattore così da renderlo scalato al mondo
        posizioneEntrata += direzioneEntrata * scalaCella;  //+= perchè vettore.qualcosa ha già direzione con segno +-

        CellaEntrataUscita CellaEntrata = Instantiate(entrataUscitaPrefab, posizioneEntrata, Quaternion.identity, transform);
        CellaEntrata.TogliMuro(direzioneEntrata);


        //uscita   
        GeneratoreCella uscita = generatoreLogica.CellaUscita;
        Vector3 posUscita = new Vector3(uscita.x * scalaCella, 0, uscita.z * scalaCella);
        Vector3 dirUscita = DeterminaDirezioneEntrataUscita(uscita, larghezza, lunghezza);

        posUscita += dirUscita * scalaCella;

        CellaEntrataUscita uscitaMuroDaTogliere = Instantiate(entrataUscitaPrefab, posUscita, Quaternion.identity, transform);
        uscitaMuroDaTogliere.TogliMuro(dirUscita);
        

        //pressure plate sopra l’uscita
        Vector3 posPressurePlate = posUscita;
        posPressurePlate.y = 0.10f;

        Instantiate(pressurePlatePrefab, posPressurePlate, Quaternion.identity, transform);
        
        //PRESSURE PLATE HA COLLIDER MAGGIORE DI QUELLO REALMENTE VISIBILE VIA GRAFICA PERCHè SENNò CC NON TOCCA E NON PARTE TRIGGER


        if (collezzionabili.celleGiaOccupate == null || collezzionabili.celleGiaOccupate.Count == 0)
        {
            Debug.Log("nessuna cella occupata no monete da disegnare");
            GraficaPronta?.Invoke(this, EventArgs.Empty);   //dopo spawna player
            return;
        }

        //monete da raccogliere
        Vector3 posCollezzionabili;
        foreach (GeneratoreCella cella in collezzionabili.celleGiaOccupate)
        {
            posCollezzionabili = new Vector3(cella.x * scalaCella, 0.7f, cella.z * scalaCella);
            GameObject moneta = Instantiate(collezionabilePrefab, posCollezzionabili, Quaternion.identity, transform);
        }


        //portali    
        GeneratoreCella cella1 = portali.celleOccupate[0];
        GeneratoreCella cella2 = portali.celleOccupate[1];

        PortaleCollisione portale1 = CreaPortale(cella1, scalaCella);
        PortaleCollisione portale2 = CreaPortale(cella2 , scalaCella);

        portale1.destinazione = portale2.transform;
        portale1.altroPortale = portale2; 

        portale2.destinazione = portale1.transform;
        portale2.altroPortale=portale1;

        GraficaPronta?.Invoke(this, EventArgs.Empty);   //dopo spawna player

    }

    private Vector3 DeterminaDirezioneEntrataUscita(GeneratoreCella cella, int larghezza, int lunghezza)
    {
        if (cella.x == 0)
            return Vector3.left;    //vettori di un'unità che indicano direzione
        else if (cella.x == larghezza - 1)
            return Vector3.right;
        else if (cella.z == 0)
            return Vector3.back;
        else if(cella.z==lunghezza - 1)
         return Vector3.forward;

        return Vector3.zero;
    }

    private PortaleCollisione CreaPortale(GeneratoreCella cella, float scala)
    {
        Vector3 pos = new Vector3(cella.x * scala, 0, cella.z * scala);

        Vector3 dir = Vector3.forward;

        //per farlo orientare sempre nella maniera più giusta
        if (!cella.Muro_Anteriore || !cella.Muro_Posteriore)
            dir = Vector3.right;
        else if (!cella.Muro_Destro || !cella.Muro_Sinistro)
            dir = Vector3.forward;

        Quaternion rot = Quaternion.LookRotation(dir);
        PortaleCollisione portale = Instantiate(portaliPrefab, pos, rot, transform);
        return portale;
    }

}
