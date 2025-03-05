using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    PlayerController miplayerController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        miplayerController = GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("municion1"))
        {
            miplayerController.cargarArma(1);

        }
        else if (other.gameObject.CompareTag("municion2"))
        {
            miplayerController.cargarArma(2);
        }
        Destroy(other.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}