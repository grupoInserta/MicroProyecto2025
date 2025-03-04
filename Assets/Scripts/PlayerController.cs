using UnityEngine;
using Unity.Cinemachine;


public class PlayerController : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    float v;
    float h;
    public Rigidbody rb;
    [SerializeField]
    float velocidadMax = 5f;
    float velocidad = 0;
    [SerializeField]
    float fuerzaSalto = 5f;


    private string ArmaSeleccionada;
    [SerializeField]
    GameObject salidaBala1;
    [SerializeField]
    GameObject salidaBala2;

    [SerializeField]
    int maximoBalas1;
    [SerializeField]
    int maximoBalas2;

    int balasActuales1;
    int balasActuales2;

    [SerializeField]
    public GameObject Bala1;
    [SerializeField]
    public GameObject Bala2;

    [SerializeField]
    public float VelocdiadBala;

    [SerializeField]
    private CinemachineCamera virtualCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        balasActuales1 = maximoBalas1;
        balasActuales2 = maximoBalas2;
        rb = GetComponent<Rigidbody>();
        ArmaSeleccionada = "Rifle";
    }

    private void Disparar(string tipoArma)
    {

        if (tipoArma == "Rifle")
        {
            balasActuales1--;
            GameObject bala = Instantiate(Bala1) as GameObject;
            bala.transform.position = salidaBala1.transform.position;
            bala.GetComponent<Bala>().configurarDisparo(VelocdiadBala, transform.forward);
        }
        else
        {
            balasActuales2--;
            GameObject bala = Instantiate(Bala2) as GameObject;
            bala.transform.position = salidaBala2.transform.position;
            bala.GetComponent<Bala>().configurarDisparo(VelocdiadBala, transform.forward);
        }

    }

    private void RotacionyMovimiento()
    {   // Calcular la direcci�n del movimiento basada en la c�mara (solo forward)
        Vector3 cameraForward = virtualCamera.transform.forward;
        Debug.Log(cameraForward);
        cameraForward.y = 0; // Ignorar la componente Y para que el movimiento sea en el plano horizontal
        cameraForward.Normalize();

        Vector3 moveDirection = cameraForward; // La direcci�n es siempre forward
        if (Input.GetKey(KeyCode.W))
        {
            rb.linearVelocity = moveDirection * velocidadMax;
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.linearVelocity = moveDirection * -velocidadMax;
        }
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            rb.linearVelocity = Vector3.zero;
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
    }


    private void SaltarDispararRodar()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
            Debug.Log("SALTO");
        }

        if ((Mathf.Abs(velocidad * h) > 0.1f || Mathf.Abs(velocidad * v) > 0.1f) && Input.GetKeyDown(KeyCode.X))
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
