using System.Collections;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
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
    bool isPlayerTurnComplete = false;

    public GameObject crosshair;
    void Update()
    {
        if (isPlayerTurnComplete) {
            EnemyTurn();
        }
        if (Input.GetMouseButtonDown(0) && !isVerticalMovementStarted) {
            isMoving = false;
            StartCoroutine(HandlePauseAndStartVerticalMovement());
        }
        
        if (Input.GetMouseButtonDown(0) && isVerticalMovementStarted) {
            isPaused = true;
            
            Vector2 positionToCheck = crosshair.transform.position;

            Collider2D[] hitColliders = Physics2D.OverlapPointAll(positionToCheck);

            AddScore(hitColliders, positionToCheck);

            Debug.Log("Crosshair paused at position: " + crosshair.transform.position);

            isPlayerTurnComplete = true;
        }

        if (isMoving && !isVerticalMovementStarted) {
            float newPositionX = startPosition.position.x + Mathf.PingPong(Time.time * moveSpeed, moveDistance);
            crosshair.transform.position = new Vector3(newPositionX, crosshair.transform.position.y, transform.position.z);
        }
    }

    private void AddScore(Collider2D[] colliders, Vector3 position) {
        bool isDoubleOutter = false;
        bool isDoubleInner = false;
        bool isTripleOutter = false;
        bool isTripleInner = false;
        int value = 0;

        if (colliders.Length > 0) {
            Debug.Log("Found " + colliders.Length + " colliders at position: " + position);

            foreach (Collider2D collider in colliders) {
                if (collider.name == "DoubleOuter") {
                    isDoubleOutter = true;
                }
                if (collider.name == "DoubleInner") {
                    isDoubleInner = true;
                }
                if (collider.name == "TripleOuter") {
                    isTripleOutter = true;
                }
                if (collider.name == "TripleInner") {
                    isTripleInner = true;
                }
                else {
                    value = int.Parse(collider.name);
                }
                Debug.Log("Collider found: " + collider.name);
            }

            if (isDoubleOutter && !isDoubleInner) {
                value *= 2;
            }
            else if (isTripleOutter && !isTripleInner) {
                value *= 3;
            }

            playerScore += value;

            UpdateUI();
        } else {
            Debug.Log("No colliders at position: " + position);

            UpdateUI();
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

    public TextMeshProUGUI playerScoreText;
    public TextMeshProUGUI enemyScoreText;
    int playerScore = 0;
    int enemyScore = 0;

    void PlayerTurn() {
        crosshair.transform.position = startPosition.position;
        isMoving = true;
        isVerticalMovementStarted = false;
        isPaused = false;
        isPlayerTurnComplete= false;
    }

    bool isEnemyTurning = false;
    [System.Serializable]
    public struct TransformWithInt
    {
        public Transform transform;
        public int value;
    }
    public TransformWithInt[] throwPositions = new TransformWithInt[20];
    void EnemyTurn() {
        if (!isEnemyTurning) {
            isEnemyTurning = true;

            int randomIndex = Random.Range(0, throwPositions.Length);
            TransformWithInt randomEntry = throwPositions[randomIndex];

            Transform randomTransform = randomEntry.transform;
            int correspondingValue = randomEntry.value;

            enemyScore += correspondingValue;

            UpdateUI();

            SpawnDart("red", randomTransform);
        }
    }

    IEnumerator DartDespawn(GameObject dart) {
        yield return new WaitForSeconds(2f);

        Destroy(dart);

        isEnemyTurning = false;
        PlayerTurn();
    }

    public GameObject blueDart;
    public GameObject redDart;
    void SpawnDart(string color, Transform position) {
        GameObject dart;

        if (color == "blue") {
            dart = Instantiate(blueDart, position.position, Quaternion.identity);
        }
        else {
            dart = Instantiate(redDart, position.position, Quaternion.identity);
        }

        StartCoroutine(DartDespawn(dart));
    }

    public void UpdateUI() {
        if (playerScoreText != null)
        {
            playerScoreText.text = $"Player: ${playerScore}";
        }

        if (enemyScoreText != null)
        {
            enemyScoreText.text = $"Enemy: {enemyScore}";
        }
    }
}
