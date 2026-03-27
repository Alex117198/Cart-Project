using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public int currentLane = 1; // Carril actual (0 = izquierdo, 1 = central, 2 = derecho)
    public int position = 0; // Posición lógica en el eje Z (progreso en el tablero)
    // Variables de movimiento
    public float laneOffset = 2f; // Distancia entre carriles en el eje X
    public float trackOffset = 0f; // Este offset es la coordenada en X que se tiene para el jugador y la IA
    public float forwardStep = 2f; // Cuánto avanza por unidad en el eje Z
    public bool isPlayer = false; // Al momento de asignar el script, marcarlo si se asigna a jugador
    public float moveSpeed = 5f; // velocidad de suavizado
    private Vector3 targetPosition; // posición objetivo

    void Start()
    {
        // Inicializa la posición objetivo al inicio
        targetPosition = transform.position;
    }

    void Update()
    {
        // Esto es para diferenciar el jugador de la IA al momento de realizar los movimientos
        // evitando que podamos mover la IA con A y D
        if (isPlayer)
        {
            // Permite cambiar de carril con las teclas A y D
            if (Keyboard.current.aKey.wasPressedThisFrame)
            {
                ChangeLane(-1); // Mover a la izquierda
            }

            if (Keyboard.current.dKey.wasPressedThisFrame)
            {
                ChangeLane(1); // Mover a la derecha
            }
        }

        // 🔥 Movimiento suave SIEMPRE
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
    }

    // Moverse hacia adelante
    public void MoveForward(int steps)
    {
        position += steps; // Aumenta la posición lógica
        UpdatePosition();  // Actualiza la posición en el mundo
    }

    // Moverse hacia atrás
    public void MoveBackward(int steps)
    {
        position -= steps;
        // Evita que la posición sea negativa
        if (position < 0) position = 0;
        UpdatePosition();
    }

    // Cambio de carril
    public void ChangeLane(int direction)
    {
        currentLane += direction;
        // Limita a que sean solo 3 carriles
        currentLane = Mathf.Clamp(currentLane, 0, 2);
        UpdatePosition();
    }

    // Te posiciona en un carril
    public void SetLane(int lane)
    {
        currentLane = Mathf.Clamp(lane, 0, 2);
        UpdatePosition();
    }

    // Actualiza la posición en Unity
    void UpdatePosition()
    {
        Vector3 pos = transform.position;

        // Carriles en el eje X
        // (currentLane - 1) centra el carril 1 en 0
        pos.x = trackOffset + (currentLane - 1) * laneOffset;

        // Avanze dado en el eje Z
        pos.z = position * forwardStep;

        // Evita teletransportaciones al moverse
        targetPosition = new Vector3(pos.x, transform.position.y, pos.z);
    }
}