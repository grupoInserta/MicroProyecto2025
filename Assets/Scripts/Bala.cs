using UnityEngine;

public class Bala : MonoBehaviour
{

    private Vector3 Direccion;
    private float VelocdiadBala;
    private AudioSource audioSource;
    public AudioClip disparo;
    private GameObject[] Enemigos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Enemigos = GameObject.FindGameObjectsWithTag("Enemigo");

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
        // contactos con enemigos:
        foreach(GameObject Enemigo in Enemigos)
        {
            float distance = Vector3.Distance(transform.position, Enemigo.transform.position);
            if (distance < 1f)
            {
                Destroy(Enemigo);
                // ó utilizar
                Enemigo.GetComponent<Enemigo>().DamageEnemigo();
            }
        }
    }

    public void configurarDisparo(float _VelocdiadBala, Vector3 _direccion)
    {
        Direccion = _direccion;
        VelocdiadBala = _VelocdiadBala;
    }

    
}
