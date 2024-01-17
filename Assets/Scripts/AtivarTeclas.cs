using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AtivarTeclas : MonoBehaviour
{
    public GameObject tecla;
    public Sprite teclaE, buttonA,cadeado;

    bool joystickConectado = false;
    bool estaPertoDaCasa = false;

    public string algoritmoOrdenador;
    

    void Update()
    {
       VerificarObjetoAFrente();
       VerificaarJoystick();    
       EntrarNaCasa();
    }
    void EntrarNaCasa(){
        if((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton0)) && estaPertoDaCasa){
            SceneManager.LoadScene(algoritmoOrdenador);
            this.transform.position = Vector3.zero;
        }
    }

    void VerificarObjetoAFrente()
    {
        Vector2 posicaoAtual = transform.position + transform.up;
        Vector2 direcaoForward = transform.position + transform.up + transform.up;

        RaycastHit2D hit = Physics2D.Raycast(posicaoAtual, direcaoForward, 2f);

        if (hit.collider != null)
        {

            if (hit.collider.tag.ToLower().Contains("house"))
            {
                algoritmoOrdenador = hit.collider.tag.Replace("house", "");
                if (joystickConectado)
                {
                    tecla.GetComponent<Image>().sprite = buttonA;
                }
                else
                {
                    tecla.GetComponent<Image>().sprite = teclaE;
                }
                tecla.SetActive(true);
                estaPertoDaCasa = true;
            }

            if (hit.collider.name.ToLower().Contains("bloqueado"))
            {
                tecla.GetComponent<Image>().sprite = cadeado;
                estaPertoDaCasa = false;
            }
           
        }
        else { tecla.SetActive(false); }
    }

    void VerificaarJoystick()
    {
        int joystickCount = Input.GetJoystickNames().Length;

       // Debug.Log(Input.IsJoystickPreconfigured(""));

        joystickConectado = joystickCount > 0;
       
    }

   
}