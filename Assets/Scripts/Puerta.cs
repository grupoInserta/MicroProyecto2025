using UnityEngine;
using System.Collections;

public class Puerta : MonoBehaviour
{
    public float duracion = 20f;
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
            posFinal = posicionFinalP2;
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
        if (time <= 0f)
        {
            obj.position = targetPos;
            yield break;
        }

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
        //Debug.Log("Animacion de Puerta FINALiZADA");       
    }
}
