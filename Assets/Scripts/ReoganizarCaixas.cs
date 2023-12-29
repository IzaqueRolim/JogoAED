using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReoganizarCaixas : MonoBehaviour
{
    public bool irParaADireita = false;

    float destino;

    void Update()
    {        
        if(irParaADireita)
        {
            if (transform.parent.position.x < destino)
            {
                transform.parent.Translate(new Vector3(Time.deltaTime*2, 0, 0));
                return;
            }
            irParaADireita=false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "box")
        {
            if(collision.transform.parent != null)
            {
                Debug.Log(collision.transform.GetChild(0).GetChild(0).GetComponent<Text>().text);
                irParaADireita = true;
                destino = transform.parent.position.x + 1;
            }
        }
    }
}
