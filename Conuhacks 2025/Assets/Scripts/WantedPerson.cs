using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class FaceSpawner : MonoBehaviour
{
    public GameObject[] facePrefabs;
    public int numFaces = 40;
    public float spacing = 2f;
    public Vector2 areaSize = new Vector2(10f, 10f);

    private GameObject uniqueFaceInstance;
    private List<GameObject> availableFaces;

    public Transform spawnParent;
    public Transform initialFaceSpawn;

    public GameObject initialWantedPoster;

    public Slider timerSlider1;
    public Slider timerSlider2;

    private float timer = 10f;
    private bool timerRunning = false;

    void Start()
    {
        availableFaces = new List<GameObject>(facePrefabs);
        StartCoroutine(SpawnFaces());
    }

    IEnumerator SpawnFaces()
    {
        float centerX = initialFaceSpawn.position.x;
        float centerY = initialFaceSpawn.position.y;

        int rows = 5;
        int cols = 8;

        float startX = centerX - (cols * spacing) / 2f;
        float startY = centerY + (rows * spacing) / 2f;

        List<Vector2> gridPositions = new List<Vector2>();
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                gridPositions.Add(new Vector2(startX + col * spacing, startY - row * spacing));
            }
        }

        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector2 uniqueFacePosition = gridPositions[randomIndex];

        int uniqueFaceIndex = Random.Range(0, availableFaces.Count);
        GameObject uniqueFacePrefab = availableFaces[uniqueFaceIndex];
        availableFaces.RemoveAt(uniqueFaceIndex);

        StartCoroutine(ShowWantedPerson(uniqueFacePrefab));

        yield return new WaitForSeconds(5);

        uniqueFaceInstance = Instantiate(uniqueFacePrefab, uniqueFacePosition, Quaternion.identity);
        uniqueFaceInstance.gameObject.transform.SetParent(spawnParent);
        uniqueFaceInstance.GetComponent<Collider2D>().isTrigger = true;

        gridPositions.RemoveAt(randomIndex);

        for (int i = 0; i < numFaces - 1; i++)
        {
            int randomGridIndex = Random.Range(0, gridPositions.Count);
            Vector2 spawnPosition = gridPositions[randomGridIndex];

            GameObject chosenFace = availableFaces[Random.Range(0, availableFaces.Count)];
            GameObject tempFaceHolder = Instantiate(chosenFace, spawnPosition, Quaternion.identity);
            tempFaceHolder.gameObject.transform.SetParent(spawnParent);

            gridPositions.RemoveAt(randomGridIndex);
        }

        timerRunning = true;
    }

    IEnumerator ShowWantedPerson(GameObject prefab)
    {
        SpriteRenderer wantedPosterRenderer = initialWantedPoster.transform.GetChild(0).GetComponent<SpriteRenderer>();
        wantedPosterRenderer.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
        initialWantedPoster.gameObject.SetActive(true);

        yield return new WaitForSeconds(5);
        
        initialWantedPoster.gameObject.SetActive(false);
    }

    bool isGameOver = false;
    void Update()
    {
        if (timerRunning)
        {
            timer -= Time.deltaTime;

            if (!isGameOver) {
                if (timer <= 0f)
                {
                    Debug.Log("Time's up! The timer has run out.");
                    timer = 0f; 
                    timerRunning = false;

                    // TODO - End and return to saloon
                    LoseGame();
                }

                timerSlider1.value = timer / 10f;
                timerSlider2.value = timer / 10f;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mousePosition);

            if (hit != null && hit.gameObject == uniqueFaceInstance)
            {
                WinGame();
            }
            else if (hit != null && hit.gameObject != uniqueFaceInstance) {
                LoseGame();
            }
        }
    }

    IEnumerator ReturnToSaloon() {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(1);
    }

    void WinGame()
    {
        Debug.Log("You win the minigame!");
        GameController.Instance.playerBalance += 10;
        isGameOver = true;
        uniqueFaceInstance.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        uniqueFaceInstance.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
        // StartCoroutine(ReturnToSaloon());

        GameController.Instance.LoadBossFight(1);
    }

    void LoseGame() {
        // TODO - show unique location
        GameController.Instance.playerBalance -= 5;
        isGameOver = true;
        uniqueFaceInstance.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        uniqueFaceInstance.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
        StartCoroutine(ReturnToSaloon());
    }
}
