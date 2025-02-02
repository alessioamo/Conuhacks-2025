using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    void Start()
    {
        playerHealth = maxHealth;

        AudioController.instance.StopMusic();
        AudioController.instance.ChangeMusic(2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool canShoot = true;
    public float shootCooldown = 1f;
    public void Fire() {
        if (canShoot) {
            StartCoroutine(PlayerShoot());
        }
    }

    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed;
    public int bulletDamage;
    IEnumerator PlayerShoot() {
        canShoot = false;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector3 direction = (mousePosition - bullet.transform.position).normalized;

        if (rb != null) {
            rb.linearVelocity = direction * bulletSpeed;
        }

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle -= 90;

        bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        yield return new WaitForSeconds(shootCooldown);

        canShoot = true;
    }

    int maxHealth = 3;
    public int playerHealth;
    public void TakeDamage() {
        playerHealth--;
        if (playerHealth <= 0) {
            PlayerDeath();
        }
    }

    void PlayerDeath() {
        GameController.Instance.DeductMoney(GameController.Instance.playerBalance/2);
        AudioController.instance.StopMusic();
        AudioController.instance.ChangeMusic(1);
        SceneManager.LoadScene(1);
    }

    public void Heal() {
        playerHealth++;
        if (playerHealth >= maxHealth) {
            playerHealth = maxHealth;
        }
    }
}
