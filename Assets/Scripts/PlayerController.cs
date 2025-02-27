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




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {       
        balasActuales = maximoBalas;        
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
