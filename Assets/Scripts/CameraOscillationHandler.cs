using UnityEngine;
using Unity.Cinemachine;

public class CameraOscillationHandler : MonoBehaviour
{
    public Transform playerTransform;
    public CinemachineCamera virtualCamera;
    public float oscillationThreshold = 0.2f; // distancia entre frames que se considera "temblor"
    public int oscillationFrameCount = 10;    // cuántos frames seguidos deben superar el umbral
    public float playerMovementThreshold = 0.05f;

    private Vector3 lastCameraPosition;
    private Vector3 lastPlayerPosition;
    private int oscillatingFrames = 0;
    private CinemachineDeoccluder camCollider;

    void Start()
    {
        if (virtualCamera == null)
            virtualCamera = GetComponent<CinemachineCamera>();

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
            Debug.LogWarning("Oscilación detectada. Activando corrección.");
            ApplyCameraCorrection();
            oscillatingFrames = 0;
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
            Invoke(nameof(ReenableCollider), 0.2f); // reactiva después de 0.2 segundos
        }

        // Alternativamente se puede  mover manualmente la cámara a un punto estable:
        // transform.position = playerTransform.position - playerTransform.forward * 5f + Vector3.up * 2f;
    }

    void ReenableCollider()
    {
        if (camCollider != null)
            camCollider.enabled = true;
    }
}
