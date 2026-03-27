using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Start_Race : MonoBehaviour
{
    [Header("Paneles de Interfaz")]
    public GameObject panelMainMenu;
    public GameObject panelTutorial;

    [Header("Efectos de Sonido")]
    public AudioSource sfxMenuStart; 
    public AudioSource sfxRazaContinue;
    public AudioSource sfxRetry;

    [Header("Configuración de Escenas")]
    public string EscenaJuego = "Pablo Scene";
    public string EscenaMenu = "Menu_Scene";


    public void IrALTutorial()
    {
        if (sfxMenuStart != null)
        {
            sfxMenuStart.Play(); 
        }

        if(panelMainMenu != null) panelMainMenu.SetActive(false);
        if(panelTutorial != null) panelTutorial.SetActive(true);
    }

    public void Empezar_Carrera()
    {
        StartCoroutine(IniciarCarrera());
    }

    private IEnumerator IniciarCarrera()
    {
        if (sfxRazaContinue != null)
        {
            sfxRazaContinue.Play(); 
        }


        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene(EscenaJuego); 
    }


    public void ReiniciarPartida()
    {
        StartCoroutine(SecuenciaRetry());
    }

    private IEnumerator SecuenciaRetry()
    {
        if (sfxRetry != null)
        {
            sfxRetry.Play();
        }

        yield return new WaitForSeconds(4.0f); 
        SceneManager.LoadScene(EscenaJuego);
    }


    public void IrAlMenu()
    {
        SceneManager.LoadScene(EscenaMenu);
    }
}