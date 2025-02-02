using System.Collections;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class Darts : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public float moveSpeed = 2f;
    public float moveDistance = 8.25f;
    public Transform startPosition;
    bool isMoving = true;
    bool isVerticalMovementStarted = false;
    bool isPaused = false;

    public GameObject crosshair;
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isVerticalMovementStarted) {
            isMoving = false;
            StartCoroutine(HandlePauseAndStartVerticalMovement());
        }
        
        if (Input.GetMouseButtonDown(0) && isVerticalMovementStarted) {
            isPaused = true;
            LogCrosshairColor();
        }

        if (isMoving && !isVerticalMovementStarted) {
            float newPositionX = startPosition.position.x + Mathf.PingPong(Time.time * moveSpeed, moveDistance);
            crosshair.transform.position = new Vector3(newPositionX, crosshair.transform.position.y, transform.position.z);
        }
    }

    private IEnumerator HandlePauseAndStartVerticalMovement() {
        isPaused = true;

        yield return new WaitForSeconds(1f);

        isPaused = false;
        isVerticalMovementStarted = true;
    }

    void FixedUpdate()
    {
        if (isVerticalMovementStarted && !isPaused) {
            float newPositionY = startPosition.position.y + Mathf.PingPong(Time.time * moveSpeed, 8f) - 8f;
            crosshair.transform.position = new Vector3(crosshair.transform.position.x, newPositionY, crosshair.transform.position.z);
        }
    }

    void LogCrosshairColor() {
    Vector3 screenPos = Camera.main.WorldToScreenPoint(crosshair.transform.position);
    
    // Normalize screen position to get UV coordinates
    float normalizedX = screenPos.x / Screen.width;
    float normalizedY = screenPos.y / Screen.height;

    // Capture the screen as a texture (use a method to capture the screen texture)
    Texture2D screenTexture = ScreenCapture.CaptureScreenshotAsTexture();
    Color color = screenTexture.GetPixel((int)screenPos.x, (int)screenPos.y);

    // Convert color to hex string
    string hexColor = ColorUtility.ToHtmlStringRGB(color);
    Debug.Log("Hex color at crosshair position: #" + hexColor);
}

    public TextMeshProUGUI playerScoreText;
    public TextMeshProUGUI enemyScoreText;
    int playerScore = 0;
    int enemyScore = 0;

    int turnCounter = 0;

    void StartTurn() {
        if (turnCounter == 0) {
            PlayerTurn();
        }
        else {
            EnemyTurn();
        }
    }

    int numberOfPlayerDarts = 3;
    int numberOfEnemyDarts = 3;
    void PlayerTurn() {
        for (int i = 0; i < numberOfPlayerDarts; i++) {

            numberOfPlayerDarts--;
        }

        numberOfPlayerDarts = 3;

        turnCounter = 1;
        StartTurn();
    }

    void EnemyTurn() {


        turnCounter = 0;
    }
}
