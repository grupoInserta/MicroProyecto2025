using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;


public class Enemigo : MonoBehaviour
{
    private Camino ScriptPath;
    public bool detectadoJugador = false;
    private MaquinaDeEstados maquinaDeEstados;
    private float tiempoDespuesGiros = 0;
    public GameObject explosionPrefabGrande;
    public AudioClip explosion;
    private AudioSource audioSource;


    // Start is called before the first frame update
    void Awake()
    {
        ScriptPath = gameObject.GetComponent("Camino") as Camino;// del propio enemigo
        maquinaDeEstados = gameObject.GetComponent("MaquinaDeEstados") as MaquinaDeEstados;
        audioSource = gameObject.AddComponent<AudioSource>();
        ScriptPath.andar();
        audioSource.playOnAwake = false;
    }

    public void Parar()
    {
        ScriptPath.parar();
    }

    public void Seguir()
    {
        maquinaDeEstados.Seguir();
    }

    private void hacerCirculoVision()
    {
        maquinaDeEstados.alertar();
        ScriptPath.parar();//el Path del enemigo
                           // se para la patrulla 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {           
            detectadoJugador = true;
            hacerCirculoVision();
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            detectadoJugador = false;
            maquinaDeEstados.dejarEstadoAlerta();
            ScriptPath.andar();
        }
    }   

    public void DamageEnemigo()
    {
        audioSource.clip = explosion;
        audioSource.Play();
        GameObject Explosion = Instantiate(explosionPrefabGrande, transform.position, Quaternion.identity);
        gameObject.tag = "Untagged";
        Destroy(gameObject,0.3f);
    }

    private void Update()
    {
        if (detectadoJugador)
        {
            tiempoDespuesGiros += Time.deltaTime;
            if (tiempoDespuesGiros == 17f) // el giro de busqueda es 16f, ver estadoALerta
            {
               maquinaDeEstados.dejarEstadoAlerta();
            }
            else if (tiempoDespuesGiros == 26f)
            {
                tiempoDespuesGiros = 0;
                hacerCirculoVision();
            }
        }

    }

}