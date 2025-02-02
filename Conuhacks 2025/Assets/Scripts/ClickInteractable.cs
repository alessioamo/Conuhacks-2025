using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickInteractable : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public Color hoverColor = Color.yellow;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject) {
                OnClick(hit.collider.gameObject.tag);
            }
        }
    }

    public void OnClick(string tag) {
        if (tag == "Darts") {
            SceneManager.LoadScene(2);
        }
        else if (tag == "Wanted") {
            SceneManager.LoadScene(3);
        }
        else if (tag == "Blackjack") {
            SceneManager.LoadScene(4);
        }
        else if (tag == "Boss") {
            SceneManager.LoadScene(5);
        }
        else if (tag == "Bar") {
            SceneManager.LoadScene(6);
        }
    }

    void OnMouseOver() {
        if (spriteRenderer != null) {
            spriteRenderer.enabled = true;
        }
    }

    void OnMouseExit() {
        if (spriteRenderer != null) {
            spriteRenderer.enabled = false;
        }
    }
}
