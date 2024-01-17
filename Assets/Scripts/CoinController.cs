using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinController : MonoBehaviour
{
    Text text;
    void Start()
    {
        text = GetComponent<Text>();
        float qtdMoedas = PlayerPrefs.GetFloat("moedas");
        text.text = qtdMoedas.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
