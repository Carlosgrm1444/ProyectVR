using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Arms : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab de la bala
    public Transform inventory;
    public float bulletSpeed = 10f; // Velocidad de la bala
    public float bulletLifetime = 2f; // Tiempo de vida de la bala
    public float childNoTime = 2f; // Tiempo de vida de la bala

    public void Shoot(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            Vector3 shootDirection = inventory.right;

            // Instanciar una nueva bala en la posición del objeto inventory y con la rotación del mismo
            GameObject bullet = Instantiate(bulletPrefab, inventory.position, inventory.rotation);
            bullet.SetActive(true);

            // Establecer el objeto bala como hijo del objeto inventory
            bullet.transform.parent = inventory;

            // Obtener el componente Rigidbody de la bala
            Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();

            // Aplicar fuerza a la bala en la dirección especificada
            bulletRigidbody.velocity = shootDirection * bulletSpeed;

            // Destruir la conexión padre-hijo después de un cierto tiempo
            Destroy(bullet, bulletLifetime);
            StartCoroutine(UnparentBulletAfterTime(bullet, childNoTime));
        }
    }

    IEnumerator UnparentBulletAfterTime(GameObject bullet, float time)
    {
        // Esperar durante el tiempo de vida de la bala
        yield return new WaitForSeconds(time);

        // Eliminar la conexión padre-hijo
        bullet.transform.parent = null;
    }
}
