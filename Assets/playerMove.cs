using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;

public class playerMove : MonoBehaviour
{
    private Rigidbody rb;
    private Transform vrCamera;

    // private PlayerInput playerInput;

    public float upForce = 250f,
        forceMode = 10f,
        maxSpeed = 5f;

    private Vector2 move;

    public GameObject text;
    public GameObject data;
    public GameObject spawners;
    public GameObject finalTexts;
    private bool play = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        vrCamera = Camera.main.transform;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        move = new Vector2(verticalInput, horizontalInput);

        Vector3 forward = vrCamera.forward;
        Vector3 right = vrCamera.right;
        // float horizontalInput = playerInput.actions["Horizontal"].ReadValue<float>() * -1;
        // float verticalInput = playerInput.actions["Vertical"].ReadValue<float>() * -1;
        // rotate = new Vector2(rotatev, rotateh);
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 desiredMoveDirection = forward * verticalInput + right * horizontalInput;

        move = new Vector2(desiredMoveDirection.x, desiredMoveDirection.z);

        if (move != Vector2.zero)
        {
            float angle = Mathf.Atan2(move.x, move.y) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, angle, 0f);
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * 10f
            );
        }
        if (data.GetComponent<TextSegControlle>().countdownValue <= 0)
        {
            for (int i = 0; i < spawners.transform.childCount; i++)
            {
                spawners.transform.GetChild(i).GetComponent<Spawner>().spawnSpeed = 0;
            }
            int savedHighScore = PlayerPrefs.GetInt("HighScore", 0);

            int newHighScore = text.GetComponent<TextSegControlle>().totalPoints;

            if (newHighScore > savedHighScore)
            {
                PlayerPrefs.SetInt("HighScore", newHighScore);

                finalTexts.transform.GetChild(0).GetComponent<TextMeshPro>().text = "Felicidades!!";
                finalTexts.transform.GetChild(1).GetComponent<TextMeshPro>().text =
                    "Nueva puntuacion: " + newHighScore;
            }
            else
            {
                finalTexts.transform.GetChild(0).GetComponent<TextMeshPro>().text =
                    "Tu puntuacion fue: " + newHighScore;
                finalTexts.transform.GetChild(1).GetComponent<TextMeshPro>().text =
                    "Puntuacion maxima: " + savedHighScore;
            }
            finalTexts.SetActive(true);
            spawners.transform.GetChild(0).GetComponent<Spawner>().play = false;
            Destroy(gameObject, 0);
        }
    }

    private void FixedUpdate()
    {
        Vector3 currentVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Si la magnitud de la velocidad actual supera el límite, detén el incremento
        if (currentVelocity.magnitude >= maxSpeed)
        {
            // Normaliza la velocidad en el plano XY y luego la multiplica por el límite máximo
            rb.velocity = currentVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;
        }
        else
        {
            // Si la velocidad no supera el límite, continúa aplicando la fuerza
            Vector3 forceToAdd = new Vector3(move.x, 0f, move.y) * forceMode;
            rb.AddForce(forceToAdd);
        }
    }

    public void Jump(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            if (CheckGround.isGrounded)
            {
                Debug.Log("Salto");
                rb.AddForce(Vector3.up * upForce);
            }
            else
            {
                Debug.Log("No salto: " + CheckGround.isGrounded);
            }
        }
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ExtraTime"))
        {
            Destroy(other.gameObject);
            text.GetComponent<TextSegControlle>().sumarSeg();
        }
    }
}
