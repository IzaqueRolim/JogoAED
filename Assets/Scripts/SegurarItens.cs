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

    private float posicaoInicialCaixa;
    public float posicaoCaixaQuePodeSerMovida;

    float indicadorPosicao;

    public bool IniciouLista = false;

    private void Start()
    {
        posicaoInicialCaixa = bubbleSort.posicaoInicialCaixa;
        posicaoCaixaQuePodeSerMovida = posicaoInicialCaixa;
        indicadorPosicao = posicaoInicialCaixa;
        indicador.position = new Vector3(indicadorPosicao + 0.5f, 8, 0);

        IniciouLista = false;
    }


    void Update()
    {
        if(bubbleSort.elementos.Count>0 && !IniciouLista)
        {
            VerificarSePodeSerMovida(bubbleSort.elementos[0], bubbleSort.elementos[1]);
            IniciouLista=true;
        }
        DetectarObjetosNaFrente();
        MoverLuz();
    }


    void MoverLuz()
    {
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            if (indicadorPosicao < bubbleSort.posicaoMaximaCaixa && indicadorPosicao!=posicaoCaixaQuePodeSerMovida)
            {
                indicadorPosicao++;
                indicador.position = new Vector3(indicadorPosicao - 0.5f, 8, 0);
            }

            else if(indicadorPosicao == bubbleSort.posicaoMaximaCaixa)
            {
                indicadorPosicao = posicaoInicialCaixa;
            }
        }
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
                            hit.collider.name += "pegou";
                        }
                    }
                    else
                    {
                        float DistanciaEsteiraCaixa = esteira.position.y - hit.transform.position.y;

                        if(DistanciaEsteiraCaixa < 1f)
                        {
                            string name = hit.collider.name;
                            Debug.Log("Nome Antes: " + name); // Exibindo o nome antes da modificação

                            // Remover as palavras "fora" e "pegou"
                            name = name.Replace("fora", string.Empty);
                            name = name.Replace("pegou", string.Empty);

                            // Atribuir o novo nome ao collider
                            hit.collider.name = name;
                            hit.collider.transform.parent = null;


                            int ElementoMovido = int.Parse(hit.collider.transform.GetChild(0).GetChild(0).GetComponent<Text>().text);

                            int ProximoElemento = bubbleSort.ReordenarArray(ElementoMovido);

                            int index = bubbleSort.elementos.IndexOf(ProximoElemento);

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
                            hit.transform.rotation = Quaternion.Euler(0,0,0);
                            
                        }
                    }
                }                
            }
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
        }
        else
        {
            if (bubbleSort.VerificarSeListaEstaOrdenada())
            {
                posicaoCaixaQuePodeSerMovida = -100;
                return;
            }

            int index = bubbleSort.elementos.IndexOf(proximoElemento);

            // Condição para o último elemento
            if (index >= bubbleSort.elementos.Count - 1)
            {
                posicaoCaixaQuePodeSerMovida = posicaoInicialCaixa;
                VerificarSePodeSerMovida(bubbleSort.elementos[0], bubbleSort.elementos[1]);
                return;
            }

            posicaoCaixaQuePodeSerMovida++;
          //  indicador.position = new Vector3(posicaoCaixaQuePodeSerMovida-0.5f, 8, 0);
            VerificarSePodeSerMovida(proximoElemento, bubbleSort.elementos[index + 1]);
        }
    }

}