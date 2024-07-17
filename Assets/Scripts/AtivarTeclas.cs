using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class AtivarTeclas : MonoBehaviour
{
    public GameObject tecla;
    public Sprite teclaE, buttonA,cadeado,comprarIcone;

    bool joystickConectado = false;
    bool estaPertoDaCasa = false;
    bool podeComprar = false;

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
                if (PlayerPrefs.GetInt("moedas") >= hit.collider.GetComponent<CasaModel>().preco)
                {
                    if(PlayerPrefs.GetInt(hit.collider.GetComponent<CasaModel>().nome) == 1)
                    {
                        tecla.GetComponent<Image>().sprite = teclaE;
                    }
                    else
                    {
                        tecla.GetComponent<Image>().sprite = comprarIcone;
                        estaPertoDaCasa = false;
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            Debug.Log(hit.collider.GetComponent<CasaModel>().nome);
                            hit.collider.name = "porta";
                            hit.collider.GetComponent<CasaModel>().placa.SetActive(false);

                            int novoSaldo = PlayerPrefs.GetInt("moedas") - hit.collider.GetComponent<CasaModel>().preco;
                            PlayerPrefs.SetInt("moedas", novoSaldo );
                            PlayerPrefs.SetInt(hit.collider.GetComponent<CasaModel>().nome, 1);
                        }
                    }
                }
                else
                {
                    tecla.GetComponent<Image>().sprite = cadeado;
                    estaPertoDaCasa = false;
                }
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

    void DesbloquearCasa()
    {

    }
}