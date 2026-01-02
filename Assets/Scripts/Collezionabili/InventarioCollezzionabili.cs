using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventarioCollezzionabili : MonoBehaviour
{

    public int ContatoreCollezzionabili { get; private set; }

    public event EventHandler MonetaRaccolta; 


    public void CatturaCollezzionabile()
    {
        ContatoreCollezzionabili++;
        MonetaRaccolta?.Invoke(this, EventArgs.Empty);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ContatoreCollezzionabili = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
