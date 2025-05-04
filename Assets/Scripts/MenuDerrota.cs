using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuDerrota : MonoBehaviour
{

    private AudioSource audioSource;
    public AudioClip sonidoDerrota;
    public void VolverAlMenu()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.clip = sonidoDerrota;
            audioSource.Play();
        }
    }
}
