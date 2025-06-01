using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ControladorNavMesh : MonoBehaviour
{
    public Transform Objetivo;
    private bool persiguiendo = false;
    private NavMeshAgent agente;
    // Start is called before the first frame update
    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        agente.updateRotation = false;
    }
    public void DetenerNavMeshAgent()
    {
        agente.enabled = false;
        persiguiendo = false;
    }

    public void ActivarNavMeshAgent()
    {
        agente.enabled = true;
        persiguiendo = true;
    }

    private bool IsOnNavMesh(Vector3 position)
    {
        NavMeshHit hit;
        bool isOnNavMesh = NavMesh.SamplePosition(position, out hit, 0.1f, NavMesh.AllAreas);
        return isOnNavMesh;
    }

    // Update is called once per frame
    public void ActualizarPuntoDestinoNavMeshAgent() // viene del script EstadoPersecucion
    {
        if (Objetivo != null && persiguiendo)
        {
            if(agente != null && agente.isOnNavMesh) //isOnNavMesh propiedad que sustituye al metodo de arriba
            {                
                agente.SetDestination(Objetivo.position); // Mueve al enemigo               
              
            }
            
            // Hacer que el enemigo mire al objetivo
            Vector3 direction = Objetivo.position - transform.position;
            direction.y = 0; // Evita inclinaciones no deseadas
            if (direction.magnitude > 0.1f) // Evita rotaciones innecesarias
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }        
    }

    public bool objetivoAlcanzado()
    {
        //Debug.Log(agente.remainingDistance);
        // parece que el valor de agente.stoppingDistance se determina en el slot del NavMesh llamado de la misma forma.
        return agente.remainingDistance <= agente.stoppingDistance && !agente.pathPending;
        /*remainingDistance es la distancia que falta para llegar al punto de destino y
        pertenece a la API de Unity como las siguientes */
        //stoppingDistance es la distancia que tenemos definida para detenernos
        //pathPending significa que no queda mas camino pendiente
    }
}




