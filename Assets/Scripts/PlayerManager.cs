using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerManager : MonoBehaviour
{

    PlayerController miplayerController;
    public int salud;  
    public int maximoBalasR;   
    public int maximoBalasP;
    public int balasActualesR { get; set; }
    public int balasActualesP { get; set; }
    private GameObject HUD;
    private TextMeshProUGUI BalasDataRSalida; // Rifle   
    private TextMeshProUGUI BalasDataPSalida;  // Pistola 
    private TextMeshProUGUI SaludDataSalida;
    public GameObject LuzVerde1;
    public GameObject LuzVerde2;
    public GameObject LuzRoja;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        miplayerController = GetComponent<PlayerController>();
       
    }

    public void CambiarLuces()
    {
        LuzVerde1.SetActive(true);
        LuzVerde2.SetActive(true);
        LuzRoja.SetActive(false);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void cargadaEscena(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "PrimerNivel" || scene.name == "SegundoNivel" || scene.name == "TercerNivel")
        {
            balasActualesR = GameManager.Instance.balasActualesR;
            balasActualesP = GameManager.Instance.balasActualesP;
            salud = GameManager.Instance.salud;
        }
        HUD = GameObject.FindWithTag("HUD");
        if(HUD != null)
        {
            SaludDataSalida = HUD.transform.GetChild(0).transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            BalasDataRSalida = HUD.transform.GetChild(0).transform.GetChild(4).GetComponent<TextMeshProUGUI>();
            BalasDataPSalida = HUD.transform.GetChild(0).transform.GetChild(5).GetComponent<TextMeshProUGUI>();
        }       
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        cargadaEscena(scene, mode);
    }
    private void cargarVida(int cantidad)
    {
        salud += cantidad;
    }

    public void damage(string tipo)
    {
        if (tipo == "enemigo")
        {
            salud -= 15;
        }
        else if (tipo == "trampa")
        {
            salud -= 5;
        }
        else
        {
            salud -= 10;
        }

        if (salud <= 0)
        {
            GameManager.Instance.reinicioEscena();           
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
            other.gameObject.GetComponent<MeshCollider>().enabled = false;
        }
    }
    private void Update()
    {
        BalasDataRSalida.text = balasActualesR.ToString();
        BalasDataPSalida.text = balasActualesP.ToString();
        SaludDataSalida.text = salud.ToString();
    }
}