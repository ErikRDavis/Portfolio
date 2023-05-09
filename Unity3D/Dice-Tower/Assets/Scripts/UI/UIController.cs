using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI diceTotal;

    public void SetDiceTotal(int total)
    {
        diceTotal.text = $"Dice Total: {total}";
    }
}
