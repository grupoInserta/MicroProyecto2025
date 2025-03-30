using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicio : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void IniciarJuego()
    {
        SceneManager.LoadScene("PrimerNivel");
       // SceneManager.LoadScene("SegundoNivel");
    }

    public void SalirJuego()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
