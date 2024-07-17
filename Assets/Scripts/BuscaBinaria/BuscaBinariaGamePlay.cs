using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class BuscaBinariaGamePlay : MonoBehaviour
{
    private float raioDetecao;
    public GameObject panelGanhouBuscaBinaria,entrega;
    private Transform seta;
    private Text textNumeroASerBuscado;

    private BuscaBinaria buscaBinaria;
    private int ElementoASerBuscado;
    private bool podeSegurarItem;
    private float posicaoIndicadorBuscaBinaria;

    int tamanhoLista;

    private bool podeDescartarMetadeDaLista;
    void Start()
    {
        podeDescartarMetadeDaLista = false;
        podeSegurarItem = false;

        buscaBinaria          = GameObject.FindObjectOfType<BuscaBinaria>();
        seta                  = GameObject.FindGameObjectWithTag("seta").GetComponent<Transform>();
        textNumeroASerBuscado = GameObject.FindGameObjectWithTag("numeroASerBuscado").GetComponent<Text>();
        panelGanhouBuscaBinaria = GameObject.FindGameObjectWithTag("PainelGanhou");
        panelGanhouBuscaBinaria.SetActive(false);
        entrega = GameObject.FindGameObjectWithTag("entrega");
        entrega.SetActive(false);

        ElementoASerBuscado = buscaBinaria.ElementoASerBuscado;
        textNumeroASerBuscado.text = ElementoASerBuscado.ToString();

        EncontrarCaixaNoMeio();
    }

   
    void Update()
    {
        DetectarObjetosAFrente();
        DescartarMetadeDaLista();
        seta.position = new Vector2(posicaoIndicadorBuscaBinaria, seta.position.y);
    }

    void EncontrarCaixaNoMeio()
    {
        tamanhoLista = buscaBinaria.listaBuscaBinaria.Count;
        // Se o tamanho for par, o meio é 0.5f, se for impar, é 0
        posicaoIndicadorBuscaBinaria = tamanhoLista % 2 == 0 ? 0.5f : 0;
    }

    void DescartarMetadeDaLista()
    {
        if(podeDescartarMetadeDaLista && Input.GetKeyDown(KeyCode.Alpha1))
        {
            buscaBinaria.DescartarMetadeDaLista(Direcao.Direita);
            podeDescartarMetadeDaLista = false;
            EncontrarCaixaNoMeio();
            posicaoIndicadorBuscaBinaria = tamanhoLista % 2 == 0 ? 0.5f : 0;
            Debug.Log("Descartou a Esquerda"+posicaoIndicadorBuscaBinaria);
        }
        else if (podeDescartarMetadeDaLista && Input.GetKeyDown(KeyCode.Alpha2))
        {
            buscaBinaria.DescartarMetadeDaLista(Direcao.Esquerda);
            podeDescartarMetadeDaLista = false;
            EncontrarCaixaNoMeio();
            posicaoIndicadorBuscaBinaria = tamanhoLista % 2 == 0 ? 0.5f : 0;
            Debug.Log("Descartou a Direita"+posicaoIndicadorBuscaBinaria);
        }
    }


    async void DetectarObjetosAFrente()
    {
        Vector2 posicaoAtual = transform.position + transform.up;
        Vector2 direcaoForward = transform.position + transform.up + transform.up;

        RaycastHit2D hit = Physics2D.Raycast(posicaoAtual, direcaoForward, raioDetecao);

        // Se identificar algum objeto no raycast
        if (hit.collider != null)
        {
            if (hit.collider.tag.ToLower().Contains("box"))
            {
                // Se o usuario apertar Space ou A no joystick
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton1))
                {
                    if (podeSegurarItem)
                    {
                        if (this.transform.childCount == 1)
                        {
                            // Se a caixa for a que pode mexer
                            if (hit.transform.position.x == posicaoIndicadorBuscaBinaria)
                            {
                                // Ajusta a posição e seta a caixa como filho
                                hit.transform.position = new Vector2(this.transform.position.x, hit.transform.position.y);
                                hit.transform.parent = this.transform;
                                hit.collider.name += "pegou";
                            }
                        }
                        else
                        {
                            hit.collider.transform.parent = null;
                        }
                            return;
                    }
                    if(hit.collider.transform.position.x == posicaoIndicadorBuscaBinaria)
                    {
                        await Task.Delay(1000);
                        hit.transform.GetChild(0).GetChild(0).GetComponent<Text>().GetComponent<CanvasRenderer>().SetAlpha(1);
                       
                        int ElementoDaCaixa = int.Parse(hit.collider.transform.GetChild(0).GetChild(0).GetComponent<Text>().text);
                    
                        // Se ele encontrou a caixa com o elemento correto
                        if(ElementoASerBuscado == ElementoDaCaixa)
                        {
                            // ganhou
                            // Aparece a entrega
                            // ativar o podeSegurarItem
                            podeSegurarItem = true;
                            entrega.SetActive(true);
                            PlayerPrefs.SetInt("ElementoASerBuscadoBuscaBinaria", 0);
                            await Task.Delay(500);
                            panelGanhouBuscaBinaria.SetActive(true);
                            return;
                        }
                        podeDescartarMetadeDaLista = true;
                        tamanhoLista = buscaBinaria.listaBuscaBinaria.Count;

                    }
                }
            }
        }
    }
}
