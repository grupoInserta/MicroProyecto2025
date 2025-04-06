using UnityEngine;
using System.Collections;

public class Puerta : MonoBehaviour
{

    public float duracion = 1f;
    public Transform posicionFinalP1;
    public Transform posicionFinalP2;

    public void IniciarDesplazamiento(int numPuerta)
    {
        Transform posFinal;
        if (numPuerta == 1) // por si hay 2 puertas
        {
            posFinal = posicionFinalP1;
        }
        else
        {
            posFinal = posicionFinalP2;
        }

        StartCoroutine(MoverObjeto(transform, posFinal.position, duracion));
        //StartCoroutine(RotarObjeto(transform.rotation, Quaternion.Euler(0, 90, 0), 1f));
    }

    /*
    IEnumerator RotarObjeto(Quaternion inicio, Quaternion fin, float tiempo)
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / tiempo;
            transform.rotation = Quaternion.Lerp(inicio, fin, t);
            yield return null;
        }
    }
    */

    private IEnumerator MoverObjeto(Transform obj, Vector3 targetPos, float time)
    {
        Vector3 startPos = obj.position;
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / time;
            obj.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }
        obj.position = targetPos;
        Debug.Log("Animacion de Puerta FINALiZADA");       
    }
}
