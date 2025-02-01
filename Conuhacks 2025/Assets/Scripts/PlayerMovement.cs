using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 moveInput;
    public Rigidbody2D rb;

    public Player player;
    
    void Start()
    {
        
    }

    void Update()
    {
        Move();
        Shoot();
    }

    private void Move() {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();

        rb.linearVelocity = moveInput * moveSpeed;
    }

    private void Shoot() {
        if (Input.GetMouseButton(0)) {
            player.Fire();
        }
    }
}
