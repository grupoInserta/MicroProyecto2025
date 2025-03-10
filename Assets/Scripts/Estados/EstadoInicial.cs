using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstadoInicial : Estado
{
    MaquinaDeEstados maquinaDeEstados;
    // Start is called before the first frame update
    void Start()
    {
        maquinaDeEstados = Enemigo.GetComponent("MaquinaDeEstados") as MaquinaDeEstados;
        maquinaDeEstados.IniciarPatrulla();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.fixedTime == Mathf.Round(4))
        {
            maquinaDeEstados.IniciarPatrulla();
        }
    }
}
