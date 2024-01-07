using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegurarItens : MonoBehaviour
{
    public float raioDetecao = 1.0f;

    public Transform esteira;

    public BubbleSort bubbleSort;

    void Update()
    {
        DetectarObjetosNaFrente();
    }

    void DetectarObjetosNaFrente()
    {
        Vector2 posicaoAtual = transform.position + transform.up;
        Vector2 direcaoForward = transform.position + transform.up + transform.up;

        RaycastHit2D hit = Physics2D.Raycast(posicaoAtual, direcaoForward, raioDetecao);


        // Se identificar algum objeto no raycast
        if (hit.collider != null)
        {
            // Se o objeto identificado tiver a tag "box"
            if (hit.collider.tag.ToLower().Contains("box"))
            {
                // Se o usuario apertar Space ou A no joystick
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton1))
                {
                    // Se o transform tiver apenas um filho, ou seja, não esteja segurando a caixa
                    if (this.transform.childCount == 1)
                    {
                        // Ajusta a posição e seta a caixa como filho
                        hit.transform.position = new Vector2(this.transform.position.x, hit.transform.position.y);
                        hit.transform.parent = this.transform;
                    }
                    else
                    {
                        float DistanciaEsteiraCaixa = esteira.position.y - hit.transform.position.y;

                        if(DistanciaEsteiraCaixa < 1f)
                        {
                            hit.collider.transform.parent = null;

                            float posX = 0;
                            // Se a quantidade de elementos criados for impar
                            if(bubbleSort.elementos.Count % 2 != 0)
                            {
                                posX = Mathf.Round(transform.position.x);
                            }
                            else
                            {
                                posX = Mathf.Round(transform.position.x * 2.0f) / 2.0f;
                            }

                            hit.transform.position = new Vector3(posX,esteira.position.y,0);
                        }
                    }
                }
            }
        }

       
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + transform.right, raioDetecao);
    }
}