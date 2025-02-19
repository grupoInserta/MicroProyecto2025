using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    float v;
    float h;

    public Rigidbody rb;
    Input tecla;
    [SerializeField]
    float velocidad = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //player = new GameObject();
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

        /*
        if (h >= 0.1)
        {
            player.transform.position += Vector3.right * velocidad;
        }
        else if( h <= -0.1)
        {
            player.transform.position += Vector3.left * velocidad;
        }
            */
    }

    private void AplicarMovimiento()
    {
        rb.linearVelocity = new Vector3(h * velocidad, rb.linearVelocity.y, v * velocidad);
    }


}
