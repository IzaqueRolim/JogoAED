using NUnit.Framework;
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

    public float posicaoInicialCaixa, posicaoMaximaCaixa;

    public GameObject painelGanhou;

    public Transform lanterna;

    public Text casos, tempo;

    private void Start()
    {
        painelGanhou = GameObject.FindGameObjectWithTag("PainelGanhou");
        painelGanhou.SetActive(false);
    }

    void Awake()
    {
        
        if (PrecisaCriarArray())
        {
            CriarArray(false);
        }
        else
        {
            BuscarElementosNoJson();
        }
    }


    // Esta funcao cria uma lista de tamanho aleatorio e com numeros aleatorios
    // Depois de criar, altera o tamanho da esteira(Para caber os blocos certinhos) e instancia as caixas com os numeros criados
    void CriarArray(bool estaOrdenado)
    {
        //List<int> elementos = new List<int>();
        int tamanhoLista = Random.Range(5, 15);

        for (int i = 0; i < tamanhoLista; i++)
        {
            int novoElemento;

            // Garantir que o novo elemento não esteja na lista
            do
            {
                novoElemento = Random.Range(1, 100);
            } while (elementos.Contains(novoElemento));

            elementos.Add(novoElemento);
        }


        GuardarJson(elementos, estaOrdenado);

        RenderizarListaNasCaixas(elementos, tamanhoLista);

      //  BuscarElementosNoJson();
    }

    void RenderizarListaNasCaixas(List<int> elementos,int tamanhoLista)
    {
        // Sempre cabe o tamanho da lista -1 na esteira, entao ela tem que ser +1 pra caber as caixas
        this.transform.localScale = new Vector3(tamanhoLista + 1, 1, 1);

        int count = 0;


        // Guarda a posicao da primeira caixa com o calculo(-tamanhoDaLista/2 + 0.5 se for par, e -tamanhoDaLista/2 se for impar), exemplo:
        // TamanhoDaLista = 14 -> -14/2 + 0.5 = -7 + 0.5 = 6.5
        // TamanhoDaLista = 13 -> -13/2 = 6.5f
        posicaoInicialCaixa = tamanhoLista % 2 == 0 ? -(tamanhoLista) / 2 + 0.5f : -(tamanhoLista / 2);
        posicaoMaximaCaixa = tamanhoLista % 2 == 0 ? -(tamanhoLista) / 2 + 0.5f + elementos.Count-1 : -(tamanhoLista / 2) + elementos.Count-1;

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

    void SetarListaComoOrdenada()
    {
        string dadosRecuperadosJSON = PlayerPrefs.GetString("MeusDadosBubbleSort", "");

        // Converte a string na classe ListaDados
        ListaDados dadosRecuperados = JsonUtility.FromJson<ListaDados>(dadosRecuperadosJSON);

        // Lista para armazenar os dados ordenados
        List<DadosParaSalvar> dadosOrdenados = new List<DadosParaSalvar>();

        // Percorre a lista de dados e verifica se algum não está ordenado
        foreach (DadosParaSalvar dado in dadosRecuperados.listaDeDados)
        {
            // Se o dado não estiver ordenado, marca como ordenado e adiciona à lista de dados ordenados
            if (!dado.ordenado)
            {
                dado.ordenado = true;
                dado.elementos = elementos;  // Certifique-se de definir 'elementos' antes desta linha

                dadosOrdenados.Add(dado);
            }
        }

        // Adiciona os dados ordenados à lista original (se houver dados ordenados)
        if (dadosOrdenados.Count > 0)
        {
            dadosRecuperados.listaDeDados.AddRange(dadosOrdenados);

            // Converte a lista de volta para JSON
            string dadosJSON = JsonUtility.ToJson(dadosRecuperados);

            // Salvando a string JSON no PlayerPrefs "DadosOrdenados"
            PlayerPrefs.SetString("DadosOrdenados", dadosJSON);
            PlayerPrefs.SetString("MeusDadosBubbleSort", dadosJSON);
            PlayerPrefs.Save();  
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

    void GuardarJson(List<int> elementos, bool estaOrdenado)
    {
        // Cria um objeto da classe de dados para guardar os elementos do array e se o elemento está ordenado
        DadosParaSalvar dados = new DadosParaSalvar
        {
            elementos = elementos,
            ordenado = estaOrdenado
        };

        // Busca os dados na memoria e adiciona na classe ListaDados
        ListaDados listaDados = RecuperarJson();
        listaDados.listaDeDados.Add(dados);

        // Convertendo dados para uma string JSON
        string dadosJSON = JsonUtility.ToJson(listaDados);

        // Salvando a string JSON no PlayerPrefs
        PlayerPrefs.SetString("MeusDadosBubbleSort", dadosJSON);
        PlayerPrefs.Save();
    }

    ListaDados RecuperarJson()
    {
        string dadosRecuperadosJSON = PlayerPrefs.GetString("MeusDadosBubbleSort", "");

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
        string dadosRecuperadosJSON = PlayerPrefs.GetString("MeusDadosBubbleSort", "");

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

    public int ReordenarArray(int elementoASerBuscado)
    {
        int indiceElemento = elementos.IndexOf(elementoASerBuscado);
        TrocarElementosNaLista(elementos,indiceElemento,indiceElemento-1);

        // Se a lista está ordenada, exibe uma mensagem de parabens e manda essa lista pra casa de ordenação
        if (VerificarSeListaEstaOrdenada())
        {
          
            SetarListaComoOrdenada();
            lanterna.localScale = new Vector2(elementos.Count,lanterna.localScale.y);
            lanterna.position = new Vector2(0, lanterna.position.y) ;
            painelGanhou.SetActive(true);
        }
        else
        {
            Debug.Log("Ainda nao ordenou");
        }

        return elementos[indiceElemento];
    }




    public void SomarMoedas()
    {
        int qtdMoedas = PlayerPrefs.GetInt("moedas");
        PlayerPrefs.SetInt("moedas", qtdMoedas + 100);
    }

    void TrocarElementosNaLista<T>(List<T> lista, int indiceA, int indiceB)
    {
        // Verificar se os índices são válidos
        if (indiceA < 0 || indiceA >= lista.Count || indiceB < 0 || indiceB >= lista.Count)
        {
            Debug.LogError("Índices inválidos.");
            return;
        }

        // Trocar os elementos nas posições especificadas
        T temp = lista[indiceA];
        lista[indiceA] = lista[indiceB];
        lista[indiceB] = temp;
    }


    public bool VerificarSeListaEstaOrdenada()
    {
        if (elementos.Count <= 1)
        {
            return true;
        }

        // Percorre a lista e verifica se cada elemento é menor ou igual ao próximo
        for (int i = 0; i < elementos.Count - 1; i++)
        {
            if (elementos[i].CompareTo(elementos[i + 1]) > 0)
            {
                return false; // Lista não está ordenada
            }
        }

        return true; 
    }
}
