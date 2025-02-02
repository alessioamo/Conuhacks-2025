using UnityEngine;
using UnityEngine.UI;

public class BetButton : MonoBehaviour
{
    public int value;
    public Sprite regular;
    public Sprite pressed;

    BlackjackGame bj;

    void Start()
    {
        bj = FindFirstObjectByType<BlackjackGame>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PressButton() {
        GameController.Instance.IncreaseBet(value);

        bj.UpdateUI();
        // if (isPressed) {
        //     GetComponent<Image>().sprite = regular;
        //     GameController.Instance.DecreaseBet(value);
        // }
        // else {
        //     GetComponent<Image>().sprite = pressed;
        //     GameController.Instance.IncreaseBet(value);
        // }
    }

    public void ResetButton() {
        GameController.Instance.ResetBet();
        bj.UpdateUI();
    }
}
