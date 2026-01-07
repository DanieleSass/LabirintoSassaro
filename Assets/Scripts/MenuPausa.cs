using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuPausa : MonoBehaviour
{
    [SerializeField] GameObject panel;
    bool giocoInPausa;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        giocoInPausa = false;
        panel.SetActive(false);

        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            giocoInPausa = !giocoInPausa;
            SettaDecisione();   //sia con esc
        }
    }

    public void PremutoEsci()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
        SceneManager.LoadScene(0);
    }

    public void PremutoContinua()
    {
        giocoInPausa = false;
        SettaDecisione();   //che per pulsante
    }

    private void SettaDecisione()
    {
        if (giocoInPausa)
        {
            panel.gameObject.SetActive(true);
            Time.timeScale = 0;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
        }
        else
        {
            panel.gameObject.SetActive(false);
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            UnityEngine.Cursor.visible = false;
        }
    }
}
