using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] InventarioCollezzionabili inventario;
    [SerializeField] SpawnCollezzionabili spawnCollezzionabili;
    [SerializeField] CollezzionabiliRenderGraphic testoMonete;

    public event EventHandler ArrivatoAllaFine;

    float gravita = -9.81f;
    float velocita = 5f;
    float altezzaSalto = 1;

    Vector3 velocitàVerticale;  //sfrutto solo la componente Y
    bool staSaltando;

    CharacterController controller;
    Animator animator;

                //HO VOLUTO PROVARE A COLLEGARE DA SOLO ANIMAZIONI, SENZA PRENDERE PERSONAGGI CON TUTTA LA GESTIONE GIA' COMPLETA COME JAMMO


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller= GetComponent<CharacterController>();

        //inchildren perchè animator è sul player grafico, figlio del contenitore che ha script
        animator = GetComponentInChildren<Animator>();


    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");  //può essere positivo o negativo a seconda se destra o sinistra
        float z = Input.GetAxis("Vertical");

        //movimento
        Vector3 movimento = transform.right * x + transform.forward * z;
         //deltatime così è compatibile con performance di qualsiasi pc/console


        //visuale grafica
        float X = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up * X);   //ruota il player attorno a asse Y, quindi anche le videocamere

        //animazione corsa
        float speed = new Vector3(x, 0, z).magnitude;   //magnitude fa pitagora (somma vettoriale con metodo parallelogramma tipo) e prende intensità complete
        //e non le singole componenti
        animator.SetFloat("Velocita", speed);   //serve nell' animator

        //salto
        if (controller.isGrounded)  //solo se sono già a terra
        {

            if (Input.GetButtonDown("Jump"))    //e premo per saltare
            {
                float velocitàSalto = Mathf.Sqrt(altezzaSalto * -2 * gravita);  //formula fisica, - perchè gravità è negativa
                velocitàVerticale.y = velocitàSalto;
                staSaltando = true;
                animator.SetBool("InSalto", true);  //fa partire animazione
            }
        }
        else
            //per simulare la realtà ad ogni frame aggiunge gravità per far sembrare che cada più velocemente (sempre più negativa)
            velocitàVerticale.y += gravita * Time.deltaTime;

        //atterraggio, se è a terra e segna che sta ancora saltando con velocità negativa di atterraggio
        //vuol dire che ha appena toccato il suolo alla fine di un salto
        //e quindi resetta il salto e l'animazione
        if(controller.isGrounded && staSaltando && velocitàVerticale.y < 0)
        {
            staSaltando = false;
            animator.SetBool("InSalto", false);
        }
        Vector3 movimentoTotale = movimento * velocita + velocitàVerticale;
        controller.Move(movimentoTotale * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))     //presssure plate, idea da minecraft
        {
            Debug.Log("FINE");

            if(inventario.ContatoreCollezzionabili < spawnCollezzionabili.numeroMoneteDaSpawnare)
            {
                Debug.Log("Devi prima prendere tutte le monete");
                testoMonete.MostraMessaggioDiPrendereTutteLeMonete();   //semplice alert che dice la roba del debug
                return;
            }

            Cursor.lockState = CursorLockMode.None;
            ArrivatoAllaFine?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            testoMonete.MostraMessaggioContatoreMonete();   //messaggio "normale" che dovrebbe apparrire sempre
        }
    }
}
