using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audioClips; // Para SFXs
    public AudioClip backgroundMusic; // Para música de fondo
    private NativeHashMap<int, int> audioClipsMap; // Mapear todos los sonidos mediante índices
    private int audioClipIndex = 0; // Índice para acceder a los clips de audio

    enum AudioType
    {
        SFX,
        BackgroundMusic
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true; // Habilitar el bucle para la música de fondo
        audioSource.clip = backgroundMusic;
        audioSource.Play(); // Reproducir la música de fondo
        audioSource.volume = 0.5f; // Ajustar el volumen de la música de fondo
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void playSound(int soundIndex)
    {
        if (soundIndex < audioClips.Length)
        {
            audioSource.PlayOneShot(audioClips[soundIndex]);
        }
        else
        {
            Debug.LogWarning("Índice de sonido fuera de rango");
        }
    }
}
