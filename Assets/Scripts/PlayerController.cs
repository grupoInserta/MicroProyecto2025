using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.EventSystems;
using System.Collections;


public class PlayerController : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    public Rigidbody rb;
    [SerializeField]
    float velocidadAndando = 5f;
    float velocidadCorriendo = 7.5f;
    float velocidadRodando = 10f;
    float velocidad = 0;
    float velocidadLateral = 2f;
    private bool movimientoLateral;
    [SerializeField]
    float fuerzaSalto = 5f;
    float fuerzaSalto2 = 3f;
    float fuerzaSaltoAdelante = 7f;
    private string ArmaSeleccionada;
    GameObject salidaBalaR;
    GameObject salidaBalaP;

    [SerializeField]
    public GameObject PlayerModel;

    [SerializeField]
    public GameObject Rifle;
    [SerializeField]
    public GameObject PosRifle2;
    [SerializeField]
    public GameObject Pistola;
    [SerializeField]
    public GameObject PistolaMano;
    private Vector3 PosIniRifle;
    private Vector3 PosIniPistola;
    private Quaternion RotIniRifle;
    private Quaternion RotIniPistola;
    //
    [SerializeField]
    public GameObject Mano;
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
    private int contadorAndar;
    private float diferenciaAlturaInicial;
    private float duracionCambioArma = 0.2f;
    private float extraGravity = 6f;


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
        salidaBalaR = GameObject.FindWithTag("SalidaBalaR");
        salidaBalaP = GameObject.FindWithTag("SalidaBalaP");
        contadorAndar = 0;
        float cameraAltura = virtualCamera.transform.position.y;
        diferenciaAlturaInicial = cameraAltura - transform.position.y;
        //
        PosIniRifle = Rifle.transform.localPosition;
        PosIniPistola = Pistola.transform.localPosition;
        RotIniRifle = Rifle.transform.localRotation;
        RotIniPistola = Pistola.transform.localRotation;
        //

    }

    private void AnimDePistolaARifle()
    {
        Pistola.transform.position = transform.position + PosIniPistola;
        Pistola.transform.rotation =  RotIniPistola;
        StartCoroutine(AnimateChild(Rifle.transform, transform.position + PosIniRifle, RotIniRifle, duracionCambioArma));
    }

    private void AnimDeRifleAPistola()
    {
        Pistola.transform.position = PistolaMano.transform.position;
        Pistola.transform.rotation = PistolaMano.transform.rotation;
        StartCoroutine(AnimateChild(Rifle.transform, PosRifle2.transform.position, PosRifle2.transform.rotation, duracionCambioArma));
    }

    private IEnumerator AnimateChild(Transform obj, Vector3 targetPos, Quaternion targetRot, float time)
    {
        Vector3 startPos = obj.position;
        Quaternion startRot = obj.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / time;
            obj.position = Vector3.Lerp(startPos, targetPos, t);
            obj.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }
        obj.position = targetPos;
        obj.rotation = targetRot;
        Debug.Log("cuantas veces 1");
    }


    private void Disparar(string tipoArma)
    {
       
        if (tipoArma == "Rifle")
        {
            if (playerManager.balasActualesR == 0) return;
            playerManager.balasActualesR--;
            GameObject bala = Instantiate(Bala1, transform.position, transform.rotation);
            bala.transform.position = salidaBalaR.transform.position;
            Vector3 direccion = salidaBalaR.transform.TransformDirection(Vector3.forward);
            bala.GetComponent<Bala>().configurarDisparo(VelocdiadBala, direccion);
        }
        else
        {
            if (playerManager.balasActualesP == 0) return;
            playerManager.balasActualesP--;

            GameObject bala = Instantiate(Bala2, transform.position, transform.rotation);
            bala.transform.position = salidaBalaP.transform.position;
            bala.GetComponent<Bala>().configurarDisparo(VelocdiadBala, salidaBalaP.transform.forward);
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
        if (Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.down * extraGravity, ForceMode.Acceleration);
            rb.AddForce(Vector3.up * fuerzaSalto2, ForceMode.Impulse);
            rb.AddForce(Vector3.forward * fuerzaSaltoAdelante, ForceMode.Impulse);
            saltando = true;
            if (ArmaSeleccionada == "Rifle")
            {
                transionActual = 3;
            }
            else
            {
                transionActual = 21;
            }
        }
        else if (Input.GetKey(KeyCode.W) && contadorAndar < 50 )
        {
            velocidad = velocidadAndando;
            movimientoLateral = false;
            saltando = false;

            if (ArmaSeleccionada == "Rifle")
            {
                transionActual = 1;
            }
            else
            {
                transionActual = 17;
            }
                
            contadorAndar++;
        }
        else if (Input.GetKey(KeyCode.W) && contadorAndar >= 30)
        {
            Debug.Log("CORRIENDO");
            //transionActual = 2; 
           // no disponemos de la animacion corriendo
            velocidad = velocidadCorriendo;
        }
        if (Input.GetKey(KeyCode.S))
        {
            velocidad = -velocidadAndando;
            if (ArmaSeleccionada == "Rifle")
            {
                transionActual = 9;
            }
            else
            {
                transionActual = 23;
            }
            movimientoLateral = false;
            saltando = false;
        }


       if (Input.GetKeyUp(KeyCode.W))
        {
            velocidad = 0;
            contadorAndar = 0;
            if (ArmaSeleccionada == "Rifle")
            {
                if (animacion.GetCurrentAnimatorStateInfo(0).IsName("WalkRifle"))
                {
                    transionActual = 5;
                }
                else
                {
                    transionActual = 8;
                }
            }
            else // Pistola
            {
                if (animacion.GetCurrentAnimatorStateInfo(0).IsName("WalkPistol"))
                { // de andar a Idle
                    transionActual = 18;
                }
                else
                { // de correr a andar
                    transionActual = 20;
                }
            }
        }  
        if (Input.GetKeyUp(KeyCode.S))
        {
            velocidad = 0;
            if (ArmaSeleccionada == "Rifle")
            {
                transionActual = 10;// de atras a Idle
            }
            else
            {
                transionActual = 24;
            }
        }

        if (Input.GetKey(KeyCode.R)) // RODAR
        {
            velocidad = velocidadRodando;
            transionActual = 11;
        }
        else if (Input.GetKeyUp(KeyCode.R))
        {
            velocidad = 0;
            
        }

        if(saltando == true && animacion.GetCurrentAnimatorStateInfo(0).IsName("JumpRifle"))
        {
            transionActual = 38;
        } else if(saltando == true && animacion.GetCurrentAnimatorStateInfo(0).IsName("JumpPistol"))
        {
            transionActual = 22;
        }
        AnimatorStateInfo stateInfo = animacion.GetCurrentAnimatorStateInfo(0);
        if (animacion.GetCurrentAnimatorStateInfo(0).IsName("WalkRifle") && velocidad == 0)
        { // de andar a parado
            transionActual = 5;
        }
        else if (animacion.GetCurrentAnimatorStateInfo(0).IsName("Rodando") && stateInfo.normalizedTime >= 1.0f)
        {
            transionActual = 12;
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
        }       
       

        // ROTACION
        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection * 1.5f);
        }
        // rotacion rifle con camara
        float cameraAltura = virtualCamera.transform.position.y;
        float diferenciaAltura = cameraAltura - transform.position.y - diferenciaAlturaInicial;
        Vector3 cameraForwardXZ = new Vector3(virtualCamera.transform.forward.x, 0, virtualCamera.transform.forward.z).normalized;
        float distanciaHorizontal = cameraForwardXZ.magnitude;
        float angle = Mathf.Atan2(diferenciaAltura, distanciaHorizontal) * Mathf.Rad2Deg;
        
        if (angle > -20f && ArmaSeleccionada == "Rifle")
        {
            Rifle.transform.localRotation = Quaternion.Euler(angle, Rifle.transform.localRotation.eulerAngles.y, Rifle.transform.localRotation.eulerAngles.z);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (GameManager.Instance.JuegoPausado == true) return;
        Animar(transionActual);
        RotacionyMovimiento();
        SaltarDispararRodar();       
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
            if(ArmaSeleccionada == "Rifle")
            {
                transionActual = 3;
            }
            else
            {
                transionActual = 21; 
            }            
        }


        if ((Mathf.Abs(velocidad) > 0.1f) && Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Ruedo");
        }

        if (Input.GetKeyDown(KeyCode.X)) // CAMBIO DE ARMA
        {
            
            if (ArmaSeleccionada == "Rifle")
            {
                Debug.Log("cambio de rifle a pistola");
                ArmaSeleccionada = "Pistola";
                transionActual = 13;                             
                Pistola.transform.SetParent(Mano.transform);
            }
            else
            {
                transionActual = 14;
                ArmaSeleccionada = "Rifle";
                Debug.Log("cambio pistola a rifle");
                Pistola.transform.SetParent(transform);
            }
        }

        AnimatorTransitionInfo transitionInfo = animacion.GetAnimatorTransitionInfo(0);
        /*
          ahora queremos que cuando termine la visualizacion del cambio de arma se pase al estado
        de IDLE pero con la pistola
         */
        // Detectar si la transición ha finalizado:
        if (ArmaSeleccionada == "Pistola" && !transitionInfo.IsName("cambioArma1") && animacion.GetCurrentAnimatorStateInfo(0).IsName("CambioArma"))
        {
            transionActual = 15;
            Debug.Log("estoy en IDLe pistola");
            //Debug.Log("La transición ha finalizado y ahora está en el estado final.");
        } /* ahora queremos que si estamos en la animacion de vuleta al rifle, cuando se termine
           que vaya al estado IDLE Rifle con una transicion que es la 16*/
        else if(ArmaSeleccionada == "Rifle" && !transitionInfo.IsName("cambioArma2") && animacion.GetCurrentAnimatorStateInfo(0).IsName("CambioArma")) 
        { 
            transionActual = 16;
            Debug.Log("vuelvo al principio");
        }
        
         //ANIMACION TERMINADA:
         AnimatorStateInfo stateInfo = animacion.GetCurrentAnimatorStateInfo(0);
            // la transicion de cambio de arma esta a la mitad
            if (ArmaSeleccionada == "Pistola" && stateInfo.IsName("CambioArma") && stateInfo.normalizedTime >= 0.5f)
            {
               // lo hacemos para que las armas se cambien de lugar antes de que termine toda la animacion
                AnimDeRifleAPistola();
            
            } else if (ArmaSeleccionada == "Rifle" && stateInfo.IsName("CambioArma") && stateInfo.normalizedTime >= 0.5f)
            {
                AnimDePistolaARifle();          
            }

            if(velocidad == 0 && animacion.GetCurrentAnimatorStateInfo(0).IsName("CambioArma"))
            {
                transionActual = 5;
            }
         

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                // Se hizo clic en un botón u otro UI, ignoramos el evento
                return;
            }
            Disparar(ArmaSeleccionada);
        }
    }
}
