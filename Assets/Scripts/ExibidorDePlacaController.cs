using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExibidorDePlacaController : MonoBehaviour
{

    public string ordenador;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(PlayerPrefs.GetInt(ordenador));
        if(PlayerPrefs.GetInt(ordenador) == 1)
        {
            this.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
