using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Referencias UI")]
    public TextMeshProUGUI turnText;
    public TextMeshProUGUI actionText;
    public TextMeshProUGUI diceText;
    public TextMeshProUGUI resultText;

    [Header("Configuración")]
    public bool isPlayerUI; // Si es el jugador se activa.

    [Header("Colores")]
    public Color playerTurnColor = Color.green;
    public Color enemyTurnColor = Color.red;

    void Awake()
    {
        AutoAssignUI(); // 🔥 Auto conecta los textos
    }

    void AutoAssignUI()
    {
        // Buscar automáticamente los textos por nombre dentro de la jerarquía

        // Dice -> DiceText
        if (diceText == null)
        {
            Transform dice = transform.Find("Dice/DiceText");
            if (dice != null)
                diceText = dice.GetComponent<TextMeshProUGUI>();
        }

        // Action -> ActionText
        if (actionText == null)
        {
            Transform action = transform.Find("Action/ActionText");
            if (action != null)
                actionText = action.GetComponent<TextMeshProUGUI>();
        }

        // Turn -> TurnText
        if (turnText == null)
        {
            Transform turn = transform.Find("Turn/TurnText");
            if (turn != null)
                turnText = turn.GetComponent<TextMeshProUGUI>();
        }

        // Result -> ResultText
        if (resultText == null)
        {
            Transform result = transform.Find("Result/ResultText");
            if (result != null)
                resultText = result.GetComponent<TextMeshProUGUI>();
        }

        // Validación para debug
        if (turnText == null) Debug.LogError("TurnText no encontrado en " + gameObject.name);
        if (actionText == null) Debug.LogError("ActionText no encontrado en " + gameObject.name);
        if (diceText == null) Debug.LogError("DiceText no encontrado en " + gameObject.name);
        if (resultText == null) Debug.LogError("ResultText no encontrado en " + gameObject.name);
    }

    // Mostrar turno
    public void SetTurn(bool isPlayerTurn)
    {
        // Si este UI pertenece al jugador
        if (isPlayerUI)
        {
            if (isPlayerTurn)
            {
                turnText.text = "Tu turno";
                turnText.color = playerTurnColor;
            }
            else
            {
                turnText.text = "Espera tu turno";
                turnText.color = enemyTurnColor;
            }
        }
        // Si este UI pertenece al enemigo (vista invertida)
        else
        {
            if (!isPlayerTurn)
            {
                turnText.text = "Tu turno";
                turnText.color = playerTurnColor;
            }
            else
            {
                turnText.text = "Espera tu turno";
                turnText.color = enemyTurnColor;
            }
        }
    }

    // Mostrar número del dado
    public void SetDice(int value)
    {
        if (diceText != null)
            diceText.text = "Dado: " + value;
    }

    // Mostrar acción actual
    public void SetAction(string message)
    {
        if (actionText != null)
            actionText.text = message;
    }

    // Mostrar ganador
    public void ShowWinner(string message)
    {
        if (resultText != null)
        {
            resultText.text = message;
            resultText.gameObject.SetActive(true);
        }
    }

    // Limpiar mensajes
    public void ClearAction()
    {
        if (actionText != null)
            actionText.text = "";
    }

    // Ocultar resultado
    public void HideWinner()
    {
        if (resultText != null)
            resultText.gameObject.SetActive(false);
    }

    // Inicialización
    void Start()
    {
        // Asegura que el texto de ganador esté oculto al inicio
        if (resultText != null)
            resultText.gameObject.SetActive(false);
    }
}