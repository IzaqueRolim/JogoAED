using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegurarItens : MonoBehaviour
{
    public float raioDetecao = 1.0f;

    public Transform esteira;

    void Update()
    {
        DetectarObjetosNaFrente();
    }

    void DetectarObjetosNaFrente()
    {
        Vector2 posicaoAtual = transform.position + transform.up;
        Vector2 direcaoForward = transform.position + transform.up + transform.up;

        RaycastHit2D hit = Physics2D.Raycast(posicaoAtual, direcaoForward, raioDetecao);

        if (hit.collider != null)
        {
            if (hit.collider.tag.ToLower().Contains("box"))
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton1))
                {
                    if (this.transform.childCount == 1)
                    {
                        hit.transform.position = new Vector2(this.transform.position.x, hit.transform.position.y);
                        hit.transform.parent = this.transform;
                    }
                    else
                    {
                            Debug.Log( esteira.position.y- hit.transform.position.y);
                        if(esteira.position.y - hit.transform.position.y < 1f)
                        {
                            hit.collider.transform.parent = null;
                            hit.transform.position = new Vector3(transform.position.x,esteira.position.y,0);
                        }
                    }
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