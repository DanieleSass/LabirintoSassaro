using NUnit.Framework;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO.Compression;

public class SpawnCollezzionabili : MonoBehaviour
{

    [SerializeField] MazeGenerator generatore;

    public event EventHandler CollezzionabiliGenerati;  

    public List<GeneratoreCella> celleGiaOccupate { get; set; } //serve per lo spawn dei portali e per la grafica da istanziare
    public int numeroMoneteDaSpawnare { get; set; } //serve per la grafica e la cond di win

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        generatore.LabirintoGenerato += OnLabirintoGenerato;    //appena il labirinto logico è pronto allora butta giù le monete
    }

    private void OnLabirintoGenerato(object sender, EventArgs e)
    {

        System.Random rand = new System.Random(Environment.TickCount);

        //facilità lettura e scrittura
        int larghezza = generatore.larghezza;
        int lunghezza = generatore.lunghezza;

        int totcelle = larghezza * lunghezza;   //area
        //proporzionale alla grandezza del labirinto, una ogni 20 celle
        numeroMoneteDaSpawnare = Math.Max(1, totcelle/20);   //almeno una moneta da mettere giu

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

        //a sua volta dopo che sono spawnate queste butta giu i portali
        CollezzionabiliGenerati?.Invoke(this, EventArgs.Empty);
        //per renderizzarle graficamente ci pensa il mazerendergraphic

    }

    private bool CheckcellaLibera(GeneratoreCella cella)
    {
        //if (celleGiaOccupate.Contains(cella))
        //    return false;   //falso se già occupata
        //else
        //    return true;

        foreach(GeneratoreCella cellaa in celleGiaOccupate)
        {
            if (cella.x == cellaa.x && cella.z == cellaa.z)
                return false;
        }
        return true;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
