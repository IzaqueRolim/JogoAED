using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VitoriaController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
       
        if (other.gameObject.CompareTag("box"))
        {
           // other.transform.parent = null;
            Destroy(other.gameObject);
            int moedas = PlayerPrefs.GetInt("moedas");
            moedas += 200;
            PlayerPrefs.SetInt("moedas", moedas);
        }
    }

    
}
