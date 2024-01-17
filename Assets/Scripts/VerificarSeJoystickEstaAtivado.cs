using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VerificarSeJoystickEstaAtivado : MonoBehaviour
{
    public GameObject TeclaEntrarESairDasCasas,TeclaPegarCaixa,TeclaMoverLuz,TeclaCorrer;
    public Sprite teclaE,teclaQ,teclaSpace,teclaLeftShift,buttonA,buttonB,buttonX,buttonLB;

    bool joystickConectado;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        VerificarJoystick();
    }

    void VerificarJoystick()
    {
        int joystickCount = Input.GetJoystickNames().Length;

        joystickConectado = joystickCount > 0;

        if (joystickConectado)
        {
            TeclaEntrarESairDasCasas.GetComponent<Image>().sprite = buttonA;
            TeclaPegarCaixa.GetComponent<Image>().sprite = buttonB;
            TeclaMoverLuz.GetComponent<Image>().sprite = buttonX;
            TeclaCorrer.GetComponent<Image>().sprite = buttonLB;
        }
        else
        {
            TeclaEntrarESairDasCasas.GetComponent<Image>().sprite = teclaE;
            TeclaPegarCaixa.GetComponent<Image>().sprite = teclaSpace;
            TeclaMoverLuz.GetComponent <Image>().sprite = teclaQ;
            TeclaCorrer.GetComponent<Image>().sprite = teclaLeftShift;
        }

    }
}
