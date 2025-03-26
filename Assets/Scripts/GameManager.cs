using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
    private Button botonPausa;
    public bool JuegoPausado { get; set; }

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
        botonPausa = GetComponentInChildren<Button>();
        JuegoPausado = false;
    }

    public void DerrotaJuego()
    {

    }

    private void OnDestroy()
    {
        // Desuscribirse del evento cuando se destruye el GameManager
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject[] Enemigos = GameObject.FindGameObjectsWithTag("Enemigo");
        numEnemigos = Enemigos.Length;
        escenaActual = scene.name;
        if (escenaActual != "PrimerNivel" && escenaActual != "SegundoNivel" && escenaActual != "TercerNivel")
        {
            botonPausa.gameObject.SetActive(false);
        }
        else
        {
            botonPausa.gameObject.SetActive(true);
        }
    }

    public void PausarJuego()
    {
        if (!JuegoPausado)
        {
            Time.timeScale = 0f;// Pausa todo el juego
            JuegoPausado = true;
            // transform.GetChild(0)
            botonPausa.GetComponentInChildren<TextMeshProUGUI>().text = "Continuar";
        }
        else
        {
            JuegoPausado = false;
            Time.timeScale = 1f;  // Reanuda el juego
            botonPausa.GetComponentInChildren<TextMeshProUGUI>().text = "Pausar";
        }         
    } 


    public void EliminarEnemigo()
    {
        numEnemigos--;
    }

    public void CambiarEscena()
    {     
       numEscena++;
       SceneManager.LoadScene(escenas[numEscena]);        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PausarJuego();
        }
    }
}
