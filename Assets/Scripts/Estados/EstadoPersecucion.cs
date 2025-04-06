using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EstadoPersecucion : Estado
{
    private ControladorNavMesh controladorNavMesh;
    private MaquinaDeEstados maquinaDeEstados;
    private ControladorVision controladorVision;
    private Camino scriptPath;
    private NavMeshAgent navMeshAgent;
    // Start is called before the first frame update
    void Start()
    {

    }


    public void OnEnable()
    {

        controladorNavMesh = GetComponent<ControladorNavMesh>();
        controladorVision = transform.GetChild(0).GetComponent<ControladorVision>();
        maquinaDeEstados = GetComponent<MaquinaDeEstados>();
        scriptPath = GetComponent<Camino>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.enabled = true; 
        perseguir();
    }

    public void perseguir()
    {
        controladorNavMesh.ActivarNavMeshAgent();
        scriptPath.parar();
    }

    public void dejarDePerseguir()
    {
        Debug.Log("DEJO DE PERSEGUIR");
        scriptPath.andar();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (!controladorVision.puedeVerAlJugador(out hit, true))
        {
            maquinaDeEstados.dejarDePerseguir();
            return;
        }
        else
        {
            controladorNavMesh.ActualizarPuntoDestinoNavMeshAgent();
            Debug.Log("persiguiendo....");
        }
    }
}
