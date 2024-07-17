using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BubbleSortOrdenacao : MonoBehaviour
{
    public Transform indicador;

    public BubbleSort bubbleSort;

    private float posicaoInicialCaixa;
    public float posicaoCaixaQuePodeSerMovida;
    public int quantidadeDeCaixasMovidas;
    void Start()
    {
        posicaoInicialCaixa = bubbleSort.posicaoInicialCaixa;
        posicaoCaixaQuePodeSerMovida = posicaoInicialCaixa;
        indicador.position = new Vector3(posicaoCaixaQuePodeSerMovida, transform.position.y, 0);
        VerificarSePodeSerMovida(bubbleSort.elementos[0], bubbleSort.elementos[1]);
        quantidadeDeCaixasMovidas = 0;
    }


    public void LargarCaixaEVerificarSeEhMenor(Transform hit)
    {
        int ElementoMovido = int.Parse(hit.GetChild(0).GetChild(0).GetComponent<Text>().text);

        int ProximoElemento = bubbleSort.ReordenarArray(ElementoMovido);

        int index = bubbleSort.elementos.IndexOf(ProximoElemento);

      
        if (index >= bubbleSort.elementos.Count - 1)
        {
            posicaoCaixaQuePodeSerMovida = posicaoInicialCaixa;
            index = 0;
        }
      
        VerificarSePodeSerMovida(bubbleSort.elementos[index], bubbleSort.elementos[index + 1]);
    }


    public void VerificarSePodeSerMovida(int elemento, int proximoElemento)
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
            Debug.Log("posicao" + posicaoCaixaQuePodeSerMovida);
            indicador.position = new Vector3(posicaoCaixaQuePodeSerMovida-0.5f, 8, 0);
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

                // Chama recursivamente a função para o próximo elemento na lista (considerando ciclo)
                VerificarSePodeSerMovida(bubbleSort.elementos[0], bubbleSort.elementos[1]);
                return;
            }

            posicaoCaixaQuePodeSerMovida++;
            indicador.position = new Vector3(posicaoCaixaQuePodeSerMovida, 8, 0);

            // Chama recursivamente a função para o próximo elemento na lista
            VerificarSePodeSerMovida(proximoElemento, bubbleSort.elementos[index + 1]);
        }
    }
}
