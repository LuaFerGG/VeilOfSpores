using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    [Header("Suavizado")]
    public float smoothSpeedX = 0.1f; 
    public float smoothSpeedUp = 0.15f;   // más rápido al subir
    public float smoothSpeedDown = 0.15f; // más lento al bajar

    [Header("Zona muerta vertical")]
    public float verticalDeadZone = 2f; // margen en Y donde la cámara no se mueve

    private float cameraY;

    void Start()
    {
        if (target != null)
            cameraY = target.position.y;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // ---- MOVIMIENTO EN X (siempre sigue al jugador) ----
        float desiredX = target.position.x + offset.x;
        float smoothedX = Mathf.Lerp(transform.position.x, desiredX, smoothSpeedX);

        // ---- MOVIMIENTO EN Y (con zona muerta y diferente suavizado) ----
        float deltaY = target.position.y - cameraY;

        if (Mathf.Abs(deltaY) > verticalDeadZone)
        {
            // Actualizar referencia de Y
            cameraY = target.position.y - Mathf.Sign(deltaY) * verticalDeadZone;
        }

        float desiredY = cameraY + offset.y;

        // Elegir suavizado según si sube o baja
        float smoothSpeedY = (desiredY > transform.position.y) ? smoothSpeedUp : smoothSpeedDown;
        float smoothedY = Mathf.Lerp(transform.position.y, desiredY, smoothSpeedY);

        // ---- APLICAR ----
        transform.position = new Vector3(smoothedX, smoothedY, transform.position.z);
    }
}
