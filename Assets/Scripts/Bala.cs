using UnityEngine;


public class Bala : MonoBehaviour
{

    private Vector3 Direccion;
    private float VelocdiadBala;
    private AudioSource audioSource;
    public AudioClip disparo;
    private GameObject[] Ojos;
    private GameObject[] Trampas;
    public GameObject explosionPrefab;
    public GameObject explosionPrefabMediano;
    public GameObject explosionPrefabGrande;
    public AudioClip explosion;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject Explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        audioSource = GetComponent<AudioSource>();
        Ojos = GameObject.FindGameObjectsWithTag("ojos");
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
        foreach (GameObject Ojo in Ojos)
        {
            if (Ojo == null) continue;
            float distance = Vector3.Distance(transform.position, Ojo.transform.position);
            if (distance < 0.8f)
            {                 
                Ojo.transform.parent.GetComponent<Enemigo>().DamageEnemigo();// explota
                Ojos = GameObject.FindGameObjectsWithTag("ojos");
            }
        }
        foreach (GameObject Trampa in Trampas)
        {
            if (Trampa == null) continue;
            float distance = Vector3.Distance(transform.position, Trampa.transform.position);
            if (distance < 0.36f)
            {
                GameObject ExplosionTrampa = Instantiate(explosionPrefabMediano, transform.position, Quaternion.identity);
                ExplosionTrampa.transform.position = Trampa.transform.position;
                Destroy(Trampa);
                Trampas = GameObject.FindGameObjectsWithTag("Trampa");
            }
        }
    }

    public void configurarDisparo(float _VelocdiadBala, Vector3 _direccion)
    {
        Direccion = _direccion;
        VelocdiadBala = _VelocdiadBala;
    }


}
