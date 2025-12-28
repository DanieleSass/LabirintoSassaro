using UnityEngine;
using System.Threading.Tasks;

public class Spawn : MonoBehaviour
{
    [SerializeField] MazeGenerator GeneratoreLabirinito;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        //aspetta che il labirinto si crei
        await AspettaGenerazioneLabirinto();

        Cella entrata = GeneratoreLabirinito.CellaEntrata;
        Vector3 scala = GeneratoreLabirinito.transform.lossyScale;
        Vector3 direzione = Vector3.zero;   //obbligatoria assegnarla al'inizio

        if (entrata.x == 0) direzione = Vector3.left;
        else if (entrata.x == GeneratoreLabirinito.larghezza - 1) direzione = Vector3.right;
        else if (entrata.z == 0) direzione = Vector3.back;
        else if (entrata.z == GeneratoreLabirinito.lunghezza - 1) direzione = Vector3.forward;

        //posizione della cella di entrata
        Vector3 posizione = new Vector3( entrata.x * scala.x, 0, entrata.z * scala.z );
        //sposta di una cella verso l’esterno
        posizione += direzione * scala.x;

        //lo posiziona
        posizione.y = 1f;


        transform.position = posizione;


        //gira il player verso l'interno del labirinto
        transform.rotation = Quaternion.LookRotation(-direzione);
    }

    private async Task AspettaGenerazioneLabirinto()
    {
        while(GeneratoreLabirinito.CellaEntrata == null)
        {
            await Task.Yield();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
