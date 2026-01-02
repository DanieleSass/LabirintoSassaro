using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class MazeGenerator : MonoBehaviour
{

    public event EventHandler LabirintoGenerato;    //genererà collezzionabili

    public GeneratoreCella[,] percorso { get; private set; }    //intero labirinto
    public GeneratoreCella CellaEntrata { get; private set; }
    public GeneratoreCella CellaUscita { get; private set; }

    public int larghezza { get; private set; }
    public int lunghezza { get; private set; }

    System.Random random;

    void Start()
    {
        random = new System.Random(Environment.TickCount);

        larghezza = 10;
        lunghezza = 10;

        GeneraLogica();
    }

    //GENERAZIONE LOGICA COMPLETA
    public void GeneraLogica()
    {
        CellaEntrata = null;
        CellaUscita = null;

        // CREA LA GRIGLIA LOGICA
        percorso = new GeneratoreCella[larghezza, lunghezza];

        for (int x = 0; x < larghezza; x++)
        {
            for (int z = 0; z < lunghezza; z++)
            {
                percorso[x, z] = new GeneratoreCella(x, z);
            }
        }

        //GENERA IL LABIRINTO
        GeneraLabirinto(null, percorso[0, 0]);

        //GENERA ENTRATA E USCITA
        GeneraEntrataUscita();


        Cursor.lockState = CursorLockMode.Locked;
        LabirintoGenerato?.Invoke(this, EventArgs.Empty);   //con questo si iniziano a spawnare le monete e il player

        //mazeRenderGraphic.ImpostaGrafica(); //dopo mostra tutto
    }


    //DFS/BACKTRACKING

    private void GeneraLabirinto(GeneratoreCella precedente, GeneratoreCella attuale)
    {

        //senza CICLI
        attuale.Visitata = true; 
        ScavaMuro(precedente, attuale);
        // Ottieni una cella non visitata
        GeneratoreCella prossima = GetCellaNonVisitata(attuale);
        //Caso base: nessuna cella = backtracking
        if (prossima == null)
            return;
            
        // Caso ricorsivo: continua
        GeneraLabirinto(attuale, prossima); 
        // Dopo aver finito quella direzione, prova la prossima
        GeneraLabirinto(precedente, attuale);
    }

    private GeneratoreCella GetCellaNonVisitata(GeneratoreCella attuale)
    {
        var vicine = new List<GeneratoreCella>();

        int x = attuale.x;
        int z = attuale.z;

        if (x + 1 < larghezza && !percorso[x + 1, z].Visitata)
            vicine.Add(percorso[x + 1, z]);

        if (x - 1 >= 0 && !percorso[x - 1, z].Visitata)
            vicine.Add(percorso[x - 1, z]);

        if (z + 1 < lunghezza && !percorso[x, z + 1].Visitata)
            vicine.Add(percorso[x, z + 1]);

        if (z - 1 >= 0 && !percorso[x, z - 1].Visitata)
            vicine.Add(percorso[x, z - 1]);

        if (vicine.Count == 0)
            return null;

        return vicine[random.Next(vicine.Count)];
    }



    private void ScavaMuro(GeneratoreCella precedente, GeneratoreCella attuale)
    {

        if(precedente == null)
            return;

        if (precedente.x < attuale.x)
        {
            precedente.Muro_Destro = false;
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

    //ENTRATA E USCITA (solo logica)
    private void GeneraEntrataUscita()
    {
        CellaEntrata = GeneraCellaSulBordo();

        GeneratoreCella uscita;

        do
        {
            uscita = GeneraCellaSulBordo();
        }
        while (uscita == CellaEntrata || GetLatoCella(uscita) == GetLatoCella(CellaEntrata));

        CellaUscita = uscita;


        var fintaEntrata = CreaCellaFinta(CellaEntrata);
        var fintaUscita = CreaCellaFinta(CellaUscita);
        //Debug.Log(fintaUscita.x);
        ScavaMuro(fintaEntrata, CellaEntrata); 
        ScavaMuro(fintaUscita, CellaUscita);
    }

    private GeneratoreCella CreaCellaFinta(GeneratoreCella cella)
    {
        if (cella.x == 0) return new GeneratoreCella(-1, cella.z);
        if (cella.x == larghezza - 1) return new GeneratoreCella(larghezza, cella.z);
        if (cella.z == 0) return new GeneratoreCella(cella.x, -1);
        if (cella.z == lunghezza - 1) return new GeneratoreCella(cella.x, lunghezza);

        return null;
    }


    private GeneratoreCella GeneraCellaSulBordo()
    {
        int rnd = random.Next(1, 5);

        switch (rnd)
        {
            case 1: return percorso[0, random.Next(0, lunghezza)];
            case 2: return percorso[larghezza - 1, random.Next(0, lunghezza)];
            case 3: return percorso[random.Next(0, larghezza), 0];
            case 4: return percorso[random.Next(0, larghezza), lunghezza - 1];
        }

        return null;
    }

    private int GetLatoCella(GeneratoreCella cella)
    {
        if (cella.x == 0) return 1;
        if (cella.x == larghezza - 1) return 2;
        if (cella.z == 0) return 3;
        if (cella.z == lunghezza - 1) return 4;

        return 0;
    }

}
