using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] musicClips;

    public static AudioController instance;
    private int currentTrackIndex = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (audioSource != null && musicClips.Length > 0)
        {
            audioSource.clip = musicClips[0];
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    // void Update() {
    //     int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    //     if (currentTrackIndex != 0 && currentSceneIndex == 0) {
    //         ChangeMusic(0);
    //     }
    //     else if (currentTrackIndex != 1 && currentSceneIndex != 5) {
    //         ChangeMusic(1);
    //     }
    //     else if (currentTrackIndex != 2 && currentSceneIndex == 5) {
    //         ChangeMusic(2);
    //     }
    // }

    public void StopMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void ChangeMusic(int trackIndex)
    {
        if (audioSource != null && trackIndex >= 0 && trackIndex < musicClips.Length)
        {
            audioSource.clip = musicClips[trackIndex];
            audioSource.Play();
        }
    }

    public void NextTrack()
    {
        currentTrackIndex = (currentTrackIndex + 1) % musicClips.Length;
        ChangeMusic(currentTrackIndex);
    }

    public void PreviousTrack()
    {
        currentTrackIndex = (currentTrackIndex - 1 + musicClips.Length) % musicClips.Length;
        ChangeMusic(currentTrackIndex);
    }

    public void QuitGame() {
        Application.Quit();
    }
}