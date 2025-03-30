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
        if (numPuerta == 1)
        {
            posFinal = posicionFinalP1;
        }
        else
        {
            posFinal = posicionFinalP2;
        }

        StartCoroutine(MoverObjeto(transform, posFinal.position, duracion));

    }


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
