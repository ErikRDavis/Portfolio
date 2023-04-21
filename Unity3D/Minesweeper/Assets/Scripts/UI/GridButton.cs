using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class GridButton : Button, IPointerEnterHandler, IPointerExitHandler
{
    public string ID => id;
    public GridSpaceType GridSpaceType { get; private set; }

    private string id;
    private TextMeshProUGUI btnText;
    private Action<string> onRightClick;
    private bool isPointerInside;

    protected override void Awake()
    {
        base.Awake();

        btnText = GetComponentInChildren<TextMeshProUGUI>();
        image = GetComponent<Image>();
    }

    public void SetButtonID(string id)
    {
        this.id = id;
    }

    public void SetType(GridSpaceType tileType)
    {
        GridSpaceType = tileType;
    }

    public void SetButtonColor(Color color)
    {
        image.color = color;
    }

    public void SetButtonText(string text)
    {
        btnText.text = text;
    }

    public void SetLeftClickAction(Action<string> onLeftClick)
    {
        onClick.AddListener(() => {
            onLeftClick(id);
        });
    }

    public void SetRightClickAction(Action<string> onRightClick)
    {
        this.onRightClick = onRightClick;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (IsActive() && IsInteractable())
            {
                DoStateTransition(SelectionState.Normal, instant: false);

                if(isPointerInside)
                {
                    onRightClick?.Invoke(id);
                }
            }
        }
        else
        {
            base.OnPointerUp(eventData);
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (IsActive() && IsInteractable())
            {
                DoStateTransition(SelectionState.Pressed, instant: false);
            }
        }
        else
        {
            base.OnPointerDown(eventData);
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        isPointerInside = true;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        isPointerInside = false;
    }
}
