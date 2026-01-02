using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopUpFine : MonoBehaviour
{
    [SerializeField] GameObject popup;
    //[SerializeField] Restart restart;
    public void PremutoSi()
    {
        Debug.Log("Premuto SI");
        popup.SetActive(false);
        SceneManager.LoadScene(0);
        //restart.EffettuaRestart();

    }
    public void PremutoNo()
    {
        Debug.Log("Premuto NO");
        Application.Quit();
    }

    public void Mostra()
    {
        popup.SetActive(true);
    }

    private void Start()
    {
        //Mostra();
        //popup.SetActive(false);
    }

}
