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
        panelMainMenu.SetActive(false); // Apaga pantalla menú
        panelTutorial.SetActive(true);  // Pantalla tutorial
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
        
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Alex Scene"); 
    }
}