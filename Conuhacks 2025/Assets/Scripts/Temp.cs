using UnityEngine;
using UnityEngine.SceneManagement;

public class Temp : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame() {
        // TODO  play horse animation

        GoToSaloon();
    }

    public void GoToSaloon() {
        AudioController.instance.StopMusic();
        AudioController.instance.ChangeMusic(1);
        SceneManager.LoadScene(1);
    }
}
