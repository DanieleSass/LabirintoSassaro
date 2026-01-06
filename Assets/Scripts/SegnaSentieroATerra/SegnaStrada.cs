using UnityEngine;

public class SegnaStrada : MonoBehaviour
{
    [SerializeField] GameObject segnoPrefab;

    //ispirato alla fiaba dove butta il pane per terra per ricordare il percorso

    int rimanenti;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rimanenti = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            if (rimanenti > 0)
            {
                Vector3 pos = transform.position;
                Quaternion rotazione = Quaternion.Euler(90, 0, 0);
                pos.y = 0.01f;  //leggermente sollevato da terra
                Instantiate(segnoPrefab, pos, rotazione); //non transform
                rimanenti--;
            }
        }
    }
}
