using Unity.VisualScripting;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    Player player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        Debug.Log("c " + collider.gameObject.tag);
        if (collider.gameObject.CompareTag("Enemy")) {
            collider.gameObject.GetComponent<Enemy>().TakeDamage(player.bulletDamage);
            Destroy(this.gameObject);
        }
        else if (collider.gameObject.CompareTag("Outer Wall")) {
            Destroy(this.gameObject);
        }
    }
}
