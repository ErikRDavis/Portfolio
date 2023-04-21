using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GridSpaceType
{
    UNEXPLORED,
    CLEARED,
    FLAG,
    MINE
}

public class MineSweeperGrid
{
    public delegate void OnHitMine(string buttonId);
    public event OnHitMine onHitMine;
    public delegate void OnGridCleared();
    public event OnGridCleared onGridCleared;

    private List<string> availableMinePositions = new List<string>();
    private Dictionary<string, GridSpaceType> gridSpaces;
    private UIController uiController;
    private Flags flags;
    private bool inputPermitted = false;
    private int mineCount;
    private int clearedCount = 0;

    public MineSweeperGrid(UIController uiController, Flags flags)
    {
        this.uiController = uiController;
        this.flags = flags;

        gridSpaces = new Dictionary<string, GridSpaceType>();

        uiController.onSpaceLeftSelected += SpaceSelected;
        uiController.onSpaceRightClicked += ToggleFlag;
    }

    private void SpaceSelected(string buttonId)
    {
        if (!inputPermitted || flags.HasFlag(buttonId)) return;

        string[] components = buttonId.Split('_');

        int row = int.Parse(components[0]);
        int col = int.Parse(components[1]);

        if (gridSpaces[buttonId] == GridSpaceType.MINE)
        {
            onHitMine?.Invoke(buttonId);

            return;
        }

        ClearGridSpace(buttonId);

        // Update the tiles surrounding the tile that got clicked
        CheckSurroundingTiles(row, col);

        // Check for all clear/win condition
        if (GridHasBeenCleared())
        {
            onGridCleared?.Invoke();
        }
    }

    private void ClearGridSpace(string spaceKey)
    {
        if (flags.HasFlag(spaceKey))
        {
            ToggleFlag(spaceKey);
        }

        uiController.DisableButton(spaceKey);
        gridSpaces[spaceKey] = GridSpaceType.CLEARED;
        uiController.DisableButton(spaceKey);
        clearedCount++;
    }

    private void ToggleFlag(string buttonId)
    {
        if (!inputPermitted) return;

        if (flags.HasFlag(buttonId))
        {
            flags.RemoveFlag(buttonId);

            uiController.SetSpaceText(buttonId, "");

#if SHOW_DEBUG
            if (gridSpaces[buttonId] == GridSpaceType.MINE)
            {
                uiController.SetSpaceText(buttonId, "M");
            }
#endif
        }
        else if (flags.CanPlantFlag())
        {
            flags.PlantFlag(buttonId, gridSpaces[buttonId]);

            uiController.SetSpaceText(buttonId, "F");

            // Check for all clear/win condition
            if (GridHasBeenCleared())
            {
                onGridCleared?.Invoke();
            }
        }

        UpdateFlagCount();
    }

    private bool GridHasBeenCleared()
    {
        return gridSpaces.Count - clearedCount == mineCount && flags.FlagCount == mineCount;
    }

    private void UpdateFlagCount()
    {
        uiController.SetFlagCount(mineCount - flags.FlagCount);
    }

    public void InitializeGrid(int gridSize, int mineCount)
    {
        this.mineCount = mineCount;

        ResetGrid();

        string key;
        for (int r = 0; r < gridSize; ++r)
        {
            for (int c = 0; c < gridSize; ++c)
            {
                key = $"{r}_{c}";

                gridSpaces.Add(key, GridSpaceType.UNEXPLORED);

                uiController.AddGridButton(key, GridSpaceType.UNEXPLORED);

                availableMinePositions.Add(key);
            }
        }

        AssignMines();
    }

    private void ResetGrid()
    {
        clearedCount = 0;
        uiController.ResetGrid();
        gridSpaces.Clear();
        availableMinePositions.Clear();
        flags.Reset();

        uiController.SetFlagCount(mineCount - flags.FlagCount);
    }

    private void AssignMines()
    {
        int index;
        string spaceKey;

        if (mineCount >= gridSpaces.Count)
        {
            Debug.LogError($"Current mine count of {mineCount} is >= to total grid size ({gridSpaces.Count}) which leads to an impossible to win game.");
        }

        try
        {
            for (int i = 0; i < Mathf.Min(mineCount, gridSpaces.Count); ++i)
            {
                index = Random.Range(0, availableMinePositions.Count);
                spaceKey = availableMinePositions[index];
                gridSpaces[spaceKey] = GridSpaceType.MINE;
                availableMinePositions.RemoveAt(index);
                uiController.SetButtonType(spaceKey, GridSpaceType.MINE);

#if SHOW_DEBUG
                uiController.SetSpaceText(spaceKey, "M");
#endif
            }
        }
        catch (System.Exception err)
        {
            Debug.LogError($"AssignMines: {err.Message + err.StackTrace}");
        }
    }

    private int CheckSurroundingTiles(int row, int col)
    {
        int newRow, newCol, mineCount = 0;
        string spaceKey;

        for (int r = -1; r < 2; ++r)
        {
            for (int c = -1; c < 2; ++c)
            {
                newRow = row + r;
                newCol = col + c;

                // Skip same tile coord
                if (newRow == row && newCol == col) continue;

                // Skip coord outside of the grid
                if (newRow < 0 || newRow >= uiController.GridConstraintCount || newCol < 0 || newCol >= uiController.GridConstraintCount) continue;

                spaceKey = $"{newRow}_{newCol}";

                // Skip tile that is already cleared
                if (gridSpaces[spaceKey] == GridSpaceType.CLEARED) continue;

                // for each neighbor at the received spot count how many mines are nearby
                if (gridSpaces[spaceKey] == GridSpaceType.MINE)
                {
                    mineCount++;
                }
                else if (mineCount == 0 && SpaceIsDiagonalTo(row, col, newRow, newCol) == false)
                {
                    ClearGridSpace(spaceKey);

                    int mines = CheckSurroundingTiles(newRow, newCol);
                    uiController.SetSpaceText(spaceKey, $"{(mines > 0 ? $"{mines}" : $"")}");
                }
            }
        }

        return mineCount;
    }

    private bool SpaceIsDiagonalTo(int startingRow, int startingColumn, int spaceRow, int spaceColumn)
    {
        if (spaceRow == startingRow || spaceColumn == startingColumn) return false;

        return true;
    }

    public void PermitInput()
    {
        inputPermitted = true;
    }

    public void ProhibitInput()
    {
        inputPermitted = false;
    }

    public bool IsSpaceMine(string spaceKey)
    {
        return gridSpaces[spaceKey] == GridSpaceType.MINE;
    }
}