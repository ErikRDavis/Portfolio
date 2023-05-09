using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public enum DemoState
{
    Waiting,
    Rolling,
    Scored
}

public class DemoManager : MonoBehaviour
{
    public DemoState State { get; private set; } = DemoState.Waiting;

    [SerializeField]
    private DiceTower tower;
    [SerializeField]
    private Die[] dice;
    [SerializeField]
    private UIController uiController;

    private List<Vector3> diceStartPositions;

    private void Awake()
    {
        diceStartPositions = new List<Vector3>();

        tower.onRollStarted += Tower_onRollStarted;
        tower.onDiceScored += Tower_onDiceScored;
    }
        private void Tower_onRollStarted()
    {
        uiController.SetDiceTotal(0);
    }

    private void Tower_onDiceScored(int score)
    {
        uiController.SetDiceTotal(score);
        State = DemoState.Scored;
    }

    private void Start()
    {
        CaptureDiceStartPositions();

        uiController.SetDiceTotal(0);
    }

    private void CaptureDiceStartPositions()
    {
        for(int i = 0; i< dice.Length; i++) 
        {
            diceStartPositions.Add(dice[i].transform.position);
        }
    }

    public void OnInput()
    {
        switch (State)
        {
            case DemoState.Waiting:
                RollAllDice();
                break;
            case DemoState.Rolling:
                break;
            case DemoState.Scored:
                ResetAll();
                break;
        }
    }

    public void ResetAll()
    {
        ResetDemo();
    }

    public void RollAllDice()
    {
        tower.RollDice(dice);
        State = DemoState.Rolling;
    }

    private void ResetDemo()
    {
        tower.ResetTower();

        for (int i = 0; i < dice.Length; i++)
        {
            dice[i].transform.position = diceStartPositions[i];
            dice[i].ResetDie();
        }

        State = DemoState.Waiting;
    }
}
