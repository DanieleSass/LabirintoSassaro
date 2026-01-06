using NUnit.Framework;
using System;
using UnityEngine;
using System.Collections.Generic;

public class SpawnPortali : MonoBehaviour
{

    [SerializeField] SpawnCollezzionabili spawnCollezzionabili;
    [SerializeField] MazeGenerator generatore;

    public List<GeneratoreCella> celleOccupate { get; set; }    //serve per la grafica da istanziare

    public event EventHandler PortaliSpawnati;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        celleOccupate = new();
        //appena le monete sono state generate, genera anche i portali (2)
        spawnCollezzionabili.CollezzionabiliGenerati += OnCollezzionabiliGenerati;
    }

    private void OnCollezzionabiliGenerati(object sender, EventArgs e)
    {
        System.Random rand = new System.Random(Environment.TickCount);
        int xrandom;
        int zrandom;
        int larghezza = generatore.larghezza;
        int lunghezza = generatore.lunghezza;
        GeneratoreCella cella;
        for (int i = 0; i <2; i++)  //2 portali
        {
            do
            {
                xrandom = rand.Next(0, larghezza);
                zrandom = rand.Next(0, lunghezza);
                //cella=new GeneratoreCella(xrandom, zrandom);
                cella = generatore.percorso[xrandom, zrandom];  //sta volta prende la cella del percorso del labirinto e non ne crae una nuova
                //perchè deve controllare come sono messi i  muri della cella
                //vedi funzione EUnCorridioi
            } while (!CheckCellaLibera(cella));
            celleOccupate.Add(cella);
        }
        Debug.Log("fino a qua");
        PortaliSpawnati?.Invoke(this, EventArgs.Empty);
    }

    private bool CheckCellaLibera(GeneratoreCella cella)
    {

        if(!EUnCorridoio(cella))
            return false;

        //stessi controlli delle monete

        //eveita che nascano sopra le monete
        foreach(GeneratoreCella cellaa in spawnCollezzionabili.celleGiaOccupate)
        {
            if (cella.x == cellaa.x && cella.z == cellaa.z)
                return false;
        }
        //evita che nasca sopra l' altro portale
        foreach(GeneratoreCella cellaa in celleOccupate)
        {
            if (cella.x == cellaa.x && cella.z == cellaa.z)
                return false;
        }

        //if (spawnCollezzionabili.celleGiaOccupate.Contains(cella))
        //    return false;

        //if(celleOccupate.Contains(cella))
        //    return false;

        return true;
    }

    private bool EUnCorridoio(GeneratoreCella c)
    {
        bool su = !c.Muro_Anteriore;    //se non c'è un muro la variabile diventa true
        bool giu = !c.Muro_Posteriore;
        bool dx = !c.Muro_Destro;
        bool sx = !c.Muro_Sinistro;

        int aperture = 0;
        if (su) 
            aperture++;
        if (giu) 
            aperture++;
        if (dx) 
            aperture++;
        if (sx)
            aperture++;

        //un corridoio deve avere esattamente 2 aperture
        if (aperture != 2)
            return false;

        //che devono essere opposte
        if (su && giu)
            return true;

        if (dx && sx)
            return true;

        return false;

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
