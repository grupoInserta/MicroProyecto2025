using UnityEngine;

public class Bala : MonoBehaviour
{

    private Vector3 Direccion;
    private float VelocdiadBala;
    private AudioSource audioSource;
    public AudioClip disparo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        audioSource = GetComponent<AudioSource>();
       

        if (audioSource != null && disparo != null)
        {
            audioSource.clip = disparo;
            audioSource.Play();
        }
       
        Destroy(gameObject, 5f);
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Direccion * VelocdiadBala * Time.deltaTime;
    }

    public void configurarDisparo(float _VelocdiadBala, Vector3 _direccion)
    {
        Direccion = _direccion;
        VelocdiadBala = _VelocdiadBala;
    }
}
