using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SegurarItens : MonoBehaviour
{
    public float raioDetecao = 1.0f;

    public Transform esteira;

    public BubbleSort bubbleSort;

    private float posicaoInicialCaixa;
    public float posicaoCaixaQuePodeSerMovida;

    int elementoInicial;


    private void Start()
    {
        posicaoInicialCaixa = bubbleSort.posicaoInicialCaixa + 1;
        posicaoCaixaQuePodeSerMovida = posicaoInicialCaixa;
        elementoInicial = 0;
    }


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
                    if (this.transform.childCount == 1 )
                    {
                        if(hit.transform.position.x == posicaoCaixaQuePodeSerMovida)
                        {
                            // Ajusta a posição e seta a caixa como filho
                            hit.transform.position = new Vector2(this.transform.position.x, hit.transform.position.y);
                            hit.transform.parent = this.transform;
                        }
                    }
                    else
                    {
                        float DistanciaEsteiraCaixa = esteira.position.y - hit.transform.position.y;

                        if(DistanciaEsteiraCaixa < 1f)
                        {
                            hit.collider.transform.parent = null;

                            int ElementoMovido = int.Parse(hit.collider.transform.GetChild(0).GetChild(0).GetComponent<Text>().text);

                            int ProximoElemento = bubbleSort.ReordenarArray(ElementoMovido);

                            int index = bubbleSort.elementos.IndexOf(ProximoElemento);

                            if(elementoInicial==0)
                            {
                                elementoInicial = bubbleSort.elementos[index];
                            }

                            VerificarSePodeSerMovida(bubbleSort.elementos[index], bubbleSort.elementos[index+1]);

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

    void VerificarSePodeSerMovida(int elemento, int proximoElemento)
    {
        // Verifica se a posição da caixa que pode ser movida ultrapassou a posição máxima
        if (posicaoCaixaQuePodeSerMovida >= bubbleSort.posicaoMaximaCaixa)
        {
            // Reinicia a posição da caixa que pode ser movida
            posicaoCaixaQuePodeSerMovida = posicaoInicialCaixa;
        }

        // Verifica se o próximo elemento é menor que o elemento atual
        if (proximoElemento < elemento)
        {
            posicaoCaixaQuePodeSerMovida++;
        }
        else
        {
            // Encontra o índice do próximo elemento na lista
            int index = bubbleSort.elementos.IndexOf(proximoElemento);

            // Verifica se o índice é o último da lista
            if (index >= bubbleSort.elementos.Count - 1)
            {
                // Reinicia a posição da caixa que pode ser movida
                posicaoCaixaQuePodeSerMovida = posicaoInicialCaixa;
                index = bubbleSort.elementos.IndexOf(elementoInicial);

                // Chama recursivamente a função para o próximo elemento na lista (considerando ciclo)
                VerificarSePodeSerMovida(bubbleSort.elementos[0], bubbleSort.elementos[1]);
                return;
            }

            posicaoCaixaQuePodeSerMovida++;

            // Chama recursivamente a função para o próximo elemento na lista
            VerificarSePodeSerMovida(proximoElemento, bubbleSort.elementos[index + 1]);
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + transform.right, raioDetecao);
    }
}