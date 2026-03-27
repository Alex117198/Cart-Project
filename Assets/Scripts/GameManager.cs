using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Referencias principales del juego
    public DiceSystem dice; // Sistema de dado
    public PlayerController player; // Jugador
    public PlayerController enemy; // IA o enemigo
    public AIController ai; // Lógica de la IA

    // Variables de control del flujo del juego
    private bool playerTurn = true; // Indica si es turno del jugador o IA
    private bool waitingForAttackInput = false; // Espera para que el jugador elija carril
    private DiceAction currentAction; // Acción actual del dado
    private int lastRoll; // Último valor del dado
    private bool gameOver = false; // Controla si el juego terminó

    void Update()
    {
        // Evita que el juego continúe si ya se tiene un ganador
        if (gameOver) return;

        // Lanza el dado al presionar E y verifica que no esté esperando input de ataque a la linea
        if (Keyboard.current.eKey.wasPressedThisFrame && !waitingForAttackInput)
        {
            PlayTurn();
        }

        // Seleecciona el carril a atacar
        if (waitingForAttackInput)
        {
            if (Keyboard.current.digit1Key.wasPressedThisFrame)
                PlayerChooseLane(0); // Carril izquierdo

            if (Keyboard.current.digit2Key.wasPressedThisFrame)
                PlayerChooseLane(1); // Carril central

            if (Keyboard.current.digit3Key.wasPressedThisFrame)
                PlayerChooseLane(2); // Carril derecho
        }
    }

    // Metodo principal para ejecutar el turno actual del jugador o la IA
    public void PlayTurn()
    {
        Debug.Log(playerTurn ? "TURNO DEL JUGADOR" : "TURNO DE LA IA");

        // Lanza el dado y obtiene acción
        int roll;
        DiceAction action = dice.RollDice(out roll);

        lastRoll = roll;
        currentAction = action;

        Debug.Log("Dado: " + roll + " → Acción: " + action);

        // Lógica del turno del jugador
        if (playerTurn)
        {
            // Si puede atacar
            if (action == DiceAction.Attack || action == DiceAction.MoveAndAttack)
            {
                // Si cae en la acción mover y atacar, primero se mueve y luego ataca ese es el orden
                if (action == DiceAction.MoveAndAttack)
                    player.MoveForward(2);

                // Muestra mensaje para elegir carril de ataque y muestra las opciones, 
                // tambien espera el input del jugador para elegir el carril a atacar
                Debug.Log("ELIGE CARRIL PARA ATACAR:");
                Debug.Log("Presiona 1 = Izquierda | 2 = Centro | 3 = Derecha");
                waitingForAttackInput = true;
            }
            // Solo moverse
            else if (action == DiceAction.Move)
            {
                player.MoveForward(2);
                EndTurn();
            }
        }
        // Turno de la IA
        else
        {
            HandleAITurn(action);
            EndTurn();
        }
    }

    // Comportamiento de la IA según la acción obtenida del dado
    void HandleAITurn(DiceAction action)
    {
        Debug.Log("IA ejecuta acción: " + action);

        if (action == DiceAction.Move)
        {
            Debug.Log("IA avanza");
            enemy.MoveForward(2);
        }
        else if (action == DiceAction.Attack)
        {
            int lane = ai.ChooseLane(); // IA elige carril de ataque
            Debug.Log("IA ataca al carril: " + (lane + 1));
            ResolveAttack(enemy, player, lane);
        }
        else if (action == DiceAction.MoveAndAttack)
        {
            Debug.Log("IA avanza y ataca");
            enemy.MoveForward(2);

            int lane = ai.ChooseLane();
            Debug.Log("IA ataca al carril: " + (lane + 1));
            ResolveAttack(enemy, player, lane);
        }
    }

    // Decide el resultado del ataque entre atacante y defensor según el carril elegido por el atacante y la defensa de la IA
    void ResolveAttack(PlayerController attacker, PlayerController defender, int attackLane)
    {
        // La IA decide en qué carril defenderse
        int defenseLane = ai.ChooseDefenseLane(defender.currentLane);
        defender.SetLane(defenseLane);
        // Muestra el carril de ataque y defensa para que el jugador pueda ver lo que hizo la IA
        Debug.Log("Ataque al carril: " + (attackLane + 1) + " | Defensa: " + (defenseLane + 1));

        // Si coincide carril se obtiene un golpe exitoso
        if (attackLane == defenseLane)
        {
            defender.MoveBackward(2);
            Debug.Log("Ataque acertado");
        }
        else
        {
            // Si falla el atacante puede avanzar un paso como penalización por no acertar
            attacker.MoveForward(1);
            Debug.Log("Ataque fallido, atacante avanza");
        }
    }

    // Verifica si hay un ganador después de cada turno y detiene el juego si es así
    void CheckWin()
    {
        if (gameOver) return;

        if (player.position >= 30)
        {
            Debug.Log("JUGADOR GANA");
            gameOver = true; // Detiene el juego
        }
        else if (enemy.position >= 30)
        {
            Debug.Log("IA GANA");
            gameOver = true;
        }
    }

    // Maneja la elección del carril de ataque por parte del jugador y resuelve el ataque
    void PlayerChooseLane(int lane)
    {
        Debug.Log("Jugador ataca al carril: " + (lane + 1));

        waitingForAttackInput = false; // No espera más input de ataque

        ResolveAttack(player, enemy, lane);

        EndTurn();
    }

    // Termina el turno e inicia el siguiente, también verifica si hay un ganador después de cada turno
    void EndTurn()
    {
        CheckWin(); // Verifica victoria
        playerTurn = !playerTurn; // Cambia turno
        Debug.Log("CAMBIO DE TURNO");
    }
}