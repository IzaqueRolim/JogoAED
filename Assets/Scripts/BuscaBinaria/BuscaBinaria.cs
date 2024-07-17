
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Direcao
{
    Esquerda,
    Direita
}

public class BuscaBinaria : MonoBehaviour
{
    public List<int> listaBuscaBinaria;
    private List<GameObject> caixasInstanciadas = new List<GameObject>();


    public int ElementoASerBuscado;
    public float posicaoInicialCaixa, posicaoMaximaCaixa;

    public GameObject Caixa;

    void Awake()
    {
        SetarListaOrdenadaDoPlayerPrefs();
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
           // DescartarMetadeDaLista(Direcao.Esquerda);
        }
    }


    void SetarListaOrdenadaDoPlayerPrefs()
    {
        string dadosRecuperadosJSON = PlayerPrefs.GetString("DadosOrdenados", "");

        // Converte a string na classe ListaDados
        ListaDados dadosRecuperados = JsonUtility.FromJson<ListaDados>(dadosRecuperadosJSON);


        foreach (DadosParaSalvar dados in dadosRecuperados.listaDeDados)
        {
            if (dados.ordenado)
            {
                listaBuscaBinaria = dados.elementos;
                ElementoASerBuscado = listaBuscaBinaria[UnityEngine.Random.Range(0, listaBuscaBinaria.Count)];
                break;
            }
        }

        if (listaBuscaBinaria.Count > 0)
        {
            SetarElementoParaBuscarNaLista(false);
            RenderizarCaixas();
        }

    }


    void SetarElementoParaBuscarNaLista(bool querMudar)
    {
        ElementoASerBuscado = PlayerPrefs.GetInt("ElementoASerBuscadoBuscaBinaria");
      
        if (ElementoASerBuscado == 0 || querMudar)
        {
            ElementoASerBuscado = listaBuscaBinaria[UnityEngine.Random.Range(0, listaBuscaBinaria.Count)];
            PlayerPrefs.SetInt("ElementoASerBuscadoBuscaBinaria", ElementoASerBuscado);
        }
    }



    /*void RenderizarCaixas()
    {
        DestruirTodasAsCaixas();
        int tamanhoLista = listaBuscaBinaria.Count;
        this.transform.localScale = new Vector3(tamanhoLista + 1, 1, 1);

        int count = 0;


        // Guarda a posicao da primeira caixa com o calculo(-tamanhoDaLista/2 + 0.5 se for par, e -tamanhoDaLista/2 se for impar), exemplo:
        // TamanhoDaLista = 14 -> -14/2 + 0.5 = -7 + 0.5 = 6.5
        // TamanhoDaLista = 13 -> -13/2 = 6.5f
        posicaoInicialCaixa = tamanhoLista % 2 == 0 ? -(tamanhoLista) / 2 + 0.5f : -(tamanhoLista / 2);
        posicaoMaximaCaixa = tamanhoLista % 2 == 0 ? -(tamanhoLista) / 2 + 0.5f + listaBuscaBinaria.Count - 1 : -(tamanhoLista / 2) + listaBuscaBinaria.Count - 1;

        // Percorre a lista "elementos" para criar os objetos
        foreach (var elemento in listaBuscaBinaria)
        {
            // Posicionar as caixas de acordo com o tamanho da lista
            float posX = tamanhoLista % 2 == 0 ? -(tamanhoLista) / 2 + count + 0.5f : -(tamanhoLista / 2) + count;
            Vector3 posicaoCaixa = new Vector3(posX, transform.position.y, 0);

            GameObject caixaInstanciada = Instantiate(Caixa, posicaoCaixa, Quaternion.identity);

            // O Objeto caixa tem um filho(canvas) que tem um filho(texto) 
            caixaInstanciada.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = elemento.ToString();
            caixaInstanciada.transform.GetChild(0).GetChild(0).GetComponent<Text>().GetComponent<CanvasRenderer>().SetAlpha(0);
            count++;
        }
    }*/


    void RenderizarCaixas()
    {
        DestruirTodasAsCaixas();
        int tamanhoLista = listaBuscaBinaria.Count;
        this.transform.localScale = new Vector3(tamanhoLista + 1, 1, 1);

        int count = 0;

        posicaoInicialCaixa = tamanhoLista % 2 == 0 ? -(tamanhoLista) / 2 + 0.5f : -(tamanhoLista / 2);
        posicaoMaximaCaixa = tamanhoLista % 2 == 0 ? -(tamanhoLista) / 2 + 0.5f + listaBuscaBinaria.Count - 1 : -(tamanhoLista / 2) + listaBuscaBinaria.Count - 1;

        foreach (var elemento in listaBuscaBinaria)
        {
            float posX = tamanhoLista % 2 == 0 ? -(tamanhoLista) / 2 + count + 0.5f : -(tamanhoLista / 2) + count;
            Vector3 posicaoCaixa = new Vector3(posX, transform.position.y, 0);

            GameObject caixaInstanciada = Instantiate(Caixa, posicaoCaixa, Quaternion.identity);
            caixasInstanciadas.Add(caixaInstanciada); // Adiciona a caixa à lista

            //MoveBox moveBox = caixaInstanciada.AddComponent<MoveBox>();
           // moveBox.Setup(posicaoCaixa, new Vector3(-10, transform.position.y, 0), 1.0f);

            caixaInstanciada.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = elemento.ToString();
            caixaInstanciada.transform.GetChild(0).GetChild(0).GetComponent<Text>().GetComponent<CanvasRenderer>().SetAlpha(0f);
            count++;
        }
    }



    void DestruirTodasAsCaixas()
    {
        // Encontrar todos os objetos com a tag especificada
        GameObject[] objetosComTag = GameObject.FindGameObjectsWithTag("box");

        // Destruir cada objeto encontrado
        foreach (GameObject objeto in objetosComTag)
        {
            Destroy(objeto);
        }
    }


    public void DescartarMetadeDaLista(Direcao direcao)
    {
        int tamanhoLista = listaBuscaBinaria.Count;
        int metade = listaBuscaBinaria.Count / 2;
        int inicioRemocao = (direcao == Direcao.Direita) ? 0 : metade;

        List<Vector3> initialPositions = new List<Vector3>();

        for (int i = inicioRemocao; i < inicioRemocao + metade; i++)
        {
            float posX = tamanhoLista % 2 == 0 ? -(tamanhoLista) / 2 + i + 0.5f : -(tamanhoLista / 2) + i;
            Vector3 initialPos = new Vector3(posX, transform.position.y, 0);
            initialPositions.Add(initialPos);
        }

        //StartCoroutine(AnimateFadeOut(initialPositions));

        // Remover da lista
        listaBuscaBinaria.RemoveRange(inicioRemocao, metade);
        RenderizarCaixas();     
    }


}
