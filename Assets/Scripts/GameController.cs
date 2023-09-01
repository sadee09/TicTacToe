using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[Serializable]
public class Player
{
    public Image panel;
    public TextMeshProUGUI text;
    public Button button;
}
[Serializable]
public class PlayerColor
{
    public Color panelColor;
}
public class GameController : MonoBehaviour
{
    public TextMeshProUGUI[] buttonList;
    
    private string playerSide;
    private string computerSide;
    public bool isMultiplayer;
    
    private int value;
    public bool playerMove;
    public float delay;
        
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;

    private int moveCount;

    public GameObject restartButton;

    public Player playerX;
    public Player playerO;
    public PlayerColor activePlayerColor;
    public PlayerColor inactivePlayerColor;
    public GameObject startInfo;

    public GameObject multiPlayer;
    public GameObject playWithComputer;
        
    public event Action<string> GameOverEvent;
    
    private void Awake()
    {
        gameOverPanel.SetActive(false);
        SetGameControllerOnButtons();
        moveCount = 0;
        restartButton.SetActive(false);
        playerMove = true;
        
        isMultiplayer = false;
        multiPlayer.SetActive(true);
        playWithComputer.SetActive(true);
        
        startInfo.SetActive(false);
        playerX.panel.gameObject.SetActive(false);
        playerO.panel.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (playerMove == false)
        {
            delay += delay * Time.deltaTime;
            if (delay >= 100)
            {
                value = UnityEngine.Random.Range(0, 8);
                if (buttonList[value].GetComponentInParent<Button>().interactable)
                {
                    buttonList[value].text = GetComputerSide();
                    buttonList[value].GetComponentInParent<Button>().interactable = false;
                    EndTurn();
                }
            }
        }
    }

    void SetGameControllerOnButtons()
    {
        for(int i = 0; i< buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
        }
    }

    public void StartMultiplayerGame()
    {
        isMultiplayer = true;
        MakeActive();
    }
    
    public void StartPlayWithComputerGame()
    {
        isMultiplayer = false;
        MakeActive();
    }
    void MakeActive()
    {
        startInfo.SetActive(true);
        SetPlayerButton(true);
        playerX.panel.gameObject.SetActive(true);
        playerO.panel.gameObject.SetActive(true);
        multiPlayer.SetActive(false);
        playWithComputer.SetActive(false);
    }
    public void SetStartingSide(string startingSide)
    {
        
        multiPlayer.SetActive(false);
        playWithComputer.SetActive(false);
        
        playerSide = startingSide;
        if (playerSide == "X")
        {
            computerSide = "O";
            SetPlayerColor(playerX,playerO);
        }
        else
        {
            computerSide = "X";
            SetPlayerColor(playerO,playerX);
        }
        StartGame();
    }
    

    void StartGame()
    {
        SetButtonInteractable(true);
        SetPlayerButton(false);
        startInfo.SetActive(false);
    }

    public string GetPlayerSide()
    {
        return playerSide;
    }

    public string GetComputerSide()
    {
        return computerSide;
    }

    public void EndTurn()
    {
        moveCount++;
        if (CheckWinCondition())
        {
            GameOver();
        }

        else if (moveCount >= 9)
        {
            SetGameOverText("It's a draw!");   
            restartButton.SetActive(true);
            SetPlayerColorsInactive();
        }
        else
        {
            if (isMultiplayer)
            {
                ChangeSideMulti();
            }
            else
            {
                ChangeSide();
            }
            delay = 10;
        }
    }
    bool CheckWinCondition()
    {
        string[] lines = {
            buttonList[0].text + buttonList[1].text + buttonList[2].text,
            buttonList[3].text + buttonList[4].text + buttonList[5].text,
            buttonList[6].text + buttonList[7].text + buttonList[8].text,
            buttonList[0].text + buttonList[3].text + buttonList[6].text,
            buttonList[1].text + buttonList[4].text + buttonList[7].text,
            buttonList[2].text + buttonList[5].text + buttonList[8].text,
            buttonList[0].text + buttonList[4].text + buttonList[8].text,
            buttonList[2].text + buttonList[4].text + buttonList[6].text
        };

        foreach (string line in lines)
        {
            if (line == playerSide + playerSide + playerSide)
            {
                SetGameOverText(playerSide + " Wins!");
                return true;
            }
            else if (line == computerSide + computerSide + computerSide)
            {
                SetGameOverText(computerSide + " Wins!");
                return true;
            }
        }
        return false;
    }
    void SetPlayerColor(Player newPlayer, Player oldPlayer)
    {
        newPlayer.panel.color = activePlayerColor.panelColor;
        oldPlayer.panel.color = inactivePlayerColor.panelColor;
    }
    void GameOver()
    {
        SetButtonInteractable(false);
        restartButton.SetActive(true);
        GameOverEvent?.Invoke(playerSide);
    }

    delegate void SetPlayerColorsDelegate(Player activePlayer, Player inactivePlayer);

    void ChangeSideMulti()
    {
        playerSide = (playerSide == "X") ? "O" : "X";
        SetPlayerColors(SetPlayerColor);
    }
    void ChangeSide()
    {
        playerMove = playerMove ? false : true;
        SetPlayerColors(SetPlayerColor);
    }
    void SetPlayerColors(SetPlayerColorsDelegate setPlayerColors)
    {
        if (playerSide == "X" || playerMove)
        {
            setPlayerColors(playerX, playerO);
        }
        else
        {
            setPlayerColors(playerO,playerX);
        }
    }
    

    void SetGameOverText(string value)
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = value;
    }

    public void RestartGame()
    {
        moveCount = 0;
        gameOverPanel.SetActive(false);
        restartButton.SetActive(false);
        SetPlayerColorsInactive();
        playerMove = true;
        delay = 10;
        multiPlayer.SetActive(true);
        playWithComputer.SetActive(true);
        
        playerX.panel.gameObject.SetActive(false);
        playerO.panel.gameObject.SetActive(false);
        
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].text = "";
        }
    }

    void SetButtonInteractable(bool toogle)
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = toogle;
        }
    }

    void SetPlayerButton(bool toogle)
    {
        playerX.button.interactable = toogle;
        playerO.button.interactable = toogle;
    }

    void SetPlayerColorsInactive()
    {
        playerX.panel.color = inactivePlayerColor.panelColor;
        playerO.panel.color = inactivePlayerColor.panelColor;
    }
    
}
