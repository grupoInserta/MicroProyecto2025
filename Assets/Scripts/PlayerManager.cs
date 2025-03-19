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
    private TextMeshProUGUI BalasDataR; // Rifle   
    private TextMeshProUGUI BalasDataP;  // Pistola 
    private TextMeshProUGUI SaludData;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
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
        Debug.Log("La escena " + scene.name + " se ha cargado completamente.");
        // Aquí puedes inicializar al Player
        HUD = GameObject.FindWithTag("HUD");
        SaludData = HUD.transform.GetChild(0).transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        BalasDataR = HUD.transform.GetChild(0).transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        BalasDataP = HUD.transform.GetChild(0).transform.GetChild(5).GetComponent<TextMeshProUGUI>();
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
        else if(tipo == "trampa")
        {
            salud -= 5;
            Debug.Log("SALUD menor por trampa: " + salud);
        }
        else
        {
            salud -= 10;
        }
        if (salud < 0)
        {
            Debug.Log("PARTIDA PERDIDA!!!!!");
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

    }
    private void Update()
    {
        BalasDataR.text = balasActualesR.ToString();
        BalasDataP.text = balasActualesP.ToString();
        SaludData.text = salud.ToString();
    }
}