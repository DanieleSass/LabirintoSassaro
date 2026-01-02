using UnityEngine;

public class GeneratoreCella        //classe normale che non eredita monobehaviour, quindi può avere un costruttore normale che funzioni
                                    //e non ha riferimenti grafici, solo logici
{
    public int x {  get; set; }
    public int z { get; set; }

    public bool Visitata { get; set; }

    public bool Muro_Sinistro { get; set; }
    public bool Muro_Destro { get; set; }
    public bool Muro_Anteriore { get; set; }
    public bool Muro_Posteriore { get; set; }

    public GeneratoreCella(int xx, int zz)
    {
        x = xx;
        z= zz;
        Visitata = false;

        Muro_Anteriore = true;  //di default ha ancora tutti i muri
        Muro_Posteriore = true;
        Muro_Destro = true;
        Muro_Sinistro= true;
    }
}
