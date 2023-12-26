using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegurarItens : MonoBehaviour
{
    public float raioDetecao = 1.0f;
    public LayerMask mascaraObjetos;
    public GameObject tecla;
    

    void Update()
    {
        DetectarObjetosNaFrente();
    }

    void DetectarObjetosNaFrente()
    {

        

        Vector2 posicaoFrente = transform.position + transform.right;

        Collider2D[] objetosDetectados = Physics2D.OverlapCircleAll(posicaoFrente, raioDetecao, mascaraObjetos);

        foreach (Collider2D objeto in objetosDetectados)
        {
            
            if(objeto.gameObject.tag == "box")
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (this.transform.childCount == 0)
                    {
                        objeto.transform.SetParent(this.transform);
                        objeto.transform.position = new Vector2(this.transform.position.x,objeto.transform.position.y);
                        return;
                    }
                    objeto.transform.SetParent(null);
                    
                }
               

            }
           
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + transform.right, raioDetecao);
    }
}