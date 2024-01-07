using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


[Serializable]
public class DadosParaSalvar
{
    public List<int> elementos;
    public bool ordenado;
}

public class ListaDados
{
    public List<DadosParaSalvar> listaDeDados;
}


public class BubbleSort : MonoBehaviour
{
    public GameObject Caixa;

    public List<int> elementos = new List<int>();


    void Start()
    {
        //RecuperarJson();

        
        if (PrecisaCriarArray())
        {
            CriarArray();
        }
        else
        {
            BuscarElementosNoJson();
        }
    }


    // Esta funcao cria uma lista de tamanho aleatorio e com numeros aleatorios
    // Depois de criar, altera o tamanho da esteira(Para caber os blocos certinhos) e instancia as caixas com os numeros criados
    void CriarArray()
    {
        //List<int> elementos = new List<int>();
        int tamanhoLista = Random.RandomRange(5, 15);

        

        for(int i = 0;i < tamanhoLista;i++)
        {
            elementos.Add(Random.Range(1,100));
        }

        GuardarJson(elementos);

        RenderizarListaNasCaixas(elementos, tamanhoLista);
    }

    void RenderizarListaNasCaixas(List<int> elementos,int tamanhoLista)
    {
        // Sempre cabe o tamanho da lista -1 na esteira, entao ela tem que ser +1 pra caber as caixas
        this.transform.localScale = new Vector3(tamanhoLista + 1, 1, 1);

        int count = 0;

        // Percorre a lista "elementos" para criar os objetos
        foreach (var elemento in elementos)
        {
            // Posicionar as caixas de acordo com o tamanho da lista
            float posX = tamanhoLista % 2 == 0 ? -(tamanhoLista) / 2 + count + 0.5f : -(tamanhoLista / 2) + count;
            Vector3 posicaoCaixa = new Vector3(posX, transform.position.y, 0);

            GameObject caixaInstanciada = Instantiate(Caixa, posicaoCaixa, Quaternion.identity);

            // O Objeto caixa tem um filho(canvas) que tem um filho(texto) 
            caixaInstanciada.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = elemento.ToString();
            count++;
        }
    }

    void BuscarElementosNoJson()
    {
        ListaDados listaDados = RecuperarJson();

        foreach(var lista in listaDados.listaDeDados)
        {
            if (!lista.ordenado)
            {
                RenderizarListaNasCaixas(lista.elementos, lista.elementos.Count);
                elementos = lista.elementos;
            }
        }
    }

    void GuardarJson(List<int> elementos)
    {
        // Cria um objeto da classe de dados para guardar os elementos do array e se o elemento está ordenado
        DadosParaSalvar dados = new DadosParaSalvar
        {
            elementos = elementos,
            ordenado = false
        };

        // Busca os dados na memoria e adiciona na classe ListaDados
        ListaDados listaDados = RecuperarJson();
        listaDados.listaDeDados.Add(dados);

        // Convertendo dados para uma string JSON
        string dadosJSON = JsonUtility.ToJson(listaDados);

        // Salvando a string JSON no PlayerPrefs
        PlayerPrefs.SetString("MeusDados", dadosJSON);
        PlayerPrefs.Save();
    }

    ListaDados RecuperarJson()
    {
        string dadosRecuperadosJSON = PlayerPrefs.GetString("MeusDados", "");

        if (dadosRecuperadosJSON != "")
        {
            // Convertendo a string JSON de volta para os dados
            ListaDados dadosRecuperados = JsonUtility.FromJson<ListaDados>(dadosRecuperadosJSON);

            return dadosRecuperados;
        }

        ListaDados listaDados = new ListaDados();
        listaDados.listaDeDados = new List<DadosParaSalvar>();
        return listaDados;
    }

    bool PrecisaCriarArray()
    {
        // Busca na memoria os dados
        string dadosRecuperadosJSON = PlayerPrefs.GetString("MeusDados", "");

        if(dadosRecuperadosJSON == "")
        {
            return true;
        }

        // Converte a string na classe ListaDados
        ListaDados dadosRecuperados = JsonUtility.FromJson<ListaDados>(dadosRecuperadosJSON);


        // Percorre a lista de dado e verifica se algum é t
        foreach (DadosParaSalvar dado in dadosRecuperados.listaDeDados)
        {
            // Se existir algum dado nao ordenado, ele retorna falso, ou seja, nao precisa criar o array
            if (!dado.ordenado)
            {
                return false;
            }
        }

        return true;

    }

    
}
