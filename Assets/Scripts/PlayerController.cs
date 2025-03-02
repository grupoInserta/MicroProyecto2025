using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    float v;
    float h;
    public Rigidbody rb;
    [SerializeField]
    float velocidad = 5f;
    [SerializeField]
    float fuerzaSalto = 5f;
    [SerializeField]
    GameObject salidaBala;
    [SerializeField]
    int maximoBalas;
    int balasActuales;
    [SerializeField]
    public GameObject Bala;
    [SerializeField]
    public float VelocdiadBala;

    public float speed = 5f;
    public float rotationSpeed = 10f;

    private CharacterController controller;
    private Transform cameraTransform;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {       
        balasActuales = maximoBalas;
        controller = GetComponent<CharacterController>();
        cameraTransform = GetComponent<Transform>();
    }

    private void Disparar()
    {
        balasActuales--;
        GameObject bala = Instantiate(Bala) as GameObject;
        bala.transform.position = salidaBala.transform.position;
        bala.GetComponent<Bala>().configurarDisparo(VelocdiadBala, transform.forward);


    }
    // Update is called once per frame
    void Update()
    {
        Movimiento();
        Rotacion();
    }

    private void Rotacion()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            // Rotar el jugador en la dirección de la cámara
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {   
        AplicarMovimiento();    
    }

    private void Movimiento()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
            Debug.Log("SALTO");
        }

        if ((Mathf.Abs(velocidad * h) > 0.1f || Mathf.Abs(velocidad * v) > 0.1f) && Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Ruedo");
        }


        if (Input.GetMouseButtonDown(0))
        {
            Disparar();
        }
    }


    private void AplicarMovimiento()
    {
        rb.linearVelocity = new Vector3(h * velocidad, rb.linearVelocity.y, v * velocidad);
    }


}
