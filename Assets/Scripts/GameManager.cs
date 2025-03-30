using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    // Datos persistentes
    public int salud;
    [SerializeField]
    public int maximoBalasR = 30;
    [SerializeField]
    public int maximoBalasP = 30;
    public int balasActualesR { get; set; }
    public int balasActualesP { get; set; }
    public string escenaActual;
    private int numEscena = 0;
    private string[] escenas;
    private Button botonPausa;
    public bool JuegoPausado { get; set; }
 

    private void Awake()
    {

        salud = 100;
        balasActualesR = maximoBalasR;
        balasActualesP = maximoBalasP;
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

    public void reinicioEscena()
    {
        balasActualesR = 30;
        balasActualesP = 30;
        salud = 100;
        SceneManager.LoadScene("Derrota");
    }


    public void Morir() {
        SceneManager.LoadScene("Derrota");
    }

    private void OnDestroy()
    {
        // Desuscribirse del evento cuando se destruye el GameManager
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void FinJuego()
    {
        SceneManager.LoadScene("Victoria");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {   
        if(scene.name != "Derrota")
        {
            escenaActual = scene.name;
        }
        
        if (escenaActual != "PrimerNivel" && escenaActual != "SegundoNivel" && escenaActual != "TercerNivel")
        {
            botonPausa.gameObject.SetActive(false);
            balasActualesR = 30;
            balasActualesP = 30;
            salud = 100;
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
