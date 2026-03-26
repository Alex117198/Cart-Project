using UnityEngine;
using UnityEngine.SceneManagement; // Librería vital para cargar niveles

public class MenuManager : MonoBehaviour
{
    public void Start_Race()
    {
        SceneManager.LoadScene("Alex Scene"); 
    }
}