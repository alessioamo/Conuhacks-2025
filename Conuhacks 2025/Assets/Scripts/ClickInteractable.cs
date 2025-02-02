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
            GameController.Instance.DisplayGameInfo(0);
            // SceneManager.LoadScene(2);
        }
        else if (tag == "Wanted") {
            GameController.Instance.DisplayGameInfo(1);
            // SceneManager.LoadScene(3);
        }
        else if (tag == "Blackjack") {
            GameController.Instance.DisplayGameInfo(2);
            // SceneManager.LoadScene(4);
        }
        else if (tag == "Boss") {
            GameController.Instance.DisplayGameInfo(3);
            // SceneManager.LoadScene(5);
        }
        else if (tag == "Bar") {
            GameController.Instance.DisplayGameInfo(4);
            // SceneManager.LoadScene(6);
        }
        else if (tag == "Horse") {
            AudioController.instance.StopMusic();
            AudioController.instance.ChangeMusic(0);
            GameController.Instance.balanceText.enabled = false;
            SceneManager.LoadScene(7);

            GameController.Instance.HideHorses();
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
