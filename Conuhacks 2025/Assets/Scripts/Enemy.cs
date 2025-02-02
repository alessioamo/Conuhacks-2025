using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    Vector3 initialPosition;
    Vector3 finalPosition;
    bool movingToFinalPosition = true;

    Player player;
    
    void Start()
    {
        initialPosition = transform.position;
        finalPosition = initialPosition + new Vector3(patrolDistance, 0, 0);

        player = FindFirstObjectByType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Shoot();
    }

    public bool doesStand;
    public bool doesPatrol;
    public bool doesFollow;

    public int enemyHealth;
    public float speed;

    public void TakeDamage(int damage) {
        enemyHealth -= damage;
        if (enemyHealth <= 0) {
            EnemyDeath();
        }
    }

    private void EnemyDeath() {
        Destroy(this.gameObject);
        GameController.Instance.AddMoney(5);
        AudioController.instance.StopMusic();
        AudioController.instance.ChangeMusic(1);
        SceneManager.LoadScene(1);
    }

    float patrolDistance = 5;
    private void Move() {
        if (doesStand) {
            
        }
        else if (doesPatrol) {
            PatrolAction();
        }
        else if (doesFollow) {
            FollowAction();
        }
    }

    private void PatrolAction() {
        Vector3 targetPosition = movingToFinalPosition ? finalPosition : initialPosition;

        targetPosition.y = transform.position.y;

        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, step);

        if (movingToFinalPosition) {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (Vector2.Distance(transform.position, targetPosition) < 0.1f) {
            movingToFinalPosition = !movingToFinalPosition;
        }
    }

    public float offsetDistance = 2.0f;

    private void FollowAction() {
        Vector3 targetPosition = player.transform.position;

        Vector3 direction = transform.position - targetPosition;

        direction.Normalize();
        targetPosition = player.transform.position + direction * offsetDistance;

        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, step);

        if (transform.position.x - targetPosition.x < 0) {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (transform.position.x - targetPosition.x > 0) {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    bool canShoot = true;
    public float shootCooldown = 0.5f;
    private void Shoot() {
        if (canShoot) {
            StartCoroutine(EnemyShoot());
        }
    }

    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed;
    IEnumerator EnemyShoot() {
        canShoot = false;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        Vector3 playerPosition = player.transform.position;
        playerPosition.z = 0;
        Vector3 direction = (playerPosition - bullet.transform.position).normalized;

        if (rb != null) {
            rb.linearVelocity = direction * bulletSpeed;
        }

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle -= 90;

        bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        yield return new WaitForSeconds(shootCooldown);

        canShoot = true;
    }
}
