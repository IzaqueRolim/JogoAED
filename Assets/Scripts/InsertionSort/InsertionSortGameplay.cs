using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InsertionSortGameplay : MonoBehaviour
{

    public float raioDetecao = 1.0f;

    private float posicaoInicialCaixa;
    public float posicaoCaixaQuePodeSerMovida;

    GameObject esteira;
    public InsertionSort insertionSort;
    public Transform lanterna;
    public GameObject painelGanhou;
    public GameObject luzGrande;

    bool pegouACaixa = false;

    public int quantidadeDeCaixasMovidas;
    void Start()
    {
        painelGanhou = GameObject.FindGameObjectWithTag("PainelGanhou");
        painelGanhou.SetActive(false);
        esteira = GameObject.FindGameObjectWithTag("esteira");
        lanterna.position = new Vector2(insertionSort.posicaoInicialCaixa,lanterna.position.y);
        luzGrande.SetActive(false);
        pegouACaixa = false;
        quantidadeDeCaixasMovidas = 0;
    }

    void Update()
    {
        DetectarObjetosNaFrente();
        AtualizarPosicaoLanterna();
        MoverCaixaParaADireita();
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
                        hit.collider.name += "pegou";

                        pegouACaixa = true;
                        posicaoCaixaQuePodeSerMovida = insertionSort.elementos.Count % 2 == 0 ? ((float)Mathf.Round(hit.collider.transform.position.x * 2) / 2)-1f : Mathf.Round(hit.collider.transform.position.x)-1f;

                        if (hit.transform.position.x == posicaoCaixaQuePodeSerMovida)
                        {
                        }
                    }
                    else
                    {
                        float DistanciaEsteiraCaixa = esteira.transform.position.y - hit.transform.position.y;
                        pegouACaixa = false;
                        quantidadeDeCaixasMovidas++;

                        if (insertionSort.listaOrdenada)
                        {
                            painelGanhou.SetActive(true);
                            luzGrande.SetActive(true);  
                        }

                        if (DistanciaEsteiraCaixa < 1f)
                        {
                            string name = hit.collider.name;
                            Debug.Log("Nome Antes: " + name); // Exibindo o nome antes da modificação

                            // Remover as palavras "fora" e "pegou"
                            name = name.Replace("fora", string.Empty);
                            name = name.Replace("pegou", string.Empty);

                            // Atribuir o novo nome ao collider
                            hit.collider.name = name;
                            hit.collider.transform.parent = null;

                            float posX = insertionSort.elementos.Count % 2 == 0 ? (float)Mathf.Round(hit.collider.transform.position.x * 2) / 2 : Mathf.Round(hit.collider.transform.position.x);

                            hit.collider.transform.position = new Vector2(posX, esteira.transform.position.y);
                            hit.collider.transform.rotation = Quaternion.Euler(0, 0, 0);

                        } 
                    }
                }
            }
        }
    }

    void AtualizarPosicaoLanterna()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // se a lanterna ta no final
            if(lanterna.position.x+1 == insertionSort.posicaoMaximaCaixa && !pegouACaixa)
            {
                lanterna.position = new Vector2(insertionSort.posicaoInicialCaixa, lanterna.position.y);
            }
            // se a lanterna esta no inicio
            else if(lanterna.position.x == insertionSort.posicaoInicialCaixa && !pegouACaixa)
            {
                lanterna.position = new Vector2(lanterna.position.x + 1, lanterna.position.y);
            }
            else if(lanterna.position.x == insertionSort.posicaoInicialCaixa && pegouACaixa)
            {
                return; 
            }
            else if(pegouACaixa)
            {
                lanterna.position = new Vector2(lanterna.position.x - 1, lanterna.position.y);

                if (posicaoCaixaQuePodeSerMovida == insertionSort.posicaoInicialCaixa - 1f)
                {
                    return;
                }

                Transform caixaEncontrada = ProcurarObjetoPorPosicao(Mathf.Round(transform.position.x)).transform;
                caixaEncontrada.position = new Vector2(caixaEncontrada.position.x + 1, caixaEncontrada.position.y);

                posicaoCaixaQuePodeSerMovida--;
                Debug.Log(posicaoCaixaQuePodeSerMovida);

                string elemento = caixaEncontrada.GetChild(0).GetChild(0).GetComponent<Text>().text;
                insertionSort.ReordenarArray(int.Parse(elemento));
            }
            else
            {
                lanterna.position = new Vector2(lanterna.position.x + 1, lanterna.position.y);  
            }
        }
    }


    void MoverCaixaParaADireita()
    {
        if(Input.GetKeyDown(KeyCode.E) && pegouACaixa)
        {
            if (posicaoCaixaQuePodeSerMovida-(insertionSort.posicaoInicialCaixa - 1f) < 0.5f)
            {
                return;
            }
            Transform caixaEncontrada = ProcurarObjetoPorPosicao(Mathf.Round(transform.position.x)).transform;
            caixaEncontrada.position = new Vector2(caixaEncontrada.position.x + 1, caixaEncontrada.position.y);
           
            posicaoCaixaQuePodeSerMovida--;
            Debug.Log(posicaoCaixaQuePodeSerMovida);


            string elemento = caixaEncontrada.GetChild(0).GetChild(0).GetComponent<Text>().text;
            insertionSort.ReordenarArray(int.Parse(elemento));
    

        }
    }

    public GameObject ProcurarObjetoPorPosicao(float posicaoDesejadaX)
    {
        GameObject[] objetosNaCena = GameObject.FindGameObjectsWithTag("box");

        foreach (GameObject objeto in objetosNaCena)
        {
            if (posicaoCaixaQuePodeSerMovida==objeto.transform.position.x)
            {
                return objeto;
            }
        }

        // Se nenhum objeto for encontrado com a posição desejada, retornamos null
        return null;
    }
}
