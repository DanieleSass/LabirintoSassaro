using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuIniziale : MonoBehaviour
{


    public void Gioca()
    {
        SceneManager.LoadScene(1);
    }

    public void Esci()
    {

        Application.Quit();
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
