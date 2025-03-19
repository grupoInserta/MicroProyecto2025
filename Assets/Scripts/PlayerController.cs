using UnityEngine;
using Unity.Cinemachine;


public class PlayerController : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    public Rigidbody rb;
    [SerializeField]
    float velocidadMax = 5f;
    float velocidad = 0;
    float velocidadLateral = 2f;
    private bool movimientoLateral;
    [SerializeField]
    float fuerzaSalto = 5f;

    private string ArmaSeleccionada;
 
    GameObject salidaBala1;
    GameObject salidaBala2;

    [SerializeField]
    public GameObject Bala1;
    [SerializeField]
    public GameObject Bala2;

    [SerializeField]
    public float VelocdiadBala;

    [SerializeField]
    private CinemachineCamera virtualCamera;
    private bool saltando;
    private Animator animacion;
    private int transionActual;
    private Puerta PuertaObjetoScript;
    private PlayerManager playerManager;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        playerManager = gameObject.GetComponent<PlayerManager>();       
        rb = GetComponent<Rigidbody>();
        ArmaSeleccionada = "Rifle";
        movimientoLateral = false;
        saltando = false;
        animacion = transform.GetChild(0).GetComponent<Animator>();
        transionActual = 0;
        PuertaObjetoScript = GameObject.FindWithTag("Puerta").GetComponent<Puerta>();
    }

   

    private void Disparar(string tipoArma)
    {
        salidaBala1 = GameObject.Find("SalidaBala");
        salidaBala2 = GameObject.Find("SalidaBala2");
        Debug.Log("Posición SalidaBala en el momento del disparo: " + salidaBala1.transform.position);
        Debug.Log("Dirección SalidaBala en el momento del disparo: " + salidaBala1.transform.forward);
        if (tipoArma == "Rifle")
        {
            if (playerManager.balasActualesR == 0) return;
            playerManager.balasActualesR--;
            GameObject bala = Instantiate(Bala1, transform.position, transform.rotation);
            bala.transform.position = salidaBala1.transform.position;
            Vector3 direccion = salidaBala1.transform.TransformDirection(Vector3.forward);
            bala.GetComponent<Bala>().configurarDisparo(VelocdiadBala, direccion);

        }
        else
        {
            if (playerManager.balasActualesP == 0) return;
            playerManager.balasActualesP--;

            GameObject bala = Instantiate(Bala2, transform.position, transform.rotation);
            bala.transform.position = salidaBala2.transform.position;
            bala.GetComponent<Bala>().configurarDisparo(VelocdiadBala, salidaBala2.transform.forward);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Suelo"))
        {
            saltando = true;            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Suelo"))
        {
            if (saltando)
            {
                transionActual = 7;
            }
            saltando = false;
            
        } 
        if (other.CompareTag("Placa"))
        {
            PuertaObjetoScript.IniciarRotacion();
        }

        
    }

    private void Animar(int transicion)
    {
       animacion.SetInteger("Transicion", transicion);
    }

    private void RotacionyMovimiento()
    {   // Calcular la direcci�n del movimiento basada en la c�mara (solo forward)
        Vector3 cameraForward = virtualCamera.transform.forward;

        cameraForward.y = 0; // Ignorar la componente Y para que el movimiento sea en el plano horizontal
        cameraForward.Normalize();

        Vector3 moveDirection = cameraForward; // La direcci�n es siempre forward
        if (Input.GetKey(KeyCode.W))
        {
            velocidad = velocidadMax;
            movimientoLateral = false;
            saltando = false;
            transionActual = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            velocidad = -velocidadMax;
            transionActual = 9;
            movimientoLateral = false;
            saltando = false;
        }

        if (Input.GetKeyUp(KeyCode.W) )
        {
            velocidad = 0;
            transionActual = 5;
        }
       if (Input.GetKeyUp(KeyCode.S))
        {
            velocidad = 0;
            transionActual = 10;
        }
        
        Vector3 perpendicular = new Vector3(-moveDirection.z, 0, moveDirection.x).normalized;
        if (Input.GetKey(KeyCode.D))
        {
            rb.linearVelocity = perpendicular * -velocidadLateral;
            Debug.Log("movimiento lateral: " + perpendicular * velocidadLateral);
            movimientoLateral = true;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            rb.linearVelocity = perpendicular * (velocidadLateral);
            movimientoLateral = true;
        }
        else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            movimientoLateral = false;
        }
         
        
        if (movimientoLateral == false && saltando == false)
        {
            rb.linearVelocity = moveDirection * velocidad;
            Debug.Log("ME MUEVO y velocidad = "+ velocidad);
        }

        // ROTACION
        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }      
    }

    // Update is called once per frame
    void Update()
    {
        SaltarDispararRodar();
        RotacionyMovimiento();
        Animar(transionActual);
    }


    public void cargarArma(int tipo)
    {
        if (tipo == 1)
        {
            playerManager.balasActualesR = playerManager.maximoBalasR;
        }
        else
        {
            playerManager.balasActualesP = playerManager.maximoBalasP;
        }
    }


    private void SaltarDispararRodar()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
            saltando = true;
            transionActual = 3;
            /*
            AnimatorStateInfo stateInfo = animacion.GetCurrentAnimatorStateInfo(0); // 0 = Layer base
            Debug.Log("Estado actual: " + stateInfo.shortNameHash);
            */
        }

        if ((Mathf.Abs(velocidad) > 0.1f) && Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Ruedo");
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (ArmaSeleccionada == "Rifle")
            {
                ArmaSeleccionada = "Pistola";
            }
            else
            {
                ArmaSeleccionada = "Rifle";
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Disparar(ArmaSeleccionada);
        }        
    }
}
