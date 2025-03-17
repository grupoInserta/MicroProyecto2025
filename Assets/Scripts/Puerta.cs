using UnityEngine;
using System.Collections;

public class Puerta : MonoBehaviour
{
    public Vector3 destino;
    public float duracion = 1f;

    public void IniciarRotacion()
    {
        StartCoroutine(RotarObjeto(transform.rotation, Quaternion.Euler(0, 90, 0), 1f));
    }

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
}
