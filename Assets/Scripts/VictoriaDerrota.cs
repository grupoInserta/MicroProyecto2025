using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoriaDerrota : MonoBehaviour
{
    public void VolverAlMenu()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void VolverAEscenaActual()
    {
        SceneManager.LoadScene(GameManager.Instance.escenaActual);
    }
}


