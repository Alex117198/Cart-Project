using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Referencias principales del juego
    public DiceSystem dice; // Sistema de dado
    public UIManager uiPlayer; // UI del jugador 1
    public UIManager uiEnemy; // UI del jugador 2
    public PlayerController player; // Jugador
    public PlayerController enemy; // IA o enemigo
    public AIController ai; // Lógica de la IA
    public GameObject bombPrefab;
    public Transform bombSpawnPoint;
    public AudioClip bombSFX;
    

    // Variables de control del flujo del juego
    private bool playerTurn = true; // Indica si es turno del jugador o IA
    private bool waitingForAttackInput = false; // Espera para que el jugador elija carril
    private DiceAction currentAction; // Acción actual del dado
    private int lastRoll; // Último valor del dado
    private bool gameOver = false; // Controla si el juego terminó
    
    void Start()
    {
        // Inicializa UI
        uiPlayer.SetTurn(playerTurn);
        uiEnemy.SetTurn(playerTurn);
    }

    void Update()
    {
        // Evita que el juego continúe si ya se tiene un ganador
        if (gameOver) 
        {
            cargarGameOverScene();
            return;
        }

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
        // Mostrar turno en UI
        uiPlayer.SetTurn(playerTurn);
        uiEnemy.SetTurn(playerTurn);

        // Lanza el dado y obtiene acción
        int roll;
        DiceAction action = dice.RollDice(out roll);

        lastRoll = roll;
        currentAction = action;

        // Mostrar dado
        uiPlayer.SetDice(roll);
        uiEnemy.SetDice(roll);

        // Mostrar acción base
        uiPlayer.SetAction("Acción: " + action);
        uiEnemy.SetAction("Acción: " + action);

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
                uiPlayer.SetAction("\n\n\nELIGE CARRIL A ATACAR:\n\n 1=Izquierda \n2=Centro \n3=Derecha");
                uiEnemy.SetAction("Jugador eligiendo ataque...");
                waitingForAttackInput = true;
            }
            // Solo moverse
            else if (action == DiceAction.Move)
            {
                player.MoveForward(2);
                uiPlayer.SetAction("Avanzas");
                uiEnemy.SetAction("Jugador avanza");
                EndTurn();
            }
        }
        // Turno de la IA
        else
        {
            HandleAITurn(action);
        }
    }

    // Comportamiento de la IA según la acción obtenida del dado
    void HandleAITurn(DiceAction action)
    {
        uiPlayer.SetAction("IA ejecuta: " + action);
        uiEnemy.SetAction("IA ejecuta: " + action);

        if (action == DiceAction.Move)
        {
            uiPlayer.SetAction("IA avanza");
            uiEnemy.SetAction("Avanzas");
            enemy.MoveForward(2);
            EndTurn();
        }
        else if (action == DiceAction.Attack)
        {
            int lane = ai.ChooseLane(); // IA elige carril de ataque
            uiPlayer.SetAction("IA ataca carril: " + (lane + 1));
            uiEnemy.SetAction("Atacas carril: " + (lane + 1));
            LaunchBomb(enemy, player, lane);
        }
        else if (action == DiceAction.MoveAndAttack)
        {
            uiPlayer.SetAction("IA avanza y ataca");
            uiEnemy.SetAction("Avanzas y atacas");
            enemy.MoveForward(2);

            int lane = ai.ChooseLane();
            uiPlayer.SetAction("IA ataca carril: " + (lane + 1));
            uiEnemy.SetAction("Atacas carril: " + (lane + 1));
            LaunchBomb(enemy, player, lane);
        }
    }

    // Decide el resultado del ataque entre atacante y defensor según el carril elegido por el atacante y la defensa de la IA
    void ResolveAttack(PlayerController attacker, PlayerController defender, int attackLane)
    {
        // La IA decide en qué carril defenderse
        int defenseLane = ai.ChooseDefenseLane(defender.currentLane);
        defender.SetLane(defenseLane);

        // Muestra el carril de ataque y defensa para que el jugador pueda ver lo que hizo la IA
        uiPlayer.SetAction("Ataque: " + (attackLane + 1) + " | Defensa: " + (defenseLane + 1));
        uiEnemy.SetAction("Ataque: " + (attackLane + 1) + " | Defensa: " + (defenseLane + 1));

        // Si coincide carril se obtiene un golpe exitoso
        if (attackLane == defenseLane)
        {
            defender.MoveBackward(2);
            uiPlayer.SetAction("Ataque acertado");
            uiEnemy.SetAction("Ataque acertado");
        }
        else
        {
            // Si falla el atacante puede avanzar un paso como penalización por no acertar
            attacker.MoveForward(1);
            uiPlayer.SetAction("Ataque fallido, atacante avanza");
            uiEnemy.SetAction("Ataque fallido, atacante avanza");
        }
    }

    // Verifica si hay un ganador después de cada turno y detiene el juego si es así
    void CheckWin()
    {
        if (gameOver) return;

        if (player.position >= 30)
        {
            uiPlayer.ShowWinner("Jugador 1 ha ganado");
            uiEnemy.ShowWinner("Jugador 1 ha ganado");
            gameOver = true; // Detiene el juego
        }
        else if (enemy.position >= 30)
        {
            uiPlayer.ShowWinner("Jugador 2 ha ganado");
            uiEnemy.ShowWinner("Jugador 2 ha ganado");
            gameOver = true;
        }
    }

    // Maneja la elección del carril de ataque por parte del jugador y resuelve el ataque
    void PlayerChooseLane(int lane)
    {
        uiPlayer.SetAction("Atacas carril: " + (lane + 1));
        uiEnemy.SetAction("Jugador ataca carril: " + (lane + 1));

        waitingForAttackInput = false;

        LaunchBomb(player, enemy, lane);
    }

    // Termina el turno e inicia el siguiente, también verifica si hay un ganador después de cada turno
    void EndTurn()
    {
        CheckWin(); // Verifica victoria
        playerTurn = !playerTurn; // Cambia turno

        uiPlayer.SetTurn(playerTurn);
        uiEnemy.SetTurn(playerTurn);
    }

    void LaunchBomb(PlayerController attacker, PlayerController defender, int attackLane)
    {
        // Posición destino según carril
        Vector3 targetPos = defender.transform.position;
        targetPos.x = defender.trackOffset + (attackLane - 1) * defender.laneOffset;

        // Instanciar bomba
        GameObject bomb = Instantiate(bombPrefab, bombSpawnPoint.position, Quaternion.identity);
        BombProjectile projectile = bomb.GetComponent<BombProjectile>();
                     

        projectile.Init(targetPos, () =>
        {
            ResolveAttack(attacker, defender, attackLane);
            AudioManager.Instance.PlaySFX(bombSFX, 0.2f);
            EndTurn(); // El turno termina al impactar
        });
    }

    void cargarGameOverScene()
    {

        SceneManager.LoadScene(0);
        AudioManager.Instance.StopMusic();
    }
}