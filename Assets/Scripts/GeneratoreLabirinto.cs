using Mono.Cecil;
using NUnit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] Cella CellaPrefab;
    [SerializeField] GameObject CellaEntrataUscitaPrefab;
    [SerializeField] GameObject PressurePlatePrefab;

    Cella[,] percorso;
    System.Random random;

    public Cella CellaEntrata { get; set; }
    public int larghezza { get; private set; }
    public int lunghezza { get; private set; }

    void Start()
    {
        random = new System.Random(Environment.TickCount);

        lunghezza = 10;
        larghezza = 10;
        //lunghezza = random.Next(15,25);
        //larghezza = random.Next(15,25);

        Rigenera();

        //percorso = new Cella[larghezza, lunghezza];
        //Vector3 scala;
        //Vector3 posizione;
        //for (int x = 0; x < larghezza; x++)
        //{
        //    for (int y = 0; y < lunghezza; y++)
        //    {
        //        scala = transform.lossyScale;   //è la scala dell' oggetto (impostato nel menu inspector per esempio
        //        //così se si modifica quello tutte le celle (figlie) si adattano a quella del padre (questa, il labirinto)ù

        //        posizione = new Vector3(x * scala.x, 0, y * scala.z);   //ogni celle è posizionata in base alla scala del padre
        //        percorso[x, y] = Instantiate(CellaPrefab, posizione, Quaternion.identity, transform);
        //        //con trasform allora tutte le istanze delle celle diventano anche figlie del labitinto, e quindi assumono la scala predefinita impostata nel labirinto
        //        percorso[x, y].x = x;
        //        percorso[x, y].z = y;
        //    }
        //}
        //GeneraLabirinto(null, percorso[0, 0]);


        ////quando arriva alla fine di tutte le chiamate ricorsive allora crea l'entrata, uscita e aggiorna la grafica
        //GeneraEntrataUscita();

        //foreach (Cella cell in percorso)
        //{
        //    cell.AggiornaGrafica();
        //}

    }

    private void GeneraEntrataUscita()
    {
        //Cella entrata;
        Cella uscita;

        CellaEntrata = GeneraCellaSulBordo();
        
        PosizionaCellaEntrataUscita(CellaEntrata, "entrata");
        do
        {
            uscita = GeneraCellaSulBordo();
        }
        //rigenera se sono uguali, sono nello stesso lato
        while (CellaEntrata == uscita ||GetLatoCella(CellaEntrata) == GetLatoCella(uscita));

        ScavaMuroEntrataUscita(CellaEntrata);
        ScavaMuroEntrataUscita(uscita);

        PosizionaCellaEntrataUscita(uscita, "uscita");
        //PosizionaPlateUscita(uscita);

    }

    private void PosizionaCellaEntrataUscita(Cella cel, string tipo)
    {

        Vector3 scala = transform.lossyScale;
        Vector3 direzione = Vector3.zero;

        if (cel.x == 0) direzione = Vector3.left;
        else if (cel.x == larghezza - 1) direzione = Vector3.right;
        else if (cel.z == 0) direzione = Vector3.back;
        else if (cel.z == lunghezza - 1) direzione = Vector3.forward;

        // posizione della cella di entrata (stessa logica delle altre)
        Vector3 posizione = new Vector3(
            cel.x * scala.x,
            0,
            cel.z * scala.z
        );

        //sposta di una cella verso esterno
        posizione += direzione * scala.x; //x o z è uguale se celle quadrate

        Instantiate(
            CellaEntrataUscitaPrefab,
            posizione,
            Quaternion.LookRotation(-direzione),
            transform
        );

        if (tipo == "entrata")
        {
            return;
        }
        else if (tipo == "uscita")
        {
            Vector3 posizionePressure = new Vector3(posizione.x, 1, posizione.z);
            GameObject plate = Instantiate(PressurePlatePrefab, posizionePressure, Quaternion.identity, transform);
            plate.transform.localScale = PressurePlatePrefab.transform.localScale;
            
            //plate.transform.position += Vector3.up * (plate.transform.localScale.y * 0.5f);
        }

    }

    private Cella GeneraCellaSulBordo()
    {
        int rnd = random.Next(1, 5);
        switch (rnd)
        {
            case 1: //sinistra
                return percorso[0, random.Next(0, lunghezza)];
            case 2: //destra
                return percorso[larghezza - 1, random.Next(0, lunghezza)];
            case 3: //basso-anteriore
                return percorso[random.Next(0, larghezza), 0];
            case 4: //alto-posteriore
                return percorso[random.Next(0, larghezza), lunghezza-1];
            default:
                return null;    //nn serve controllare perchè rnd è sempre tra 1 e 4
        }
    }
    //private bool DistanzaTraEntrataUscita(Cella entrata, Cella uscita)
    //{
    //    if (entrata.x == uscita.x)
    //    {
    //        if(Math.Abs(entrata.z - uscita.z) >= 5) //almeno 5 blocchi di distanza
    //        {
    //            return true;
    //        }
    //    }
    //    else if (entrata.z == uscita.z)
    //    {
    //        if(Math.Abs(entrata.x - uscita.x) >= 5)
    //        {
    //            return true;
    //        }
    //    }
    //    return false;
    //}

    private int GetLatoCella(Cella cella)
    {
        if (cella.x == 0)   //sinistra
            return 1;
        else if(cella.x==larghezza - 1) //destra
            return 2;
        else if(cella.z == 0)   //alto-posteriore
            return 3;
        else if (cella.z == lunghezza - 1)  //basso-anteriore
            return 4;

        return 0;
    }

    private void ScavaMuroEntrataUscita(Cella cella)
    {
        if (cella.x == 0)   //lato sinistro
        {
            cella.Muro_Sinistro= false;
        }
        else if(cella.x==larghezza - 1) //lato destro
        {
            cella.Muro_Destro= false;
        }
        else if (cella.z == 0) //lato alto-posteriore
        {
            cella.Muro_Posteriore= false;
        }
        else if (cella.z == lunghezza - 1) //lato basso-anteriore   
        {
            cella.Muro_Anteriore= false;
        }
        return;
    }









    private void GeneraLabirinto(Cella precedente, Cella attuale)
    {        
        attuale.Visita();   //flag per non farci entrare di nuovo

        ScavaMuro(precedente, attuale); //toglie il muro tra le due celle
        
        Cella successiva;
        do
        {
            successiva = GetProssimaCellaDaEsplorare(attuale);

            if (successiva != null)     //firstordefault può ritornare null se non ci sono celle non visitate
            {
                GeneraLabirinto(attuale, successiva);
            }
        } while (successiva != null);   //finchè ci sono celle non visitate
    }

    private Cella GetProssimaCellaDaEsplorare(Cella attuale)
    {
        List<Cella> celleNonVisitate = GetCelleNonVisitate(attuale);
        return celleNonVisitate.OrderBy(x => random.Next(1,10)).FirstOrDefault();
    }

    private List<Cella> GetCelleNonVisitate(Cella attuale)
    {
        List<Cella> lista = new List<Cella>();

        int x = attuale.x;
        int z = attuale.z;

        if (x + 1 < larghezza && percorso[x + 1, z].Visitata == false)
            lista.Add(percorso[x + 1, z]);

        if (x - 1 >= 0 && percorso[x - 1, z].Visitata == false)
            lista.Add(percorso[x - 1, z]);

        if (z + 1 < lunghezza && percorso[x, z + 1].Visitata == false)
            lista.Add(percorso[x, z + 1]);

        if (z - 1 >= 0 && percorso[x, z - 1].Visitata == false)
            lista.Add(percorso[x, z - 1]);

        return lista;
    }



    private void ScavaMuro(Cella precedente, Cella attuale)
    {
        if (precedente == null)
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

        return;
    }


    public void Rigenera()
    {  
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        CellaEntrata = null;

        percorso = new Cella[larghezza, lunghezza];
        Vector3 scala;
        Vector3 posizione;
        for (int x = 0; x < larghezza; x++)
        {
            for (int y = 0; y < lunghezza; y++)
            {
                scala = transform.lossyScale;   //è la scala dell' oggetto (impostato nel menu inspector per esempio
                //così se si modifica quello tutte le celle (figlie) si adattano a quella del padre (questa, il labirinto)ù

                posizione = new Vector3(x * scala.x, 0, y * scala.z);   //ogni celle è posizionata in base alla scala del padre
                percorso[x, y] = Instantiate(CellaPrefab, posizione, Quaternion.identity, transform);
                //con trasform allora tutte le istanze delle celle diventano anche figlie del labitinto, e quindi assumono la scala predefinita impostata nel labirinto
                percorso[x, y].x = x;
                percorso[x, y].z = y;
            }
        }
        GeneraLabirinto(null, percorso[0, 0]);


        //quando arriva alla fine di tutte le chiamate ricorsive allora crea l'entrata, uscita e aggiorna la grafica
        GeneraEntrataUscita();

        foreach (Cella cell in percorso)
        {
            cell.AggiornaGrafica();
        }

    }

}
