
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EstadoAlerta : Estado
{
    public float velocidadGiroBusqueda = 10f;//grados por segundo
    public bool busquedaFinalizada = false;
    private MaquinaDeEstados maquinaDeEstados;
    private ControladorVision controladorVision;
    private ControladorNavMesh controladorNavMesh;
    private float angulosBuscando;
    private float rotationSpeed = 1600f;
    //private AudioSource Alerta;

    void Start()
    {
        maquinaDeEstados = GetComponent<MaquinaDeEstados>();
        controladorVision = transform.GetChild(0).GetComponent<ControladorVision>();
        controladorNavMesh = GetComponent<ControladorNavMesh>();
    }

    

    void Update()
    {
        //Time.deltaTime tiempo esn segundos desde el frame anterior al actual
        transform.Rotate(0f, -velocidadGiroBusqueda * Time.deltaTime, 0f);//cómo voy a girar
        angulosBuscando += velocidadGiroBusqueda * rotationSpeed * Time.deltaTime;//se incrementa en tiempo real el tiempo que está en alerta
        
        if (angulosBuscando >= 360 * rotationSpeed)
        {
            maquinaDeEstados.ActivarEstado(maquinaDeEstados.estadoPatrulla);//Volvemos al estado patrulla si no vemos al jugador en 4 segundos
            angulosBuscando = 0;
            busquedaFinalizada = true;
            return;//No hace falta, es sólo por si escribiéramos más código debajo
        }

        RaycastHit hit;

        if (controladorVision.puedeVerAlJugador(out hit))//Si ve al jugador se activa el estado de persecución, si nomandamos un segundo parametro mirar al jugador es false
        {           
            controladorNavMesh.Objetivo = hit.collider.gameObject.transform;//Devuelve la posición del jugador            
            maquinaDeEstados.perseguir();
            return;//Volvemos sin seguir debugando el método
        }
    }

    void OnEnable()
    {
        /*
        Alerta = GetComponent<AudioSource>();
        Alerta.enabled = true;
        Alerta.Play();
        */
 
        controladorNavMesh = GetComponent<ControladorNavMesh>();
        controladorNavMesh.DetenerNavMeshAgent();
        controladorNavMesh.enabled = false;
        angulosBuscando = 0;
        busquedaFinalizada = false;

    }

}