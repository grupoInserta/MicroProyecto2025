using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ControladorVision : MonoBehaviour

{
    public float rangoVision = 100000f;//La distancia a la que vamos a lanzar el rayo
    public Transform Ojos;//Desde dónde lanzamos el rayo
    public Vector3 offset = new Vector3(0f, 0f, 0f);//La posición del jugador está en y=0, por eso la subimos, para que ojos pueda ver en paralelo al suelo
    private ControladorNavMesh controladorNavMesh;
    private float RadioRayo = 1.0f;


    void Start()
    {

        controladorNavMesh = transform.parent.GetComponent<ControladorNavMesh>();
        Ojos = this.gameObject.transform;
    }


    public bool puedeVerAlJugador(out RaycastHit hit, bool mirarHaciaElJugador = false)
    {
       
        Vector3 vectorDireccion;

        if (mirarHaciaElJugador)//En caso de estar en persecución, necesitamos que siga al jugador en cualquier caso, no sólo si lo ve
        {
            vectorDireccion = (controladorNavMesh.Objetivo.position + offset) - Ojos.position;
            //La dirección del rayo sería la resta de la posición del jugador - la posición del objeto ojos
        }
        else
        { // en caso de alerta y patrulla
            vectorDireccion = Ojos.forward;
            // forward es un vector en direccion z: new Vector3(0,0.1) del objeto!!
            // Vector3.forward es global   y transform.forward es locoal
        }
        Debug.DrawRay(gameObject.transform.position, vectorDireccion * 100, Color.green, 0.2f, false);// se ve rayo verde
        //bool puedeVerlo = Physics.SphereCast(Ojos.transform.position, RadioRayo, vectorDireccion * 100, out hit, Mathf.Infinity) && hit.collider.CompareTag("Player");
        bool puedeVerlo = Physics.Raycast(Ojos.transform.position, vectorDireccion * 100, out hit, Mathf.Infinity) && hit.collider.CompareTag("Player");
        if (puedeVerlo)
        {
            float distancia = hit.distance;
            if (distancia < 0.2f)
            {
                hit.collider.gameObject.transform.GetComponent<PlayerManager>().damage("enemigo");
                transform.parent.gameObject.GetComponent<Enemigo>().DamageEnemigo();
                
            }
        }
        return puedeVerlo;
        //(origen del rayo, la dirección del rayo, información de dónde ha impactado, distancia máxima && ha colisionado con el jugador)
    }
}
