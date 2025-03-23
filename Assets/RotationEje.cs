using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationEje : MonoBehaviour
{
    public Transform targetObject; // Objeto alrededor del cual se va a rotar
    public float rotationSpeed = 5f; // Velocidad de rotación

    void Update()
    {
        // Verificar si el objeto alrededor del cual se va a rotar está asignado
        if (targetObject != null)
        {
            // Rotar este objeto alrededor del objetivo en su eje hacia arriba
            transform.RotateAround(
                targetObject.position,
                Vector3.up,
                rotationSpeed * Time.deltaTime
            );
        }
        else
        {
            Debug.LogWarning("El objeto alrededor del cual se va a rotar no está asignado.");
        }
    }
}
