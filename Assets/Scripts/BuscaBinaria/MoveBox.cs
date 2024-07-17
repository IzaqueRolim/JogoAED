using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBox : MonoBehaviour
{
    private Vector3 targetPosition;
    private Vector3 initialPosition;
    private float duration;
    private float startTime;
    private float fadeDuration = 0.5f; // Duração do desaparecimento (em segundos)
    private SpriteRenderer spriteRenderer;

    public void Setup(Vector3 targetPosition, Vector3 initialPosition, float duration)
    {
        this.targetPosition = targetPosition;
        this.initialPosition = initialPosition;
        this.duration = duration;
        this.startTime = Time.time;
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float t = (Time.time - startTime) / duration;
        transform.position = Vector3.Lerp(initialPosition, targetPosition, t);

        if (t >= 1.0f)
        {
            // A movimentação foi concluída, ajusta a posição para evitar valores quebrados
            transform.position = targetPosition;

            // Inicia a animação de desaparecimento
            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeOut()
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            SetAlpha(alpha);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Garante que a transparência seja definida corretamente no final
        SetAlpha(1f);

        // Remove este componente
        Destroy(this.gameObject);
    }

    void SetAlpha(float alpha)
    {
        Color spriteColor = spriteRenderer.color;
        spriteColor = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);
        spriteRenderer.color = spriteColor;
    }
}
