using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GridSpace : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI textMeshProUGUI; 
    public string playerSide;

    private GameController gameController;

    public void SetSpace()
    {
        if (gameController.playerMove == true)
        {
            textMeshProUGUI.text = gameController.GetPlayerSide();
            button.interactable = false;
            gameController.EndTurn();
        }
    }

    public void SetGameControllerReference(GameController controller)
    {
        gameController = controller;
    }
}
