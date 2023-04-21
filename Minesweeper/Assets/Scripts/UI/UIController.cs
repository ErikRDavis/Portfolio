using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;

//[RequireComponent(typeof(UIDocument))]
public class UIController : UIBehaviour
{
    public delegate void OnSpaceLeftSelected(string buttonId);
    public event OnSpaceLeftSelected onSpaceLeftSelected;
    public delegate void OnSpaceRightClicked(string buttonId);
    public event OnSpaceRightClicked onSpaceRightClicked;
    public delegate void OnNewGame();
    public event OnNewGame onNewGame;

    [SerializeField]
    private GridButton prefab;
    [SerializeField]
    public GridLayoutGroup gridLayout;
    [SerializeField]
    public TextMeshProUGUI timerLabel;
    [SerializeField]
    public TextMeshProUGUI flagCount;
    [SerializeField]
    private TextMeshProUGUI winLoseLabel;
    [SerializeField]
    private Button newGameBtn;
    [SerializeField]
    private GameObject escapeMenu;
    [SerializeField]
    private Button escapeButton;

    private int currentTime = 0;
    private Coroutine timer = null;
    private Dictionary<string, GridButton> buttons = new Dictionary<string, GridButton>();
    private int gridSize;

    public int GridConstraintCount => gridLayout.constraintCount;

    protected override void Awake()
    {
        newGameBtn.onClick.AddListener(() => { onNewGame?.Invoke(); });
        escapeButton.onClick.AddListener(ExitGame);
    }

    private void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    protected override void Start()
    {
        UpdateTimerLabel();

        SetWinLoseLabel("");
        prefab.gameObject.SetActive(false);
        escapeMenu.SetActive(false);
    }

    private void UpdateTimerLabel()
    {
        timerLabel.text = currentTime.ToString("000");
    }

    public void SetWinLoseLabel(string text)
    {
        winLoseLabel.text = text;
    }

    public void SetGridSize(int count)
    {
        gridSize = count;
        gridLayout.constraintCount = gridSize;
    }

    protected override void OnRectTransformDimensionsChange()
    {
        base.OnRectTransformDimensionsChange();

        AdjustGridCellsToFit();
    }

    public void AdjustGridCellsToFit()
    {
        if(gridSize <= 0)
        {
            return;
        }

        RectTransform gridRectTransform = gridLayout.transform as RectTransform;
        float totalWidth = gridRectTransform.rect.width;

        float cellSize = totalWidth / gridSize;

        Vector2 temp = gridLayout.cellSize;
        temp.x = temp.y = cellSize;
        gridLayout.cellSize = temp;
    }

    public void StartTimer()
    {
        timer = StartCoroutine(Timer());
    }

    public void StopTimer()
    {
        if(timer != null)
        {
            StopCoroutine(timer);

            timer = null;
        }
    }

    public void AddGridButton(string key, GridSpaceType gridSpaceType)
    {
        GridButton btn = Instantiate(prefab, gridLayout.transform);

        btn.SetType(gridSpaceType);
        btn.gameObject.SetActive(true);
        btn.SetButtonID(key);
        btn.SetButtonText("");
        btn.SetLeftClickAction(SpaceLeftSelected);
        btn.SetRightClickAction(SpaceRightClicked);

        buttons.Add(key, btn);
    }

    public void SetButtonType(string spaceKey, GridSpaceType gridSpaceType)
    {
        buttons[spaceKey].SetType(gridSpaceType);
    }

    public void SetSpaceText(string buttonId, string text)
    {
        if(buttons.ContainsKey(buttonId))
        {
            buttons[buttonId].SetButtonText(text);
        }
    }

    public void DisableButton(string buttonId)
    {
        if (buttons.ContainsKey(buttonId))
        {
            buttons[buttonId].interactable = false;
        }
    }

    public void SetFlagCount(int count)
    {
        flagCount.text = $"{count}";
    }

    private void SpaceLeftSelected(string buttonId)
    {
        onSpaceLeftSelected?.Invoke(buttonId);
    }

    private void SpaceRightClicked(string buttonId)
    {
        onSpaceRightClicked?.Invoke(buttonId);
    }

    private IEnumerator Timer()
    {
        currentTime = 0;

        while(currentTime < 999)
        {
            yield return new WaitForSeconds(1);

            currentTime++;

            UpdateTimerLabel();
        }

        timer = null;
    }

    public void ShowMinesForLoss(string triggeredMineId)
    {
        foreach(KeyValuePair<string, GridButton> pair in buttons)
        {
            if(pair.Value.GridSpaceType == GridSpaceType.MINE)
            {
                SetSpaceText(pair.Value.ID, "M");
            }

            if(string.Equals(pair.Value.ID, triggeredMineId))
            {
                buttons[pair.Key].SetButtonColor(Color.red);
            }
        }
    }

    public void ResetGrid()
    {
        StopTimer();

        currentTime = 0;

        UpdateTimerLabel();

        RemovePreviousButtons();

        buttons.Clear();

        SetWinLoseLabel("");
    }

    private void RemovePreviousButtons()
    {
        foreach (KeyValuePair<string, GridButton> pair in buttons)
        {
            Destroy(pair.Value.gameObject);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            escapeMenu.SetActive(!escapeMenu.activeInHierarchy);
        }
    }
}
