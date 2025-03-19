using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    // Datos persistentes
    public int balasActualesR { get; set; }
    public int balasActualesP { get; set; }
    public int salud { get; set; }
    private string escenaActual;
    private int numEnemigos;
    private int numEscena = 0;
    private string[] escenas;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Evita que el objeto se destruya al cambiar de escena
        }
        else
        {
            Destroy(gameObject); // Destruye duplicados en caso de volver a la primera escena
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
        escenas = new string[4];
        escenas[0] = "PrimerNivel";
        escenas[1] = "SegundoNivel";
        escenas[2] = "TercerNivel";
        escenas[3] = "Victoria";
    }

    private void OnDestroy()
    {
        // Desuscribirse del evento cuando se destruye el GameManager
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Escena cargada: " + scene.name);
        GameObject[] Enemigos = GameObject.FindGameObjectsWithTag("Enemigo");
        numEnemigos = Enemigos.Length;
        escenaActual = scene.name;
    }


    public void EliminarEnemigo()
    {
        numEnemigos--;        
        if(numEnemigos == 0){
            numEscena++;
            SceneManager.LoadScene(escenas[numEscena]);
        }
    }
}
