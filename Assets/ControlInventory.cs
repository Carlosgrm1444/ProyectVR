using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlInventory : MonoBehaviour
{
    public int enemyKills = 0;
    public int cantKillsExtraSegs = 5;

    public Transform objectToRotate; // Objeto que rotará
    public Transform cameraTransform; // Transform de la cámara
    private PlayerInput playerInput; // Referencia al PlayerInput

    void Start()
    {
        // Obtener la referencia al PlayerInput
        playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        // Obtener la entrada del eje del joystick
        float horizontalInput = playerInput.actions["RotateH"].ReadValue<float>() * 1;
        float verticalInput = playerInput.actions["RotateV"].ReadValue<float>() * -1;

        // Calcular el ángulo basado en la entrada del joystick
        float angleInRadians = Mathf.Atan2(verticalInput, horizontalInput);

        // Convertir el ángulo de radianes a grados y asegurarse de que esté en el rango de 0 a 360 grados
        float angleInDegrees = (angleInRadians * Mathf.Rad2Deg + 360) % 360;
        // Debug.Log(angleInDegrees);

        // Obtener la rotación actual de la cámara
        Quaternion cameraRotation = cameraTransform.rotation;

        // Crear una rotación basada en la rotación de la cámara y el ángulo calculado, y asignarla al objeto que rotará
        Quaternion targetRotation = Quaternion.Euler(
            0f,
            angleInDegrees + cameraRotation.eulerAngles.y,
            0f
        );
        objectToRotate.rotation = targetRotation;
        UnityEngine.Debug.Log(horizontalInput);
        UnityEngine.Debug.Log(verticalInput);
    }
}
