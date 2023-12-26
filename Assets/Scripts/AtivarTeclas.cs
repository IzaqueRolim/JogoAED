using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AtivarTeclas : MonoBehaviour
{
    public GameObject tecla;

    public Sprite teclaE, buttonA;

    bool joystickConectado = false;

    void Start()
    {
        
    }

    void Update()
    {
       VerificarObjetoAFrente();
       VerificaarJoystick();    
    }

    void VerificarObjetoAFrente()
    {
        Vector2 posicaoAtual = transform.position + transform.up;
        Vector2 direcaoForward = transform.position + transform.up + transform.up;

        RaycastHit2D hit = Physics2D.Raycast(posicaoAtual, direcaoForward, 2f);

        if (hit.collider != null)
        {
            if (hit.collider.tag == "house")
            {
                if (joystickConectado)
                {
                    tecla.GetComponent<Image>().sprite = buttonA;
                    tecla.SetActive(true);
                }
                else
                {
                    tecla.GetComponent<Image>().sprite = teclaE;
                    tecla.SetActive(true);
                }
            }
        }
        else { tecla.SetActive(false); }
    }

    void VerificaarJoystick()
    {
        int joystickCount = Input.GetJoystickNames().Length;

        joystickConectado = joystickCount > 0;
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "house")
        {
            tecla.SetActive(true);
        }
    }
}