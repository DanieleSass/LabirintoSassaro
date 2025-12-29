using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public event EventHandler ArrivatoAllaFine;

    float gravita = -9.81f;
    float velocita = 5f;
    float altezzaSalto = 2f;

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
        float x = Input.GetAxis("Horizontal");  //può essere positivo o negativo a seconda se destra o sinistra
        float z = Input.GetAxis("Vertical");

        Vector3 movimento = transform.right * x+transform.forward * z;
        controller.Move(movimento * velocita * Time.deltaTime); //deltatime così è compatibile con performance di qualsiasi pc/console

        //if(Input.GetButtonDown("Jump") && controller.isGrounded)
        //{
        //    float velocitaSalto = Mathf.Sqrt(altezzaSalto * -2f * gravita);
        //    movimento.y = velocitaSalto;
        //}

        float X = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up * X);   //ruota il player, quindi anche le videocamere

        
        float speed = new Vector3(x, 0, z).magnitude;
        animator.SetFloat("Velocita", speed);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            Debug.Log("FINE");
            Cursor.lockState = CursorLockMode.None;
            ArrivatoAllaFine?.Invoke(this, EventArgs.Empty);
        }
    }
}
