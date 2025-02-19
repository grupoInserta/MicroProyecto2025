using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    float v;
    float h;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = new GameObject();
    }

    // Update is called once per frame
    void Update()
    {
        Movimiento();
    }

    private void Movimiento()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        if (h >= 0.1)
        {
            
        }
    }

}
