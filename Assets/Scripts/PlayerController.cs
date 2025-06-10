using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerController : MonoBehaviour
{
    [SerializeField]
    GameObject player; // quitar porque se utiliza el componente transform directamente...
    public Rigidbody rb;
    [SerializeField]
    float velocidadAndando = 5f;
    float velocidadRodando = 7f;
    float velocidad = 0;
    float velocidadLateral = 2f;
    private float velModelRollingInvers = 2.9f;// movimiento del modelo hacia atras en animacion rolling para ajustar posiciones
    private bool movimientoLateral;
    [SerializeField]
    float fuerzaSalto = 7f;//// es la fuerza de salto up
    private float extraGravity = 20f;
    private float Gravity = 16f;
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
    public GameObject PosPistola2;
    private Vector3 PosfIniRifle;
    private Quaternion RotIniRifle;
    private Vector3 PosIniPistola;
    private Quaternion RotIniPistola;
    private AudioSource audioSource;
    public AudioClip cambioArma;
    private Vector3 moveDirection;
    public AudioClip recargaArma;
    //
    [SerializeField]
    public GameObject Mano;
    [SerializeField]
    public GameObject Bala1;
    [SerializeField]
    public GameObject BalaPartic;
    [SerializeField]
    public GameObject Bala2;
    [SerializeField]
    public float VelocdiadBala;
    [SerializeField]
    private CinemachineCamera virtualCamera;
    private bool saltando;
    private bool cayendo;// del parkour del segundo nivel
    private bool bajando;// de la placa roja del segundo nivel
    private Animator animacion;
    private int transionActual;
    private Puerta PuertaObjetoScript;
    private Puerta Puerta2ObjetoScript;
    private PlayerManager playerManager;
    private float diferenciaAlturaInicial;
    private float duracionCambioArma = 0.7f;
    private int contadorPlacaPulsada;
    private bool animacionCambio;
    private CapsuleCollider capsuleCollider;
    private BoxCollider boxCollider;
    private float radioCapsuleCollider;
    private float alturaBoxcollider;
    private Vector3 TamanioBoxCollider;
    // Rodar
    public float moveSpeed = 5f;
    private bool isRolling = false;
    private Vector3 PlayerModelPosicIni;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerModelPosicIni = PlayerModel.transform.localPosition;
        audioSource = GetComponent<AudioSource>();
        playerManager = gameObject.GetComponent<PlayerManager>();
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        boxCollider = GetComponent<BoxCollider>();
        radioCapsuleCollider = capsuleCollider.radius;
        alturaBoxcollider = boxCollider.size.y;
        TamanioBoxCollider = boxCollider.size;
        ArmaSeleccionada = "Rifle";
        movimientoLateral = false;
        saltando = false;
        cayendo = false;
        bajando = false;
        animacion = transform.GetChild(0).GetComponent<Animator>();
        animacion.applyRootMotion = true;
        transionActual = 0;
        PuertaObjetoScript = GameObject.FindWithTag("Puerta").GetComponent<Puerta>();
        Puerta2ObjetoScript = GameObject.FindWithTag("Puerta2").GetComponent<Puerta>();
        salidaBalaR = GameObject.FindWithTag("SalidaBalaR");
        salidaBalaP = GameObject.FindWithTag("SalidaBalaP");
        float cameraAltura = virtualCamera.transform.position.y;
        diferenciaAlturaInicial = cameraAltura - transform.position.y;
        animacionCambio = false;
        contadorPlacaPulsada = 0;
        StartCoroutine(RecordInitialAfterFrame());
    }

    private IEnumerator RecordInitialAfterFrame()
    {
        yield return null; // esperar a que se acomode el padre
        PosfIniRifle = Rifle.transform.localPosition;
        RotIniRifle = Rifle.transform.localRotation;
        PosIniPistola = Pistola.transform.localPosition;
        RotIniPistola = Pistola.transform.localRotation;
    }

    private void AnimDePistolaARifle()
    {
        StartCoroutine(MoveAndRotateRoutine(Pistola, Pistola.transform.localPosition, PosIniPistola, Pistola.transform.localRotation, RotIniPistola));
        StartCoroutine(MoveAndRotateRoutine(Rifle, Rifle.transform.localPosition, PosfIniRifle, Rifle.transform.localRotation, RotIniRifle));
        if (audioSource != null)
        {
            audioSource.clip = cambioArma;
            audioSource.Play();
        }
    }

    private void AnimDeRifleAPistola()
    {
        StartCoroutine(MoveAndRotateRoutine(Pistola, Pistola.transform.localPosition, PosPistola2.transform.localPosition, Pistola.transform.localRotation, PosPistola2.transform.localRotation));
        StartCoroutine(MoveAndRotateRoutine(Rifle, Rifle.transform.localPosition, PosRifle2.transform.localPosition, Rifle.transform.localRotation, PosRifle2.transform.localRotation));
        if (audioSource != null)
        {
            audioSource.clip = cambioArma;
            audioSource.Play();
        }
    }

    private IEnumerator MoveAndRotateRoutine(GameObject obj, Vector3 PosIniObj, Vector3 PosFinalObj, Quaternion RotIniObj, Quaternion RotFinalObj)
    {
        Vector3 startPos = PosIniObj;
        Quaternion startRot = RotIniObj;
        float elapsed = 0f;
        while (elapsed < duracionCambioArma)
        {
            float t = elapsed / duracionCambioArma;
            // Puedes usar curvas para easing (por ejemplo, Mathf.SmoothStep)
            obj.transform.localPosition = Vector3.Lerp(startPos, PosFinalObj, t);
            obj.transform.localRotation = Quaternion.Slerp(startRot, RotFinalObj, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        // Asegurar posición y rotación final
        obj.transform.localPosition = PosFinalObj;
        obj.transform.localRotation = RotFinalObj;
        //moveRoutine = null;
    }


    private void Disparar(string tipoArma)
    {
        if (tipoArma == "Rifle")
        {
            if (playerManager.balasActualesR == 0) return;
            playerManager.balasActualesR--;
            GameObject bala = Instantiate(Bala1, transform.position, transform.rotation);
            GameObject balaPart = Instantiate(BalaPartic, transform.position, transform.rotation);
            Destroy(balaPart, 2f);
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

    /* se llama cuando salta y en este caso se desactivan los colliders
     para volverse a activa arriba del todo
     y tambien cunado en el parkour toca con la zona de caida para que no estorben
     los colliders de las plataformas 
    */
    private void disminuirColliders()
    {
        capsuleCollider.radius = 0.1f;
        boxCollider.size = new Vector3(0.2f, alturaBoxcollider, 0.2f);
    }

    private void restaurarColliders()
    {
        capsuleCollider.radius = radioCapsuleCollider;
        boxCollider.size = TamanioBoxCollider;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Plataforma") && !saltando)
        {
            bajando = true; // de la plataforma
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Suelo") || other.CompareTag("Plataforma"))
        {
            if (saltando && other.CompareTag("Suelo"))
            {
                transionActual = 7;
                restaurarColliders();
            }
            else if (other.CompareTag("Plataforma"))
            {
                capsuleCollider.enabled = true;
                boxCollider.enabled = true;
            }
            else if (other.CompareTag("Suelo"))
            {
                restaurarColliders();
            }

            saltando = false;
            bajando = false;

            if (cayendo)
            {
                cayendo = false;
                GameManager.Instance.reinicioEscena();
                PuertaObjetoScript.restablecerPosicPuerta();
                restaurarColliders();
            }
        }
        else if (other.CompareTag("zonaCaida"))
        { /* cambiar esto por ontriggerstay?? */
            cayendo = true;
            saltando = false;
            disminuirColliders();
            Debug.Log("estoy en zona caida");
        }

        if (other.CompareTag("Placa"))
        {
            contadorPlacaPulsada++;
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
            if (contadorPlacaPulsada == 2)
            {
                PuertaObjetoScript.IniciarDesplazamiento(1);// tercer Nivel                
                Debug.Log("contador placa pulsada: " + contadorPlacaPulsada + " escena: " + GameManager.Instance.escenaActual);
            }
            else if (contadorPlacaPulsada == 1 && GameManager.Instance.escenaActual == "SegundoNivel")
            {
                PuertaObjetoScript.IniciarDesplazamiento(1);// segundo Nivel
            }
        }
        else if (other.CompareTag("Llave"))
        {
            playerManager.CambiarLuces();
            other.gameObject.SetActive(false);
            Puerta2ObjetoScript.IniciarDesplazamiento(2);
            audioSource.clip = playerManager.obtenerVida;
            audioSource.Play();
        }
    }

    private void Animar(int transicion)
    {
        animacion.SetInteger("Transicion", transicion);
    }

    private void RotacionyMovimiento()
    {

        // Calcular la direcci�n del movimiento basada en la c�mara (solo forward)
        Vector3 cameraForward = virtualCamera.transform.forward;
        cameraForward.y = 0; // Ignorar la componente Y para que el movimiento sea en el plano horizontal
        cameraForward.Normalize();

        if (Input.GetKeyDown(KeyCode.R)) // CLIC RODAR
        {
            velocidad = 0;
            isRolling = true;
            transionActual = 11;
        }
        moveDirection = cameraForward; // La direcci�n es siempre forward        

        if (Input.GetKey(KeyCode.W) && !isRolling)
        {            
            if (saltando)
            {
                velocidad = velocidadAndando + 1f;// velocidad cuando salta
            }
            else
            {
                velocidad = velocidadAndando;
            }
            movimientoLateral = false;
            bajando = false;
            if (ArmaSeleccionada == "Rifle")
            {
                transionActual = 1;
            }
            else
            {
                transionActual = 17;
            }
        }

        if (!saltando)
        {
            rb.AddForce(Vector3.down * Gravity, ForceMode.Acceleration);
        }
        else
        {
            rb.AddForce(Vector3.down * extraGravity, ForceMode.Acceleration);
        }
        if (bajando) // sale de una placa o de una plataforma
        {
            rb.AddForce(Vector3.down * extraGravity * 5f, ForceMode.Acceleration);
        }

        // saber si en un salto estamos arriba del todo:
        if (saltando && rb.linearVelocity.y > 0)
        {
            capsuleCollider.enabled = true;
            boxCollider.enabled = true;
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

        if (saltando == true && animacion.GetCurrentAnimatorStateInfo(0).IsName("JumpRifle"))
        {
            transionActual = 38;
        }
        else if (saltando == true && animacion.GetCurrentAnimatorStateInfo(0).IsName("JumpPistol"))
        {
            transionActual = 22;
        }
        AnimatorStateInfo stateInfo = animacion.GetCurrentAnimatorStateInfo(0);
        if (animacion.GetCurrentAnimatorStateInfo(0).IsName("WalkRifle") && velocidad == 0)
        { // de andar a parado
            transionActual = 5;
        }

        Vector3 perpendicular = new Vector3(-moveDirection.z, 0, moveDirection.x).normalized;
        if (Input.GetKey(KeyCode.D))
        {
            rb.linearVelocity = perpendicular * -velocidadLateral;
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

        if (movimientoLateral == false && saltando == false && cayendo == false && isRolling == false)
        {
            rb.linearVelocity = moveDirection * velocidad;
        }

        // ROTACION
        if (moveDirection != Vector3.zero)
        {
            moveDirection.x *= 2f; // Amplifica el bajandoeje X para más sensibilidad lateral
            moveDirection = moveDirection.normalized;
            transform.rotation = Quaternion.LookRotation(moveDirection);
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

    private void controlarRodar()
    {
        // el tiempo en el que esta rodando
        if (animacion.GetCurrentAnimatorStateInfo(0).IsName("Rodando") &&
            animacion.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.02f && animacion.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.7f)
        {
            Rifle.SetActive(false);
            Pistola.SetActive(false);
            rb.linearVelocity = moveDirection * velocidadRodando;
            PlayerModel.transform.position -= PlayerModel.transform.forward * velModelRollingInvers * Time.deltaTime;
        }
        else if (animacion.GetCurrentAnimatorStateInfo(0).IsName("Rodando") &&
             animacion.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.98f)
        {
            // Debug.Log("fin animacion rodar");
            animacion.Play("IDL");
            PlayerModel.transform.localPosition = PlayerModelPosicIni;
            isRolling = false;
            transionActual = 0;
            Rifle.SetActive(true);
            Pistola.SetActive(true);
        }
    }

    void FixedUpdate()
    {
        if (cayendo)
        {
            Vector3 gravity = Vector3.down * extraGravity * 1.5f;
            rb.AddForce(gravity, ForceMode.Acceleration);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.JuegoPausado == true) return;
        Animar(transionActual);
        if (isRolling)
        {
            controlarRodar();
        }
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
        audioSource.clip = recargaArma;
        audioSource.Play();
    }

    private void SaltarDispararRodar()
    {
        if (Input.GetKeyDown(KeyCode.Space) && saltando == false)
        {
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
            saltando = true;
            disminuirColliders();
            boxCollider.enabled = false;
            capsuleCollider.enabled = false;

            if (ArmaSeleccionada == "Rifle")
            {
                transionActual = 3;
            }
            else
            {
                transionActual = 21;
            }
        }


        if (Input.GetKeyDown(KeyCode.X)) // CAMBIO DE ARMA
        {
            if (ArmaSeleccionada == "Rifle")
            {
                // Debug.Log("cambio de rifle a pistola");
                AnimDeRifleAPistola();
                ArmaSeleccionada = "Pistola";
                transionActual = 13;
                animacionCambio = true;
            }
            else
            {
                transionActual = 14;
                ArmaSeleccionada = "Rifle";
                // Debug.Log("cambio pistola a rifle");
                AnimDePistolaARifle();
                animacionCambio = true;
            }
        }
        /**** AC ANIMACIONES CUERPO AUTOMATICAS; SIN PULSAR TECLA ****/
        AnimatorTransitionInfo transitionInfo = animacion.GetAnimatorTransitionInfo(0);
        AnimatorStateInfo stateInfo = animacion.GetCurrentAnimatorStateInfo(0);
        /*
          ahora queremos que cuando termine la visualizacion del cambio de arma se pase al estado
        de IDLE pero con la pistola
         */
        // Detectar si la transición ha finalizado:
        if (ArmaSeleccionada == "Pistola" && !transitionInfo.IsName("cambioArma1") && animacion.GetCurrentAnimatorStateInfo(0).IsName("CambioArma") && animacionCambio)
        {
            animacionCambio = false;
            transionActual = 15;
            //Debug.Log("voy a IDLe pistola");
        } /* ahora queremos que si estamos en la animacion de vuleta al rifle, cuando se termine
           que vaya al estado IDLE Rifle con una transicion que es la 16*/
        else if (ArmaSeleccionada == "Rifle" && !transitionInfo.IsName("cambioArma2") && animacion.GetCurrentAnimatorStateInfo(0).IsName("CambioArma") && animacionCambio)
        {
            transionActual = 16;
            //Debug.Log("vuelvo al principio de armas");
            animacionCambio = false;
        }
        /*** FIN AC ***/

        /*** SA ANIMACIONES ARMA SOLAMENTE ***/
        // la transicion de cambio de arma esta a la mitad

        if (ArmaSeleccionada == "Pistola" && stateInfo.IsName("CambioArma") && stateInfo.normalizedTime >= 0.5f && !animacionCambio)
        {
            // lo hacemos para que las armas se cambien de lugar antes de que termine toda la animacion
            animacionCambio = true;
            // Debug.Log("voy por la mitad");
        }
        else if (ArmaSeleccionada == "Rifle" && stateInfo.IsName("CambioArma") && stateInfo.normalizedTime >= 0.5f && !animacionCambio)
        {
            animacionCambio = true;
        }

        /*** SA Fin ANIMACIONES ARMA SOLO ***/

        if (velocidad == 0 && animacion.GetCurrentAnimatorStateInfo(0).IsName("WalkRifle"))
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
