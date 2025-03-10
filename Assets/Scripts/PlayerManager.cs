using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    PlayerController miplayerController;
    public int salud;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        miplayerController = GetComponent<PlayerController>();
        salud = 30;
    }

    public void damage(string tipo)
    {
        if(tipo == "enemigo")
        {
            salud -= 5;
            Debug.Log(salud);
        }
        else
        {
            salud -= 10;
        }
        if(salud < 0)
        {
            Debug.Log("PARTIDA PERDIDA!!!!!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Municion1"))
        {
            miplayerController.cargarArma(1);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Municion2"))
        {
            miplayerController.cargarArma(2);
            Destroy(other.gameObject);
        }        
    }

    // Update is called once per frame
    void Update()
    {

    }
}