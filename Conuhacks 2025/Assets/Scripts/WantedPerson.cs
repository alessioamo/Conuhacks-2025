using UnityEngine;

public class FaceSpawner : MonoBehaviour
{
    public GameObject[] facePrefabs;  // Array of face prefabs to choose from
    public int numFaces = 30;         // Number of faces to spawn
    public float spacing = 2f;        // Distance between faces
    public Vector2 areaSize = new Vector2(10f, 10f);  // Area size for spawning
    public GameObject uniqueFacePrefab;  // The unique face prefab (set this in the Inspector)

    private GameObject uniqueFaceInstance;  // Instance of the unique face

    void Start()
    {
        SpawnFaces();
    }

    void SpawnFaces()
    {
        // Calculate the center of the area
        float centerX = 0f;
        float centerY = 0f;

        // Determine the number of rows and columns based on numFaces
        int rows = Mathf.CeilToInt(Mathf.Sqrt(numFaces)); // Number of rows (rounded up)
        int cols = Mathf.CeilToInt((float)numFaces / rows); // Number of columns

        // Calculate the starting position for the first face (top-left corner)
        float startX = centerX - (cols * spacing) / 2f;
        float startY = centerY + (rows * spacing) / 2f;

        // Randomly select an index for the unique face (this face will only appear once)
        int uniqueFaceIndex = Random.Range(0, numFaces);

        // Loop through the faces and place them evenly spaced
        for (int i = 0; i < numFaces; i++)
        {
            int row = i / cols;  // Row index
            int col = i % cols;  // Column index

            // Calculate the position of each face
            float xPos = startX + col * spacing;
            float yPos = startY - row * spacing;

            if (i == uniqueFaceIndex)  // Check if this is the position for the unique face
            {
                // Instantiate the unique face prefab at this position
                uniqueFaceInstance = Instantiate(uniqueFacePrefab, new Vector3(xPos, yPos, 0f), Quaternion.identity);
                uniqueFaceInstance.GetComponent<Collider2D>().isTrigger = true;  // Make sure it's interactable
            }
            else
            {
                // Instantiate a random face prefab for all other positions
                int randomIndex = Random.Range(0, facePrefabs.Length);
                GameObject chosenFace = facePrefabs[randomIndex];
                Instantiate(chosenFace, new Vector3(xPos, yPos, 0f), Quaternion.identity);
            }
        }
    }

    void Update()
    {
        // Check if the player clicks on the unique face
        if (Input.GetMouseButtonDown(0)) // Left mouse button click
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mousePosition);

            if (hit != null && hit.gameObject == uniqueFaceInstance)
            {
                // Player clicked on the unique face
                WinGame();
            }
        }
    }

    void WinGame()
    {
        Debug.Log("You win the minigame!");
        // Add any additional winning logic here (e.g., display message, play animation, etc.)
    }
}
