using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public event EventHandler ArrivatoAllaFine;

    float gravita = -9.81f;
    float velocita = 5f;
    float altezzaSalto = 1;

    Vector3 velocit‡Verticale;
    bool staSaltando;

    CharacterController controller;
    Animator animator;

                //HO VOLUTO PROVARE A COLLEGARE DA SOLO ANIMAZIONI, SENZA PRENDERE PERSONAGGI CON TUTTA LA GESTIONE GIA' COMPLETA


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller= GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();


    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");  //puÚ essere positivo o negativo a seconda se destra o sinistra
        float z = Input.GetAxis("Vertical");

        //movimento
        Vector3 movimento = transform.right * x+transform.forward * z;
        //controller.Move(movimento * velocita * Time.deltaTime); //deltatime cosÏ Ë compatibile con performance di qualsiasi pc/console


        //visuale grafica
        float X = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up * X);   //ruota il player, quindi anche le videocamere

        //animazione corsa
        float speed = new Vector3(x, 0, z).magnitude;
        animator.SetFloat("Velocita", speed);

        //salto
        if (controller.isGrounded)
        {
            if (!staSaltando)
                velocit‡Verticale.y = -1;

            if (Input.GetButtonDown("Jump"))
            {
                float velocit‡Salto = Mathf.Sqrt(altezzaSalto * -2 * gravita);
                velocit‡Verticale.y = velocit‡Salto;
                staSaltando = true;
                animator.SetBool("InSalto", true);
            }
        }
        else
            velocit‡Verticale.y += gravita * Time.deltaTime;

        //atterraggio
        if(controller.isGrounded && staSaltando && velocit‡Verticale.y < 0)
        {
            staSaltando = false;
            animator.SetBool("InSalto", false);
        }
        Vector3 movimentoTotale = movimento * velocita + velocit‡Verticale;
        controller.Move(movimentoTotale * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))     //presssure plate
        {
            Debug.Log("FINE");
            Cursor.lockState = CursorLockMode.None;
            ArrivatoAllaFine?.Invoke(this, EventArgs.Empty);
        }
    }
}
