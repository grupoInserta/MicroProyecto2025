using UnityEngine;
using System.Collections;

public class Puerta : MonoBehaviour
{
    private float duracion = 12f;
    public Transform posicionFinalP1;
    public Transform posicionFinalP2;
    public AudioClip aperturaPuerta;
    private AudioSource audioSource;
    private Vector3 posIniPuerta;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        posIniPuerta = transform.position;
    }

    public void IniciarDesplazamiento(int numPuerta)
    {
        if (audioSource != null)
        {
            audioSource.clip = aperturaPuerta;
            audioSource.Play();
        }
        Transform posFinal;
        if (numPuerta == 1) // por si hay 2 puertas
        {
            posFinal = posicionFinalP1;
        }
        else
        {
            posFinal = posicionFinalP2; // abrir puerta de la llave
        }

        StartCoroutine(MoverObjeto(transform, posFinal.position, duracion));
        //StartCoroutine(RotarObjeto(transform.rotation, Quaternion.Euler(0, 90, 0), 1f));
    }

    public void restablecerPosicPuerta()
    {
        transform.position = posIniPuerta;
    }

    private IEnumerator MoverObjeto(Transform obj, Vector3 targetPos, float time)
    {

        Vector3 startPos = obj.position;
        float elapsedTime = 0f;
        int contador = 0;
        while (elapsedTime < time)
        {
            contador++;
            elapsedTime += Time.deltaTime;
            //float t = elapsedTime / time;
            float t = Mathf.Clamp01(elapsedTime / time);
            obj.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null; //  pausa la ejecución de la corrutina hasta el siguiente frame.
        }
        obj.position = targetPos;

    }
}
