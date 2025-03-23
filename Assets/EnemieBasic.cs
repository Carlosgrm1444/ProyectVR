using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemieBasic : MonoBehaviour
{
    public Transform target; // El objeto que seguirá
    public float followSpeed = 5f; // Velocidad de seguimiento del objeto
    private Rigidbody rb; // El Rigidbody del objeto seguidor
    public GameObject points;
    public GameObject inventory;
    public GameObject extraTime;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Obtener el Rigidbody del objeto seguidor
    }

    void FixedUpdate()
    {
        // Verificar si el objeto a seguir está asignado
        if (target != null)
        {
            // Calcular la dirección hacia el objetivo
            Vector3 directionToTarget = target.position - transform.position;

            // Normalizar la dirección para mantener una velocidad constante
            Vector3 normalizedDirection = directionToTarget.normalized;

            // Calcular la posición a la que se moverá el objeto seguidor
            Vector3 newPosition =
                transform.position + normalizedDirection * followSpeed * Time.deltaTime;

            // Mover el objeto seguidor a la nueva posición
            rb.MovePosition(newPosition);
        }
        else
        {
            Debug.LogWarning("El objetivo para seguir no está asignado.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Verificar si el objeto que entró en contacto tiene el tag "bullet"
        if (other.CompareTag("bullet"))
        {
            if (
                inventory.GetComponent<ControlInventory>().enemyKills
                    % inventory.GetComponent<ControlInventory>().cantKillsExtraSegs
                == 0
            )
            {
                Vector3 spawnPosition = transform.position;
                spawnPosition.y += 0.1f;
                GameObject pwu = Instantiate(extraTime, spawnPosition, Quaternion.identity);
                pwu.SetActive(true);
            }
            inventory.GetComponent<ControlInventory>().enemyKills++;
            points.GetComponentInChildren<TextSegControlle>().masPoints();
            Destroy(gameObject);
        }
        if (other.CompareTag("Player"))
        {
            // Destruir el enemigo

            // Destruir también el objeto de la bala
            other.gameObject.transform.GetChild(0).GetComponent<TextSegControlle>().restarSeg();
            Destroy(gameObject);
        }
    }
}
