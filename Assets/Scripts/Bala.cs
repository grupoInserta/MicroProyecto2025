using UnityEngine;


public class Bala : MonoBehaviour
{

    private Vector3 Direccion;
    private float VelocdiadBala;
    private AudioSource audioSource;
    public AudioClip disparo;
    private GameObject[] Enemigos;
    private GameObject[] Trampas;
    public GameObject explosionPrefab;
    public GameObject explosionPrefabMediano;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject Explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        audioSource = GetComponent<AudioSource>();
        Enemigos = GameObject.FindGameObjectsWithTag("Enemigo");
        Trampas = GameObject.FindGameObjectsWithTag("Trampa");

        if (audioSource != null && disparo != null)
        {
            audioSource.clip = disparo;
            audioSource.Play();
        }
        Explosion.transform.position = transform.position;
        Destroy(Explosion, 5f);
        Destroy(gameObject, 5f);        
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
                Enemigo.GetComponent<Enemigo>().DamageEnemigo();// explota
                Destroy(Enemigo);
            }
        }
        foreach (GameObject Trampa in Trampas)
        {
            if (Trampa == null) continue;
            float distance = Vector3.Distance(transform.position, Trampa.transform.position);
            if (distance < 0.28f)
            {
                GameObject ExplosionTrampa = Instantiate(explosionPrefabMediano, transform.position, Quaternion.identity);
                ExplosionTrampa.transform.position = Trampa.transform.position;
                Destroy(Trampa);
                Trampas = GameObject.FindGameObjectsWithTag("Trampa");
                // ó utilizar
                
            }
        }


    }

    public void configurarDisparo(float _VelocdiadBala, Vector3 _direccion)
    {
        Direccion = _direccion;
        VelocdiadBala = _VelocdiadBala;
    }


}
