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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //popup.SetActive(false);
        SceneManager.LoadScene(1);
        //restart.EffettuaRestart();

    }
    public void PremutoNo()
    {
        Debug.Log("Premuto NO");
        Application.Quit();
    }


    private void Start()
    {
        //Mostra();
        //popup.SetActive(false);
    }

}
