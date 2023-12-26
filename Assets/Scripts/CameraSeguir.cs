using UnityEngine;

public class CameraSeguir : MonoBehaviour
{
    public Transform alvo;  // O transform do seu personagem
    public float suavidade = 1.0f;  // A suavidade do movimento da câmera

    void Update()
    {
        if (alvo != null)
        {
            
            Vector3 posicaoDesejada = new Vector3(alvo.position.x, alvo.position.y, transform.position.z);

            
            transform.position = Vector3.Lerp(transform.position, posicaoDesejada, suavidade * Time.fixedDeltaTime);
        }
    }
}
