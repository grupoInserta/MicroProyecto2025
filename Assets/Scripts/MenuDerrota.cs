using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuDerrota : MonoBehaviour
{
    public void VolverAlMenu()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }
}
