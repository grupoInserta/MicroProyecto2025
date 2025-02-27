using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class miCamara : MonoBehaviour
{
    float RotationMin = -20f;
    float RotationMax = 20f;
    float smoothTime = 0.12f;

    public float Yaxis;
    public float Xaxis;
    //public float RotationSensitivity = 8f; 
    public float RotationSensitivity;
    [SerializeField]
    private Transform target;// el Jugador
    Vector3 targetRotation;
    Vector3 currentVel;
    private PlayerController playerControllerScript;
  

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript =  target.GetComponent<PlayerController>();

    }  

    // Update is called once per frame
    void LateUpdate()
    {
        Debug.Log("muevo camara");

        Yaxis += Input.GetAxis("Mouse X") * RotationSensitivity;
        Xaxis -= Input.GetAxis("Mouse Y") * RotationSensitivity;
        Xaxis = Mathf.Clamp(Xaxis, RotationMin, RotationMax);
        targetRotation = Vector3.SmoothDamp(targetRotation, new Vector3(Xaxis, Yaxis), ref currentVel, smoothTime);
        transform.eulerAngles = targetRotation;
        transform.position = target.position - transform.forward * 2f;
        Vector3 mover = new Vector3(0.7f, 0.6f, 0.4f);
        transform.Translate(mover);
    }
}
