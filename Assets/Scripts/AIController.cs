using UnityEngine;

public class AIController : MonoBehaviour
{
    // Al atacar elige un carril al azar.
    public int ChooseLane()
    {
        return Random.Range(0, 3); // Devuelve 0, 1 o 2
    }

    // Aca hay un chance de 1/2 de que acierte el carril del jugador
    public int ChooseLaneSmart(int playerLane)
    {
        // 50% de probabilidad de predecir al jugador
        if (Random.value > 0.5f)
            return playerLane; // intenta atacar donde está el jugador
        // 50% el restante es aleatorio
        return Random.Range(0, 3);
    }

    // Selección de carril para esquivar
    //
    public int ChooseDefenseLane(int currentLane)
    {
        // Chance de quedarse en el mismo carril (50% en este caso)
        if (Random.value > 0.5f)
            return currentLane;

        // Chance de moverse a un carril cercano (-1, 0 o +1) es de 50% 
        return Mathf.Clamp(currentLane + Random.Range(-1, 2), 0, 2);
    }
}