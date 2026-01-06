using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class MazeGenerator : MonoBehaviour
{

    public event EventHandler LabirintoGenerato;    //genererà collezzionabili

    public GeneratoreCella[,] percorso { get; private set; }    //intero labirinto
    public GeneratoreCella CellaEntrata { get; private set; }   //cella in cui viene scavato muro verso esterno
    public GeneratoreCella CellaUscita { get; private set; }    //idem

    public int larghezza { get; private set; }
    public int lunghezza { get; private set; }

    System.Random random;

    GeneratoreCella prossima;   //variabile di appoggio nella funzione ricorsiva

    void Start()
    {
        random = new System.Random(Environment.TickCount);

        larghezza = random.Next(10,15);
        lunghezza = random.Next(10,15);
        //larghezza = 20;
        //lunghezza = 20;
        GeneraLogica();
    }

    public void GeneraLogica()
    {
        CellaEntrata = null;
        CellaUscita = null;

        percorso = new GeneratoreCella[larghezza, lunghezza];

        for (int x = 0; x < larghezza; x++)
        {
            for (int z = 0; z < lunghezza; z++)
            {
                percorso[x, z] = new GeneratoreCella(x, z); //inizializza la matrice con celle logiche (con unità=1)
            }
        }     
        GeneraLabirinto(null, percorso[0, 0]);

        GeneraEntrataUscita();  //dopo generazione labirinto fa entrata/uscita


        Cursor.lockState = CursorLockMode.Locked;
        LabirintoGenerato?.Invoke(this, EventArgs.Empty);   //con questo si iniziano a spawnare le monete e il player

        //mazeRenderGraphic.ImpostaGrafica(); //dopo mostra tutto
    }

    //funzione ricorsiva
    private void GeneraLabirinto(GeneratoreCella precedente, GeneratoreCella attuale)
    {

        attuale.Visitata = true;    //marca la cella attuale come da non visitare più

        ScavaMuro(precedente, attuale); //toglie il muro (ora logicamente) tra le due celle
       
        prossima = GetCellaNonVisitata(attuale);
        
        //caso base torna indietro di una chiamata
        if (prossima == null)
            return;
            
        //passo ricorsivo, va avanti verso quella cella trovata in maniera casuale
        GeneraLabirinto(attuale, prossima); 
        
        //quando finisce la chiamata della funzione qui sopra (pk prossima==null)allora torna indietro di una cella
        //e cerca verso un' altra direzione, e se non c'è tornerà indietro

        //segnala che ha esplorato tutto da attuale e che torna indietro per vedere se magari ci sono altre direzioni da provate
        GeneraLabirinto(precedente, attuale);
    }

    private GeneratoreCella GetCellaNonVisitata(GeneratoreCella attuale)
    {
        List<GeneratoreCella> vicine = new List<GeneratoreCella>();

        int x = attuale.x;  //facilità di lettura e scrittura
        int z = attuale.z;

        //se le possibili celle vicine non sono già visitate+sono dentro i limiti (prima controlla quello) allora sono possibili candidate ad essere la prossima cella da esplorare
        if (x + 1 < larghezza && !percorso[x + 1, z].Visitata)
            vicine.Add(percorso[x + 1, z]);

        if (x - 1 >= 0 && !percorso[x - 1, z].Visitata)
            vicine.Add(percorso[x - 1, z]);

        if (z + 1 < lunghezza && !percorso[x, z + 1].Visitata)
            vicine.Add(percorso[x, z + 1]);

        if (z - 1 >= 0 && !percorso[x, z - 1].Visitata)
            vicine.Add(percorso[x, z - 1]);

        if (vicine.Count == 0)
            return null;    //necessario

        return vicine[random.Next(0, vicine.Count)];    //ne prende una casule
    }



    private void ScavaMuro(GeneratoreCella precedente, GeneratoreCella attuale)
    {

        if(precedente == null)  //gestisce caso iniziale per esempio
            return;

        if (precedente.x < attuale.x)
        {
            precedente.Muro_Destro = false;     //toglie muri di direzione opposta
            attuale.Muro_Sinistro = false;
        }
        else if (precedente.x > attuale.x)
        {
            precedente.Muro_Sinistro = false;
            attuale.Muro_Destro = false;
        }
        else if (precedente.z < attuale.z)
        {
            precedente.Muro_Anteriore = false;
            attuale.Muro_Posteriore = false;
        }
        else if (precedente.z > attuale.z)
        {
            precedente.Muro_Posteriore = false;
            attuale.Muro_Anteriore = false;
        }
    }

    private void GeneraEntrataUscita()
    {
        CellaEntrata = GeneraCellaSulBordo();   //per facilità si poteva tenere sempre a percorso[0,0] siccome è tutto randomico

        GeneratoreCella uscita;

        do
        {
            uscita = GeneraCellaSulBordo();
        }
        //non va bene se entrata e uscita stanno sullo stesso lato
        while (GetLatoCella(uscita) == GetLatoCella(CellaEntrata));

        CellaUscita = uscita;


        GeneratoreCella fintaEntrata = CreaCellaFinta(CellaEntrata);
        GeneratoreCella fintaUscita = CreaCellaFinta(CellaUscita);
        //Debug.Log(fintaUscita.x);
        ScavaMuro(fintaEntrata, CellaEntrata); //di fatto toglie solo il muro della cella di entrata, perchè dopo le celle finte andranno perse
        ScavaMuro(fintaUscita, CellaUscita);
    }

    private GeneratoreCella CreaCellaFinta(GeneratoreCella cella)
    {
        if (cella.x == 0)
            return new GeneratoreCella(-1, cella.z);
        if (cella.x == larghezza - 1)
            return new GeneratoreCella(larghezza, cella.z);
        if (cella.z == 0)
            return new GeneratoreCella(cella.x, -1);
        if (cella.z == lunghezza - 1)
            return new GeneratoreCella(cella.x, lunghezza);

        return null;
    }


    private GeneratoreCella GeneraCellaSulBordo()
    {
        int rnd = random.Next(1, 5);

        switch (rnd)
        {
            case 1:
                return percorso[0, random.Next(0, lunghezza)];
            case 2: 
                return percorso[larghezza - 1, random.Next(0, lunghezza)];
            case 3:
                return percorso[random.Next(0, larghezza), 0];
            case 4:
                return percorso[random.Next(0, larghezza), lunghezza - 1];
        }

        return null;
    }

    private int GetLatoCella(GeneratoreCella cella)
    {
        if (cella.x == 0)   //sinistra 
            return 1;   //valori random scelti senza logica
        if (cella.x == larghezza - 1)   //destra
            return 2;
        if (cella.z == 0)   //alto-indietro
            return 3;
        if (cella.z == lunghezza - 1)   //basso-avanti
            return 4;

        return 0;
    }

}
