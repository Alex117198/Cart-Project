using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // Vital para usar temporizadores (Corrutinas)

public class Start_Race : MonoBehaviour
{
    public AudioSource sfxArranque; 

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

        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("Pablo Scene"); 
    }
}