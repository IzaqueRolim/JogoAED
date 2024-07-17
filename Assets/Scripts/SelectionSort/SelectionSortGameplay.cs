using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class SelectionSortGameplay : MonoBehaviour
{
    public float raioDetecao = 1.0f;

    private float posicaoInicialCaixa;
    public float posicaoCaixaQuePodeSerMovida;

    public float posicaoMenorElemento;

    float contadorElementoJaOrdenado;

    GameObject esteira;
    public SelectionSort selectionSort;
    public Transform lanterna;
    public GameObject painelGanhou;
    public Text anotacaoMenorElemento;
    public GameObject luzGrande;


    public bool pegouACaixa = false;
    public bool percorreuTudo = false;
    void Start()
    {
        painelGanhou = GameObject.FindGameObjectWithTag("PainelGanhou");
        painelGanhou.SetActive(false);
        esteira = GameObject.FindGameObjectWithTag("esteira");
        lanterna.position = new Vector2(selectionSort.posicaoInicialCaixa, lanterna.position.y);
        pegouACaixa = false;
        contadorElementoJaOrdenado = selectionSort.posicaoInicialCaixa;
        luzGrande.SetActive(false);


        // definir o primeiro elemento como o menor
        Transform caixaEncontrada = ProcurarObjetoPorPosicao(selectionSort.posicaoInicialCaixa).transform;
        posicaoMenorElemento = caixaEncontrada.position.x;
        anotacaoMenorElemento.text = caixaEncontrada.GetChild(0).GetChild(0).GetComponent<Text>().text;
    }

    void Update()
    {
        DetectarObjetosNaFrente();
        AtualizarPosicaoLanterna();
        GuardarMenor();
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

                    if (!percorreuTudo)
                    {
                        return;
                    }

                    // Se o transform tiver apenas um filho, ou seja, não esteja segurando a caixa
                    if (this.transform.childCount == 1 )
                    {
                        // Ajusta a posição e seta a caixa como filho
                        hit.transform.position = new Vector2(this.transform.position.x, hit.transform.position.y);
                        hit.transform.parent = this.transform;
                        hit.collider.name += "pegou";

                        pegouACaixa = true;
                        lanterna.position = new Vector2(contadorElementoJaOrdenado, lanterna.position.y);
                        posicaoCaixaQuePodeSerMovida = selectionSort.elementos.Count % 2 == 0 ? ((float)Mathf.Round(hit.collider.transform.position.x * 2) / 2) - 1f : Mathf.Round(hit.collider.transform.position.x) - 1f;
                    }
                    else
                    {
                        float DistanciaEsteiraCaixa = esteira.transform.position.y - hit.transform.position.y;
                        pegouACaixa = false;
                        percorreuTudo = false;
                        contadorElementoJaOrdenado++;
                        lanterna.position = new Vector2(contadorElementoJaOrdenado, lanterna.position.y);
                        Transform caixaEncontrada = ProcurarObjetoPorPosicao(contadorElementoJaOrdenado).transform;
                        posicaoMenorElemento = caixaEncontrada.position.x;
                        anotacaoMenorElemento.text = caixaEncontrada.GetChild(0).GetChild(0).GetComponent<Text>().text;


                        if (contadorElementoJaOrdenado == selectionSort.posicaoMaximaCaixa)
                        {
                            painelGanhou.SetActive(true);
                            selectionSort.OrdenarLista();
                            luzGrande.SetActive(true);
                        }

                        if (DistanciaEsteiraCaixa < 1f)
                        {
                            string name = hit.collider.name;

                            // Atribuir o novo nome ao collider
                            hit.collider.name = name;
                            hit.collider.transform.parent = null;

                            float posX = selectionSort.elementos.Count % 2 == 0 ? (float)Mathf.Round(hit.collider.transform.position.x * 2) / 2 : Mathf.Round(hit.collider.transform.position.x);

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
            if (percorreuTudo)
            {
                return;
            }
            else if (lanterna.position.x == selectionSort.posicaoMaximaCaixa && !pegouACaixa && !percorreuTudo)
            {
                lanterna.position = new Vector2(posicaoMenorElemento, lanterna.position.y);
                percorreuTudo = true;
            }
            else
            {
                lanterna.position = new Vector2(lanterna.position.x + 1, lanterna.position.y);
            }
        }
    }


    void GuardarMenor()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            float PosX = selectionSort.elementos.Count % 2 == 0 ? ((float)Mathf.Round(lanterna.position.x * 2) / 2) : Mathf.Round(lanterna.position.x);
            Transform caixaEncontrada = ProcurarObjetoPorPosicao(PosX).transform;
            posicaoMenorElemento = caixaEncontrada.position.x;
            anotacaoMenorElemento.text = caixaEncontrada.GetChild(0).GetChild(0).GetComponent<Text>().text;
        }
    }

    public GameObject ProcurarObjetoPorPosicao(float posicaoDesejadaX)
    {
        GameObject[] objetosNaCena = GameObject.FindGameObjectsWithTag("box");

        foreach (GameObject objeto in objetosNaCena)
        {
            if (posicaoDesejadaX == objeto.transform.position.x)
            {
                return objeto;
            }
        }

        // Se nenhum objeto for encontrado com a posição desejada, retornamos null
        return null;
    }
}
