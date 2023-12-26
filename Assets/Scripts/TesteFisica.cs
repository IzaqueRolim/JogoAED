using UnityEngine;

public class TesteFisica : MonoBehaviour
{
    public float[] speeds; // Velocidades para cada marcha
    public float acceleration = 2f;
    public float rotationSpeed = 200f;

    [Header("Informações de velocidade")]
    public int marcha = 0;
    public float currentSpeed;

    public float velocidade;

    void Start()
    {
        currentSpeed = speeds[marcha];
    }

    void Update()
    {
        // Obter a entrada do jogador
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");


        if (Input.GetKeyUp(KeyCode.S))
        {
            velocidade -= acceleration * 5 * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W))
        {
            velocidade += acceleration * 2 * Time.deltaTime;
        }
        else
        {
            if (velocidade <= 0){
                velocidade = 0;
                return;
            }
            velocidade -= acceleration * Time.deltaTime;
        }

        transform.Translate(new Vector3(0, velocidade));

        float rotation = -horizontalInput * rotationSpeed * Time.deltaTime;

        if(verticalInput!= 0)
        {
            transform.Rotate(Vector3.forward * rotation);
        }
    }
}
