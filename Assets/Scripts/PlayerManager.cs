using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerManager : MonoBehaviour
{

    PlayerController miplayerController;
    public int salud;
    [SerializeField]
    public int maximoBalasR;
    [SerializeField]
    public int maximoBalasP;
    public int balasActualesR { get; set; }
    public int balasActualesP { get; set; }

    private GameObject HUD;
    private TextMeshProUGUI BalasDataRSalida; // Rifle   
    private TextMeshProUGUI BalasDataPSalida;  // Pistola 
    private TextMeshProUGUI SaludDataSalida;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        miplayerController = GetComponent<PlayerController>();
        salud = 30;
        balasActualesR = maximoBalasR;
        balasActualesP = maximoBalasP;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if( scene.name == "SegundoNivel" || scene.name == "TercerNivel")
        {
            balasActualesR = GameManager.Instance.balasActualesR;
            balasActualesP = GameManager.Instance.balasActualesP;
            salud = GameManager.Instance.salud;
        }
        // Aqu� puedes inicializar al Player
        HUD = GameObject.FindWithTag("HUD");
        SaludDataSalida = HUD.transform.GetChild(0).transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        BalasDataRSalida = HUD.transform.GetChild(0).transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        BalasDataPSalida = HUD.transform.GetChild(0).transform.GetChild(5).GetComponent<TextMeshProUGUI>();
    }


    private void cargarVida(int cantidad)
    {
        salud += cantidad;
        Debug.Log(salud);
    }

    public void damage(string tipo)
    {
        if (tipo == "enemigo")
        {
            salud -= 5;
            Debug.Log("SALUD: " + salud);
        }
        else if (tipo == "trampa")
        {
            salud -= 5;
            Debug.Log("SALUD menor por trampa: " + salud);
        }
        else
        {
            salud -= 10;
        }
        // IMPLEMENTAR QUE SE CAIGA EN NIVEL 2 !!!!!!!!!!!!!!!!!!!!!!!!!
        if (salud < 0)
        {
            GameManager.Instance.DerrotaJuego();
        }        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Municion1"))
        {
            miplayerController.cargarArma(1);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Municion2"))
        {
            miplayerController.cargarArma(2);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("CajaVida"))
        {
            cargarVida(10);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Trampa"))
        {
            damage("trampa");
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("CambioEscena"))
        {
            GameManager.Instance.balasActualesR = balasActualesR;
            GameManager.Instance.balasActualesR = balasActualesP;
            GameManager.Instance.salud = salud;
            GameManager.Instance.CambiarEscena();
        }
    }
    private void Update()
    {
        BalasDataRSalida.text = balasActualesR.ToString();
        BalasDataPSalida.text = balasActualesP.ToString();
        SaludDataSalida.text = salud.ToString();
    }
}