using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    float v;
    float h;

    CharacterController CaracterController;
    public Rigidbody rb;
    [SerializeField]
    float velocidad = 5f;
    [SerializeField]
    float fuerzaSalto = 5f;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CaracterController = GetComponent<CharacterController>();
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
            rb.AddForce(Vector3.up* fuerzaSalto, ForceMode.Impulse );
            Debug.Log("SALTO");
        }

        if((Mathf.Abs(velocidad * h) > 0.1f || Mathf.Abs(velocidad * v) > 0.1f) && Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Ruedo");
        }
    }

   
    private void AplicarMovimiento()
    {
        rb.linearVelocity = new Vector3(h * velocidad, rb.linearVelocity.y, v * velocidad);
    }


}
