using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class Darts : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isAiming) {
            if (isMovingHorizontally) {
                MoveHorizontally();
            }
            
            else {
                MoveVertically();
            }
        }

        if (Input.GetMouseButtonDown(0) && !isPlayerReady) {
            isMovingHorizontally = false;
            isPlayerReady = true;
        }

        if (Input.GetMouseButtonUp(0) && isPlayerReady) {
            LogFinalPosition();
            isPlayerReady = false;
        }
    }

    bool isAiming = true;
    public Transform crosshair;
    public Transform horizontalStart;
    public Transform horizontalEnd;
    public Transform verticalStart;
    public Transform verticalEnd;
    public float horizontalSpeed = 5f;
    public float verticalSpeed = 5f; 

    private bool isMovingHorizontally = true;
    private bool isPlayerReady = false;
    private Vector3 finalPosition;

    private void MoveHorizontally() {
        float moveDirection = Mathf.PingPong(Time.time * horizontalSpeed, 1);
        crosshair.position = Vector3.Lerp(horizontalStart.position, horizontalEnd.position, moveDirection);
    }

    private void MoveVertically() {
        float moveDirection = Mathf.PingPong(Time.time * verticalSpeed, 1);
        crosshair.position = Vector3.Lerp(verticalStart.position, verticalEnd.position, moveDirection);
    }

    private void LogFinalPosition() {
        finalPosition = crosshair.position;
        Debug.Log("Final Position: " + finalPosition);
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
