using UnityEngine;
using Unity.Cinemachine;

public class CameraOscillationHandler : MonoBehaviour
{
    public Transform playerTransform;
    public CinemachineCamera virtualCamera;
    private float oscillationThreshold = 0.1f; // distancia entre frames que se considera "temblor"
    public int oscillationFrameCount = 10;    // cu�ntos frames seguidos deben superar el umbral
    public float playerMovementThreshold = 0.05f;
    private float rotationThreshold = 2.0f;
    private Quaternion expectedRotation;

    private Vector3 lastCameraPosition;
    private Vector3 lastPlayerPosition;
    private int oscillatingFrames = 0;
    private CinemachineDeoccluder camCollider;

    void Start()
    {
        if (virtualCamera == null)
            virtualCamera = GetComponent<CinemachineCamera>();
        expectedRotation = playerTransform.rotation;
        camCollider = virtualCamera.GetComponent<CinemachineDeoccluder>();
        lastCameraPosition = transform.position;
        lastPlayerPosition = playerTransform.position;
    }

    void LateUpdate()
    {
        float cameraMovement = Vector3.Distance(transform.position, lastCameraPosition);
        float playerMovement = Vector3.Distance(playerTransform.position, lastPlayerPosition);

        if (cameraMovement > oscillationThreshold && playerMovement < playerMovementThreshold)
        {
            oscillatingFrames++;
        }
        else
        {
            oscillatingFrames = 0;
        }

        if (oscillatingFrames >= oscillationFrameCount)
        {
            Debug.Log("Oscilaci�n detectada. Activando correcci�n.");
            ApplyCameraCorrection();
            oscillatingFrames = 0;
        }

        float angleDifference = Quaternion.Angle(expectedRotation, transform.rotation);
        if (angleDifference > rotationThreshold)
        {
            Debug.Log("Oscilaci�n rotacional detectada.");
            ApplyCameraCorrection();
            // Aplicar correcci�n o suavizado si es necesario
        }
       

        lastCameraPosition = transform.position;
        lastPlayerPosition = playerTransform.position;
    }

    void ApplyCameraCorrection()
    {
        if (camCollider != null)
        {
            // Desactiva temporalmente el Collider para que se estabilice
            camCollider.enabled = false;
            Invoke(nameof(ReenableCollider), 0.2f); // reactiva despu�s de 0.2 segundos
        }

        // Alternativamente se puede  mover manualmente la c�mara a un punto estable:
        // transform.position = playerTransform.position - playerTransform.forward * 5f + Vector3.up * 2f;
    }

    void ReenableCollider()
    {
        if (camCollider != null)
            camCollider.enabled = true;
    }
}
