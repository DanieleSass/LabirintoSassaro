using TMPro;
using UnityEngine;

public class ContapassiRenderGraphic : MonoBehaviour
{
    TMP_Text testo;
    [SerializeField] Contapassi player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        testo = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        //non ho usato evento perchè ne verrebbe invocato tipo 1/s e quindi potevo usare anche update
        testo.text=player.passiPercorsi.ToString();
    }
}
