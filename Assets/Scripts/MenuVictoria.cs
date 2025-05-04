using UnityEngine;

public class MenuVictoria : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip sonidoVictoria;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.clip = sonidoVictoria;
            audioSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
