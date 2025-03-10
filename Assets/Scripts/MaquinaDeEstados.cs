using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MaquinaDeEstados : MonoBehaviour
{
    //Se podrían referenciar como el nombre del script, es decir, EstadoPatrulla, EstadoAlerta..., ésta es otra manera de hacerlo.
    public Estado estadoPatrulla;
    private Estado estadoAlerta;
    public Estado estadoPersecucion;
    private Estado estadoInicial;
    private Estado estadoActual;
    private Estado estadoPausa;


    //Podríamos establecer la función Awake y referenciar internamente con la instrucción GetComponent<>();
    //pero lo vamos a hacer arrastrando los scripts en el inspector.
    void Awake()
    {
        /*
          PRIMERO VA AWAKE Y LUEGO START
         Awake is used to initialize any variables or game state before the game starts.
        Awake is called only once during the lifetime of the script instance.
        Awake() and Start() are guaranteed to be called only once per object. OnEnable(), however, 
        will be called every time the object is enabled, either by another of your scripts or by Unity.
          */
        estadoPausa = gameObject.AddComponent(typeof(EstadoPausa)) as Estado;
        estadoInicial = gameObject.AddComponent(typeof(EstadoInicial)) as Estado;
        estadoAlerta = gameObject.AddComponent(typeof(EstadoAlerta)) as Estado;
        estadoPatrulla = gameObject.AddComponent(typeof(EstadoPatrulla)) as Estado;
        estadoPersecucion = gameObject.AddComponent(typeof(EstadoPersecucion)) as Estado;

    }
    void Start()
    {
        ActivarEstado(estadoInicial);
    }
    public void ActivarEstado(Estado nuevoEstado)
    {//La primera vez que este método se ejecuta, EstadoActual no tiene ningún valor, por lo que obtendríamos una excepción
     //así que vamos a ejecutarlo sólo si EstadoActual es distinto de nulo
        if (estadoActual != null)
        {
            estadoActual.enabled = false; //Deshabilitar el estado anterior
        }
        estadoActual = nuevoEstado; //Asignar el nuevo estado        
        estadoActual.enabled = true; //Habilitar el nuevo estado        
    }

    public void alertar()
    {
        ActivarEstado(estadoAlerta);
    }

    public void dejarEstadoAlerta()
    {
        ActivarEstado(estadoPatrulla);
    }

    public void perseguir()
    {
        ActivarEstado(estadoPersecucion);
    }
    public void dejarDePerseguir()
    {
        ActivarEstado(estadoPatrulla);
    }

    public void IniciarPatrulla()
    {
        ActivarEstado(estadoPatrulla);
    }


    public void Pausar()
    {
        estadoActual.enabled = false;
        estadoPausa.enabled = true;
    }

    public void Seguir()// cuando se pausa el juego con botones y/o controladores
    {
        estadoActual.enabled = true;
        estadoPausa.enabled = false;
    }
}

