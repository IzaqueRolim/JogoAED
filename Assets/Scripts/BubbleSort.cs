using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BubbleSort : MonoBehaviour
{
    public GameObject Caixa;
    public List<int> elementos = new List<int>();

    void Start()
    {
        CriarArray();
    }


    // Esta funcao cria uma lista de tamanho aleatorio e com numeros aleatorios
    // Depois de criar, altera o tamanho da esteira(Para caber os blocos certinhos) e instancia as caixas com os numeros criados
    void CriarArray()
    {
        int tamanhoLista = Random.RandomRange(5, 15);

        // Sempre cabe o tamanho da lista -1 na esteira, entao ela tem que ser +1 pra caber as caixas
        this.transform.localScale = new Vector3(tamanhoLista+1,1,1);

        for(int i = 0;i < tamanhoLista;i++)
        {
            elementos.Add(Random.Range(1,100));
        }

        int count = 0;
        // Percorre a lista "elementos" para criar os objetos
        foreach (var elemento in elementos)
        {
            float posX = tamanhoLista % 2 == 0 ? -tamanhoLista/2 + count+0.5f : -(tamanhoLista / 2) + count;
            Vector3 posicaoCaixa = new Vector3( posX, transform.position.y, 0);

            GameObject caixaInstanciada = Instantiate(Caixa,posicaoCaixa,Quaternion.identity);

            // O Objeto caixa tem um filho(canvas) que tem um filho(texto) 
            caixaInstanciada.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = elemento.ToString();
            count++;
        } 

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
