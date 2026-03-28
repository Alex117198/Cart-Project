using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Start_Race : MonoBehaviour
{
    [Header("Paneles de Interfaz")]
    public GameObject panelMainMenu;
    public GameObject panelTutorial;
    public GameObject panelTutorial2;
    public GameObject panelGameOver;

    [Header("Efectos de Sonido")]
    public AudioSource sfxMenuStart;
    public AudioSource sfxRazaContinue;
    public AudioSource sfxRetry;

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

    public void MostrarMenu2()
    {
        panelMainMenu.SetActive(false);
        panelTutorial.SetActive(false);
        panelTutorial2.SetActive(true);

    }

    private IEnumerator IniciarCarrera()
    {
        if (sfxRazaContinue != null)
        {
            sfxRazaContinue.Play(); 
        }


        yield return new WaitForSeconds(3.5f);
        panelMainMenu.SetActive(false);
        panelTutorial.SetActive(false);
        panelTutorial2.SetActive(false);
        panelGameOver.SetActive(true);
        SceneManager.LoadScene(1); 
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
        SceneManager.LoadScene(1);
    }


    public void IrAlMenu()
    {
        SceneManager.LoadScene(0);
    }
}