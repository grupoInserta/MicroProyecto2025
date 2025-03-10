using UnityEngine;

public class Estado : MonoBehaviour
{
    protected GameObject Enemigo;

    protected void Awake()
    {
        this.enabled = false;
        Enemigo = this.gameObject;
    }
}