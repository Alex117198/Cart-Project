using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Start_Race : MonoBehaviour
{
    public GameObject panelMainMenu;
    public GameObject panelTutorial;

    public AudioSource sfxArranque; 

    public void MostrarTutorial()
    {
        panelMainMenu.SetActive(false); // Apagamos el menú
        panelTutorial.SetActive(true);  // Encendemos el tutorial
    }


    public void IniciarCarrera()
    {
        StartCoroutine(SecuenciaArranque());
    }

    private IEnumerator SecuenciaArranque()
    {
        if (sfxArranque != null)
        {
            sfxArranque.Play();
        }
        
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Pablo Scene"); 
    }
}