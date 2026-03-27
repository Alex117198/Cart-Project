using UnityEngine;
// Se crea un enum para las posibles acciones dentro del juego
public enum DiceAction
{
    None, // No pasa nada (turno perdido)
    Move, // Solo avanzar
    Attack, // Solo atacar
    MoveAndAttack // Avanzar y atacar
}

public class DiceSystem : MonoBehaviour
{
    // Lanzar dado
    public DiceAction RollDice(out int value)
    {
        // Genera un número entre 1 y 20
        value = Random.Range(1, 21);
        // Casos en los que se pierde el turno
        if (value == 1 || value == 20)
            return DiceAction.None;
        // Casos en los que te puedes mover
        if (value >= 2 && value <= 5)
            return DiceAction.Move;
        // Casos en los que puedes atacar
        if (value >= 6 && value <= 9)
            return DiceAction.Attack;
        // Casos en los que puedes moverte y atacar (si, las dos)
        if (value >= 10 && value <= 11)
            return DiceAction.MoveAndAttack;
        // Casos en los que puedes atacar
        if (value >= 12 && value <= 15)
            return DiceAction.Attack;
        // Casos en los que te puedes mover v2
        if (value >= 16 && value <= 19)
            return DiceAction.Move;
        // Aunque en teoria nunca cae aca, lo pongo unicamente para que no se reviente
        return DiceAction.None;
    }
}