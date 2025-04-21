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
    [SerializeField]
    private GameObject CanvasGameManager;
    [SerializeField]
    private Button botonPausa;
    [SerializeField]
    private Button botonInicio;
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
        JuegoPausado = false;
    }

    private void IrAInicio()
    {
        balasActualesR = 30;
        balasActualesP = 30;
        salud = 100;
        JuegoPausado = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void TerminarJuego()
    {
        SceneManager.LoadScene("Victoria");
    }
    public void reinicioEscena()
    {
        balasActualesR = 30;
        balasActualesP = 30;
        salud = 100;
        CanvasGameManager.SetActive(false);
        SceneManager.LoadScene(escenaActual);
    }

    private void OnDestroy()
    {
        // Desuscribirse del evento cuando se destruye el GameManager
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {   
        if(scene.name != "Derrota")
        {
            escenaActual = scene.name;
        }
        
        if (escenaActual != "PrimerNivel" && escenaActual != "SegundoNivel" && escenaActual != "TercerNivel")
        {
            CanvasGameManager.SetActive(false);
        }
        
        botonPausa.onClick.AddListener(() => Reanudar());
        botonInicio.onClick.AddListener(() => IrAInicio());
    }

    private void Reanudar()
    {
        JuegoPausado = false;
        Time.timeScale = 1f;  // Reanuda el juego
                              // botonPausa.GetComponentInChildren<TextMeshProUGUI>().text = "Pausar";
        CanvasGameManager.SetActive(false);
    }

    public void PausarJuego()
    {
        if (!JuegoPausado)
        {
            CanvasGameManager.SetActive(true);
            Time.timeScale = 0f;// Pausa todo el juego
            JuegoPausado = true;
            botonPausa.GetComponentInChildren<TextMeshProUGUI>().text = "Continuar";
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
