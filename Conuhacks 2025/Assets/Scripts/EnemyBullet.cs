using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.CompareTag("Player")) {
            collider.gameObject.GetComponent<Player>().TakeDamage();
            Destroy(this.gameObject);
        }
        else if (collider.gameObject.CompareTag("Outer Wall")) {
            Destroy(this.gameObject);
        }
    }
}
