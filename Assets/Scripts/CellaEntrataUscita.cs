using UnityEngine;

public class CellaEntrataUscita : MonoBehaviour
{

    [SerializeField] GameObject MuroDestro;
    [SerializeField] GameObject MuroSinistro;
    [SerializeField] GameObject MuroAnteriore;
    [SerializeField] GameObject MuroPosteriore;


    public void TogliMuro(Vector3 direzione)
    {
        if(direzione == Vector3.left)
            MuroDestro.SetActive(false);
        else if(direzione == Vector3.right)
            MuroSinistro.SetActive(false);
        else if (direzione == Vector3.forward)
            MuroPosteriore.SetActive(false);
        else if(direzione == Vector3.back)
            MuroAnteriore.SetActive(false);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
