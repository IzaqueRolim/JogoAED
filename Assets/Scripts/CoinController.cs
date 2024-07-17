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
       
    }
    void Update()
    {
        int qtdMoedas = PlayerPrefs.GetInt("moedas");
        text.text = qtdMoedas.ToString();
    }
}
