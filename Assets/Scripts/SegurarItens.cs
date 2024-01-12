using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class SegurarItens : MonoBehaviour
{
    public float raioDetecao = 1.0f;

    public Transform esteira,indicador;

    public BubbleSort bubbleSort;
    //public BubbleSortOrdenacao bubbleSortOrdenacao;

    private float posicaoInicialCaixa;
    public float posicaoCaixaQuePodeSerMovida;

    public float indexGeral;

    private void Start()
    {
        posicaoInicialCaixa = bubbleSort.posicaoInicialCaixa;
        posicaoCaixaQuePodeSerMovida = posicaoInicialCaixa;
        indexGeral = 0;
        //VerificarSePodeSerMovida(bubbleSort.elementos[0], bubbleSort.elementos[1]);
        Analisar(bubbleSort.elementos[0]);
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
                        // Se a caixa for a que pode mexer
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

                            Debug.Log("Elementos movido" + ElementoMovido);
                            Debug.Log("Proximo Elemento" + ProximoElemento);


                            if (index >= bubbleSort.elementos.Count - 1)
                            {
                                posicaoCaixaQuePodeSerMovida = posicaoInicialCaixa;
                                index = 0;
                            }

                            VerificarSePodeSerMovida(bubbleSort.elementos[index], bubbleSort.elementos[index + 1]);

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

    void Analisar(int elemento)
    {
        List<int> elementos = bubbleSort.elementos;
        int index = elementos.IndexOf(elemento);
        Debug.Log(index);
        Debug.Log(elementos[index]);

        int nextIndex = (index + 1) % elementos.Count;

        //   Debug.Log(elementos[nextIndex]);

        if (index >= elementos.Count - 1)
        {
            // Se o próximo índice for o último da lista
            posicaoCaixaQuePodeSerMovida = posicaoInicialCaixa;
            indicador.position = new Vector3(posicaoCaixaQuePodeSerMovida, 9, 0);
            Analisar(elementos[0]);
            return;
        }

        // Verificar se os elementos são iguais
        while (elementos[index] == elementos[nextIndex])
        {
            // Se forem iguais, apenas passar para o próximo índice
            index++;
            nextIndex++;
            posicaoCaixaQuePodeSerMovida++;
        }


        

        if (elementos[nextIndex] < elementos[index])
        {
            posicaoCaixaQuePodeSerMovida++;
            indicador.position = new Vector3(posicaoCaixaQuePodeSerMovida, 9, 0);
            return;
        }
        else
        {
           

            if (nextIndex >= elementos.Count - 1 && elementos[nextIndex] > elementos[index])
            {
                posicaoCaixaQuePodeSerMovida = posicaoInicialCaixa;
                Analisar(elementos[0]);
                return;
            }
            posicaoCaixaQuePodeSerMovida++;
            Analisar(elementos[nextIndex]);
        }
    }



    void AtualizarPosicaoDaCaixa(int elemento, int proximoElemento)
    { 

        // Recebe apenas o elemento e faz a comparacao com o proximo, para nao repetir codigos em outros lugares
        //                 -
        //| 44 | 52 | 54 | 2 | 73 | 39 | 74 | 39 | 74 |
        // Se o proximo elemento for menor que o elemento
        if(proximoElemento < elemento)
        {
            posicaoCaixaQuePodeSerMovida++;
            indicador.position = new Vector3(posicaoCaixaQuePodeSerMovida, 9, 0);
        }
        else
        {
            List<int> elementos = bubbleSort.elementos;
            int indexProx = elementos.IndexOf(proximoElemento);

            if(indexProx >= elementos.Count - 1)
            {
                posicaoCaixaQuePodeSerMovida = posicaoInicialCaixa;
                AtualizarPosicaoDaCaixa(elementos[0], elementos[1]);
                return;
            }
            
            posicaoCaixaQuePodeSerMovida++;
            indicador.position = new Vector3(posicaoCaixaQuePodeSerMovida, 9, 0);
            int depoisDoProximoElemento = elementos[indexProx+1];

            AtualizarPosicaoDaCaixa(proximoElemento, depoisDoProximoElemento);
        }
    }
   

    void VerificarSePodeSerMovida(int elemento, int proximoElemento)
    {
        // Condição de parada
        if (posicaoCaixaQuePodeSerMovida >= bubbleSort.posicaoMaximaCaixa)
        {
            posicaoCaixaQuePodeSerMovida = posicaoInicialCaixa;
            return;  // Saia da função para evitar recursão infinita
        }

        // Condição de movimento
        if (proximoElemento <= elemento)
        {
            posicaoCaixaQuePodeSerMovida++;
            indicador.position = new Vector3(posicaoCaixaQuePodeSerMovida,9,0);
           // Debug.Log("posicao" + posicaoCaixaQuePodeSerMovida);
        }
        else
        {
            int index = bubbleSort.elementos.IndexOf(proximoElemento);
            Debug.Log(index);

            // Condição para o último elemento
            if (index >= bubbleSort.elementos.Count - 1)
            {
                posicaoCaixaQuePodeSerMovida = posicaoInicialCaixa;
                VerificarSePodeSerMovida(bubbleSort.elementos[0], bubbleSort.elementos[1]);
                return;
            }

            posicaoCaixaQuePodeSerMovida++;
            indicador.position = new Vector3(posicaoCaixaQuePodeSerMovida, 9, 0);
            VerificarSePodeSerMovida(proximoElemento, bubbleSort.elementos[index + 1]);
        }
    }

}