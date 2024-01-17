using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReoganizarCaixas : MonoBehaviour
{
    public bool irParaADireita = false, estaDentroNaEsteira = false;

    float destino;

    float tempo;


    private void Start()
    {
        
    }

    void Update()
    {        
        if(irParaADireita)
        {
            if (transform.parent.position.x < destino)
            {
                Vector3 Destino = new Vector3(destino, transform.parent.position.y, 0);
                transform.parent.position = Vector2.MoveTowards(transform.parent.position, Destino, Time.deltaTime);
               
                return;
            }
            irParaADireita=false;
            
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        irParaADireita = false;
       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "box" && !collision.name.ToLower().Contains("fora") && collision.name.ToLower().Contains("pegou"))
        {
            collision.name += "fora"; 
            if(collision.transform.parent != null)
            {
                irParaADireita = true;
                destino = transform.parent.position.x + 1;
            }
        }
    }
}
