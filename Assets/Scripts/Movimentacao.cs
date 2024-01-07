    using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Movimentacao : MonoBehaviour
{
    [Header("Defina a velocidade")]
    public float velocidade;

    void Start()
    {
        velocidade = 5;
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        transform.position += Vector3.right * horizontal * Time.deltaTime * velocidade;
        transform.position += Vector3.up * vertical * Time.deltaTime * velocidade;



        // Logica para correr
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            velocidade = 10;
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            velocidade = 5;
        }





        // Logica para rotacionar o personagem
        if(horizontal!=0 || vertical !=0)
        {
            Vector3 posLook = transform.position + new Vector3(horizontal,vertical,0);
            Vector3 lookDirection = posLook - transform.position;
            lookDirection.z = 0;
        
            transform.up = lookDirection.normalized;
        }

    }
}
