using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class AnimacaoEscrever : MonoBehaviour
{

    public Text textComponent;
    public Color targetColor;
    public float animationSpeed = 1.0f;

    private string originalText;
    private Color[] originalColors;
    private Color[] targetColors;
    private float timer = 0.0f;

    void Start()
    {
        // Salva o texto original e suas cores originais
        textComponent = GetComponent<Text>();
        originalText = textComponent.text;
        originalColors = new Color[originalText.Length];
        for (int i = 0; i < originalText.Length; i++)
        {
            originalColors[i] = textComponent.color;
        }

        // Define as cores alvo para todas as letras
        targetColors = new Color[originalText.Length];
        for (int i = 0; i < targetColors.Length; i++)
        {
            targetColors[i] = targetColor;
        }

        // Inicializa o texto com a cor branca
        textComponent.text = "<color=#FFFFFF>" + originalText + "</color>";
    }

    void Update()
    {
        timer += Time.deltaTime * animationSpeed;

        // Atualiza a cor de cada letra gradualmente
        for (int i = 0; i < originalText.Length; i++)
        {
            Color lerpedColor = Color.Lerp(originalColors[i], targetColors[i], timer);
            textComponent.text = textComponent.text.Replace(originalText[i].ToString(), "<color=#" + ColorToHex(lerpedColor) + ">" + originalText[i] + "</color>");
        }
    }

    // Converte uma cor para uma string hexadecimal
    private string ColorToHex(Color color)
    {
        Color32 color32 = color;
        string hex = color32.r.ToString("X2") + color32.g.ToString("X2") + color32.b.ToString("X2");
        return hex;
    }
}
