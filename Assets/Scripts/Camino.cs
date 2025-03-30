using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Camino : MonoBehaviour
{
    private Transform[] Puntos;
    GameObject[] PuntosObjeto;
    [SerializeField] float velocidad;
    public Transform PuntoActivo;
    public bool parado;
    private int IndicePuntos;
    private bool puntosDetectados = false;
    private NavMeshAgent agente;
    private Vector3 PosicionInicial;
    private Quaternion RotacionInicial;
    [SerializeField]
    public GameObject ContenedorPuntosObjeto;

    void Start()
    {
        if (Puntos != null && Puntos.Length > 0)
        {
            puntosDetectados = true;           
            Iniciar();
        }
    }

    public void PosicionarInicial()
    {
        gameObject.transform.position = PosicionInicial;
        gameObject.transform.rotation = RotacionInicial;
    }

    private void Iniciar()
    {
        transform.position = Puntos[0].position;
        IndicePuntos = 0;
        if (Puntos != null && Puntos.Length > 0)
        {
            PuntoActivo = Puntos[1];
        }
        PosicionInicial = Puntos[0].transform.position;
        RotacionInicial = gameObject.transform.rotation;
        gameObject.transform.position = PosicionInicial;
        agente = GetComponent<NavMeshAgent>();       
        agente.autoBraking = false;
    }

    public void parar()
    {
        this.enabled = false;
        parado = true;
    }

    public void andar()
    {
        this.enabled = true;
        parado = false;
    }


    private void irProximoPunto()
    {
        if (Puntos.Length == 0)
        {
            return;
        }
        agente.destination = Puntos[IndicePuntos].position; // orden de que se dirija aun punto
    }

    // Update is called once per frame
    void Update()
    {
        if (puntosDetectados == false)
        { 
            if (ContenedorPuntosObjeto == null)
            {
                return;
            }
            int numeroObjetos = ContenedorPuntosObjeto.transform.childCount;
            if (numeroObjetos > 0)
            {
                Puntos = new Transform[numeroObjetos];
                for (int i = 0; i < numeroObjetos; i++)
                {
                    Puntos[i] = ContenedorPuntosObjeto.transform.GetChild(i).gameObject.transform;
                }
                puntosDetectados = true;
                Iniciar();
            }
            return;
        }


        if (parado)
        {
            return;
        }

        //transform.position = Vector3.MoveTowards(transform.position, Puntos[IndicePuntos].transform.position, velocidad * Time.deltaTime);
        if (agente.remainingDistance < 0.5f)
        {
            IndicePuntos++; // sigue a atro punto
            irProximoPunto();
        }
        if (IndicePuntos == Puntos.Length - 1)
        {
            IndicePuntos = 0;
        }
        PuntoActivo = Puntos[IndicePuntos];
        var targetRotation = Quaternion.LookRotation(PuntoActivo.transform.position - transform.position);
        // Smoothly rotate towards the target point.
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.5f * Time.deltaTime);
    }
}

