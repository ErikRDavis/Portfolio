using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;

public enum GameState
{
    INIT,
    WAITING,
    PLAYING,
    ENDED
}

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private UIController uiController;
    [SerializeField]
    private GameConfig gameConfig;

    private MineSweeperGrid grid;

    private GameState gameState = GameState.INIT;

    // Start is called before the first frame update
    void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        Flags flags = new Flags(gameConfig.mineCount);

        grid = new MineSweeperGrid(uiController, flags);

        grid.onHitMine += OnHitMine;
        grid.onGridCleared += OnGridCleared;

        uiController.SetGridSize(gameConfig.gridSize);
        uiController.AdjustGridCellsToFit();

        uiController.onNewGame += NewGame;

        NewGame();
    }

    private void OnHitMine(string buttonId)
    {
        GameOver(buttonId);
    }

    private void GameOver(string mineId)
    {
        gameState = GameState.ENDED;
        grid.ProhibitInput();
        uiController.StopTimer();
        uiController.SetWinLoseLabel("Game Over! :(");
        uiController.ShowMinesForLoss(mineId);
    }

    private void OnGridCleared()
    {
        PlayerWin();
    }

    private void PlayerWin()
    {
        gameState = GameState.ENDED;
        grid.ProhibitInput();
        uiController.StopTimer();
        uiController.SetWinLoseLabel("Player Wins!");
    }

    private void NewGame()
    {
        if (gameState != GameState.WAITING)
        {
            RegisterInputHandlers();
        }

        gameState = GameState.WAITING;

        grid.InitializeGrid(gameConfig.gridSize, gameConfig.mineCount);

        grid.PermitInput();
    }

    private void RegisterInputHandlers()
    {
        uiController.onSpaceLeftSelected += LeftClickStartedGame;
        uiController.onSpaceRightClicked += RightClickStartedGame;
    }

    private void LeftClickStartedGame(string spaceKey)
    {
        UnregisterInputHandlers();

        if (grid.IsSpaceMine(spaceKey))
        {
            return;
        }

        gameState = GameState.PLAYING;

        uiController.StartTimer();
    }

    private void UnregisterInputHandlers()
    {
        uiController.onSpaceLeftSelected -= LeftClickStartedGame;
        uiController.onSpaceRightClicked -= RightClickStartedGame;
    }

    private void RightClickStartedGame(string str)
    {
        gameState = GameState.PLAYING;

        UnregisterInputHandlers();

        uiController.StartTimer();
    }
}
