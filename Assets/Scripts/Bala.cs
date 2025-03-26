using UnityEngine;


public class Bala : MonoBehaviour
{

    private Vector3 Direccion;
    private float VelocdiadBala;
    private AudioSource audioSource;
    public AudioClip disparo;
    private GameObject[] Enemigos;
    public GameObject explosionPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject Explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        audioSource = GetComponent<AudioSource>();
        Enemigos = GameObject.FindGameObjectsWithTag("Enemigo");

        if (audioSource != null && disparo != null)
        {
            audioSource.clip = disparo;
            audioSource.Play();
        }
        Destroy(Explosion, 5f);
        Destroy(gameObject, 5f);
        
    }

    private void Awake()
    {
        //Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Direccion * VelocdiadBala * Time.deltaTime;
        // contactos con enemigos:
        foreach (GameObject Enemigo in Enemigos)
        {
            if (Enemigo == null) continue;
            float distance = Vector3.Distance(transform.position, Enemigo.transform.position);
            if (distance < 1f)
            {
                Destroy(Enemigo);
                //GameManager.Instance.EliminarEnemigo();
                Enemigos = GameObject.FindGameObjectsWithTag("Enemigo");
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
