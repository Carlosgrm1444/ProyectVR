using System.Collections;
using TMPro;
using UnityEngine;

public class TextSegControlle : MonoBehaviour
{
    public GameObject objectToRotate; // Objeto que se orientará hacia la cámara
    public int countdownStartValue = 10; // Valor de inicio de la cuenta regresiva
    public int cantRestar = 5;
    public int countdownValue; // Valor actual de la cuenta regresiva
    private TextMeshPro textMeshPro; // Referencia al componente TextMeshPro
    public GameObject objetPoints; // Objeto que se orientará hacia la cámara
    private TextMeshPro points; // Referencia al componente TextMeshPro

    public int totalPoints = 0;
    public int sumPoints = 10;
    public int totalTime = 0;

    public GameObject spawners;
    public GameObject inventory;

    void Start()
    {
        // Obtener el componente TextMeshPro del objeto
        textMeshPro = objectToRotate.GetComponentInChildren<TextMeshPro>();
        points = objetPoints.GetComponentInChildren<TextMeshPro>();

        // Inicializar la cuenta regresiva con el valor de inicio
        countdownValue = countdownStartValue;

        // Iniciar la cuenta regresiva
        StartCoroutine(StartCountdown());
        StartCoroutine(elevateDificult());
    }

    IEnumerator StartCountdown()
    {
        // Esperar un segundo antes de comenzar el contador
        yield return new WaitForSeconds(1f);

        // Bucle para actualizar la cuenta regresiva
        while (countdownValue > 0)
        {
            // Restar uno al valor de la cuenta regresiva
            countdownValue--;
            totalPoints++;
            totalTime++;

            // Actualizar el texto del TextMeshPro con el valor de la cuenta regresiva
            textMeshPro.text = countdownValue.ToString() + " Seg";
            textMeshPro.color = Color.white;
            points.text = "Puntuacion: " + totalPoints.ToString();
            points.color = Color.white;

            // Esperar un segundo antes de la próxima actualización
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator elevateDificult()
    {
        // Esperar un segundo antes de comenzar el contador
        yield return new WaitForSeconds(1f);

        // Bucle para actualizar la cuenta regresiva
        while (countdownValue > 0)
        {
            if (totalTime % 10 == 0)
            {
                for (int i = 0; i < spawners.transform.childCount; i++)
                {
                    spawners.transform.GetChild(i).GetComponent<Spawner>().spawnSpeed *= 1.1f;
                    spawners.transform.GetChild(i).GetComponent<Spawner>().play = false;
                }
            }
            if (totalTime % 17 == 0)
            {
                inventory.GetComponent<ControlInventory>().cantKillsExtraSegs += 1;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    void Update()
    {
        if (objectToRotate != null && Camera.main != null)
        {
            // Obtener la posición de la cámara
            Vector3 cameraPosition = Camera.main.transform.position;

            // Calcular la dirección hacia la cámara
            Vector3 lookDirection = cameraPosition - objectToRotate.transform.position;
            lookDirection.z *= -1;
            lookDirection.y *= -1;
            lookDirection.x *= -1;

            // Rotar el objeto para que mire hacia la cámara
            objectToRotate.transform.rotation = Quaternion.LookRotation(lookDirection);
        }
    }

    public void restarSeg()
    {
        countdownValue -= cantRestar;
        textMeshPro.text = countdownValue.ToString() + " Seg";
        textMeshPro.color = Color.red;
        totalPoints -= cantRestar;
        points.text = "Puntuacion: " + totalPoints.ToString();
        points.color = Color.red;
    }

    public void sumarSeg()
    {
        countdownValue += 5;
        textMeshPro.text = countdownValue.ToString() + " Seg";
        textMeshPro.color = Color.blue;
    }

    public void masPoints()
    {
        totalPoints += sumPoints;
        points.text = "Puntuacion: " + totalPoints.ToString();
        points.color = Color.blue;
    }
}
