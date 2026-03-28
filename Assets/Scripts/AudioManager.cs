using UnityEngine;
//singleton pattern 

/// <summary>
/// Gestor de audio simple para Unity.
/// - Implementa un singleton para que exista una única instancia accesible globalmente.
/// - Contiene referencias a dos `AudioSource`: uno para efectos de sonido (SFX) y otro
///   para la música de fondo.
/// 
/// Nota para estudiantes: asigna los `AudioSource` desde el Inspector (arrastrar los
/// componentes). Conviene que el `musicAudioSource` tenga `loop = true` y el `sfxAudioSource`
/// `loop = false` (los SFX suelen reproducirse puntual o solapadamente).
/// </summary>
/// El patrón singleton es un patrón de diseño que garantiza que una clase tenga una única instancia y 
/// proporciona un punto de acceso global a esa instancia. En el contexto de Unity, esto es útil para gestores como el AudioManager, 
/// donde queremos asegurarnos de que solo haya un gestor de audio en toda la aplicación y que sea fácilmente accesible desde cualquier 
/// otro script sin necesidad de referencias complicadas o dependencias.
public class AudioManager : MonoBehaviour

{
// Singleton: permite acceder al gestor desde cualquier script mediante AudioManager.Instance
    public static AudioManager Instance { get; private set; }

    // Referencias a los AudioSource que se deben asignar desde el Inspector.
    // - sfxAudioSource: para reproducir sonidos cortos (disparos, pasos, UI...)
    // - musicAudioSource: para reproducir música de fondo (BGM)
    [Header("Audio Sources")]
    [SerializeField] AudioSource musicAudioSource, sfxAudioSource;
    [Header("Global Volume")]
    //[Range(0f, 1f)] limita el volumen a un rango entre 0 y 1 para evitar errores de volumen excesivo.
    public float masterVolume = 1f;
    public float musicVolume = 1f;
    public float sfxVolume = 1f;


    /// <summary>
    /// Awake se ejecuta cuando se instancia el objeto. Aquí se aplica el patrón singleton
    /// y se asegura que el objeto persista al cambiar de escena con DontDestroyOnLoad.
    /// Si ya existe otra instancia, destruimos esta para mantener una sola copia.
    /// </summary>
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    /// <summary>
    /// Reproduce un efecto de sonido (SFX) usando PlayOneShot.
    /// PlayOneShot permite solapar varias instancias del mismo clip sin interrumpir
    /// reproducciones previas, ideal para disparos, impactos o sonidos UI.
    /// </summary>
    /// <param name="clip">Clip de audio a reproducir como efecto.</param>
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        Debug.Log("PlaySFX llamado con: " + clip.name);
        Debug.Log("sfxAudioSource: " + sfxAudioSource.name);
        //if (clip == null || sfxAudioSource == null) return; // protección simple contra nulls
        sfxAudioSource.PlayOneShot(clip, volume * sfxVolume * masterVolume);

    }
    /// <summary>
    /// Cambia y reproduce la música de fondo.
    /// - Se detiene la música actual, se asigna el nuevo clip y se reproduce.
    /// - Si quieres crossfades o transiciones suaves, hay que implementar lógica adicional.
    /// </summary>
    /// <param name="newMusic">Nuevo clip de música para reproducir.</param>
    public void PlayMusic(AudioClip newMusic)
    {
        if(musicAudioSource == null) return; // protección simple contra nulls
        if(newMusic == musicAudioSource.clip) return; // si ya está sonando esa música, no hacemos nada

        // Si se desea un cambio instantáneo: detener, asignar y reproducir.
        musicAudioSource.Stop();
        musicAudioSource.clip = newMusic;
        musicAudioSource.volume = musicVolume * masterVolume;
        musicAudioSource.loop = true; // la música de fondo suele ser en bucle  
        musicAudioSource.Play();
    }
    public void StopMusic()
    {
        if(musicAudioSource == null) return; // protección simple contra nulls
        musicAudioSource.Stop();
    }

}