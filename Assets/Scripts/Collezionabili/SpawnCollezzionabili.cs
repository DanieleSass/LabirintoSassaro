using NUnit.Framework;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO.Compression;

public class SpawnCollezzionabili : MonoBehaviour
{

    [SerializeField] MazeGenerator generatore;

    public event EventHandler CollezzionabiliGenerati;  

    public List<GeneratoreCella> celleGiaOccupate { get; set; }
    public int numeroMoneteDaSpawnare { get; set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        generatore.LabirintoGenerato += OnLabirintoGenerato;
    }

    private void OnLabirintoGenerato(object sender, EventArgs e)
    {

        System.Random rand = new System.Random(Environment.TickCount);

        //facilità lettura e scrittura
        int larghezza = generatore.larghezza;
        int lunghezza = generatore.lunghezza;

        int totcelle = larghezza * lunghezza;
        //proporzionale alla grandezza del labirinto, una ogni 20 celle
        numeroMoneteDaSpawnare = Math.Max(1, totcelle/20);   //almeno una moneta da mettere gi

        celleGiaOccupate = new();  //per evitare che spawnino nella stessa cella

        int xrandom;
        int zrandom;
        GeneratoreCella cella;
        for(int i = 0; i < numeroMoneteDaSpawnare; i++)
        {
            do
            {
                xrandom = rand.Next(0, larghezza);
                zrandom = rand.Next(0, lunghezza);
                cella = new GeneratoreCella(xrandom,zrandom);
            } while (!CheckcellaLibera(cella));

            celleGiaOccupate.Add(cella);

        }
        Debug.Log("Monete generate: " + celleGiaOccupate.Count);

        //a sua volta dopo che sono spawnate queste aggiorna tutta la grafica
        CollezzionabiliGenerati?.Invoke(this, EventArgs.Empty);
        //per renderizzarle graficamente ci pensa il mazerendergraphic

    }

    private bool CheckcellaLibera(GeneratoreCella cella)
    {
        if (celleGiaOccupate.Contains(cella))
            return false;   //falso se già occupata
        else
            return true;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
