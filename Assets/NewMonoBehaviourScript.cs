using UnityEngine;
using UnityEngine.InputSystem;

enum GameState { Playing, GameOver, Restart }

public class NewMonoBehaviourScript : MonoBehaviour
{
    private GameState currentState;
    int Min = 1;
    int Max = 1000;
    string Guess = "";
    int targetNum;
    Keyboard keyboard;

    int attempts;
    int maxAttempts = 10;

    void Start()
    {
        keyboard = Keyboard.current;
        StartGame();
    }

    private void StartGame()
    {
        targetNum = Random.Range(Min, Max + 1);
        //Debug.Log("Target: " + targetNum);

        attempts = 0;
        Guess = "";
        currentState = GameState.Playing;

        Debug.Log("<size=15><color=red> Welcome </color></size>");
        Debug.LogFormat("Min is {0} \n\t Max is {1}", Min, Max);
        Debug.Log($"Max attempts: {maxAttempts}. Enter your guess:");
    }

    void Update()
    {
        keyboard = Keyboard.current;
        if (keyboard == null) return;

        switch (currentState)
        {
            case GameState.Playing:
                handleUserInput();
                break;

            case GameState.GameOver:
                if (keyboard.enterKey.wasPressedThisFrame)
                {
                    currentState = GameState.Restart;
                }
                break;

            case GameState.Restart:
                StartGame();
                break;
        }
    }

    void handleUserInput()
    {
        if (keyboard.anyKey.wasPressedThisFrame)
        {
            string input = keyboard.allKeys[0].device.description.deviceClass;
            Keyboard.current.onTextInput += (char ch) => {
                if (char.IsDigit(ch) && currentState == GameState.Playing)
                {
                    Guess += ch;
                }
            };
        }

        if (keyboard.enterKey.wasPressedThisFrame || keyboard.numpadEnterKey.wasPressedThisFrame)
        {
            if (int.TryParse(Guess, out int PlayerGuess))
            {
                // Range check
                if (PlayerGuess < Min || PlayerGuess > Max)
                {
                    Debug.Log("Invalid input! Try again.");
                }
                else
                {
                    attempts++;
                    Debug.LogFormat("Your guess: <b>{0}</b>", Guess);

                    if (PlayerGuess == targetNum)
                    {
                        Debug.Log("<color=green><b>You Won!</b></color>");
                        currentState = GameState.GameOver;
                    }
                    else if (attempts >= maxAttempts)
                    {
                        Debug.Log("Game Over! Number was " + targetNum);
                        currentState = GameState.GameOver;
                    }
                    else if (PlayerGuess > targetNum)
                    {
                        Debug.Log($"<color=red>Too High!</color> You have {maxAttempts - attempts} attempts left."); 
                    }
                    else
                    {
                        Debug.Log($"<color=orange>Too Low!</color> You have {maxAttempts - attempts} attempts left."); 
                    }
                }
            }
            Guess = "";
        }
    }
}