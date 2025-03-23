using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour
{
    public bool play = true;
    public float spawnSpeed = 1f; // Velocidad de spawn
    public GameObject[] objectsToSpawn; // Lista de objetos a spawnear
    public int spawnIndex = 0; // Índice del objeto a spawnear

    private float nextSpawnTime; // Tiempo para el próximo spawn

    void Update()
    {
        // Verificar si ha llegado el momento de spawnear
        if (Time.time >= nextSpawnTime)
        {
            // Spawnear el objeto en el índice especificado
            SpawnObject();

            // Calcular el tiempo para el próximo spawn
            nextSpawnTime = Time.time + 1f / spawnSpeed;
        }
    }

    void SpawnObject()
    {
        // Verificar si el índice es válido
        if (spawnIndex >= 0 && spawnIndex < objectsToSpawn.Length)
        {
            // Spawnear el objeto en la posición del spawner y sin rotación
            GameObject spawnedObject = Instantiate(
                objectsToSpawn[spawnIndex],
                transform.position,
                Quaternion.identity
            );

            // Activar el objeto spawnear
            spawnedObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Índice de spawn no válido.");
        }
    }

    public void reload(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && play == false)
        {
            SceneManager.LoadScene("New Scene");
            Debug.Log("Formateo");
        }
        Debug.Log("inten Formateo" + play);
    }
}
