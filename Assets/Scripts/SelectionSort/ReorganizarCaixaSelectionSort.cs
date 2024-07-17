using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReorganizarCaixaSelectionSort : MonoBehaviour
{
    public bool irParaADireita = false, estaDentroNaEsteira = false;
    public bool podeIrParaDireita = true;

    float destino;

    float velocidade = 3f;


    void Update()
    {
        if (this.transform.parent.gameObject.name.Contains("pegou"))
        {
            this.irParaADireita = false;
        }
        else if (irParaADireita && podeIrParaDireita && !this.transform.parent.gameObject.name.Contains("pegou"))
        {
            Vector3 Destino = new Vector3(destino, transform.parent.position.y, 0);
            if (transform.parent.position.x < destino)
            {
                transform.parent.position = Vector2.MoveTowards(transform.parent.position, Destino, 3 * Time.deltaTime);

                // Se a distância restante for menor que a velocidade * deltaTime, ajuste a posição diretamente para o destino
                if (Vector3.Distance(transform.parent.position, Destino) < 3 * Time.deltaTime)
                {
                    transform.parent.position = Destino;
                }
            }
            else
            {
                transform.parent.position = Destino;
                this.irParaADireita = false;
            }


        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "box" && !this.transform.parent.gameObject.name.Contains("pegou"))
        {
            Debug.Log(collision.name);
            irParaADireita = true;
            destino = transform.parent.position.x + 1;
            transform.parent.position = new Vector2(destino, transform.parent.position.y);
        }
    }
}
