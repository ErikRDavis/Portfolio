using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceTower : MonoBehaviour
{
    public delegate void OnRollStarted();
    public event OnRollStarted onRollStarted;
    public delegate void OnDiceScored(int score);
    public event OnDiceScored onDiceScored;

    [SerializeField]
    private Transform diceDropPt;
    [SerializeField] 
    private float diceMinMaxTorque;
    [SerializeField]
    private float dieAnimationTime = 0.2f;

    private Dictionary<int, Die> activeDice;
    private List<int> activeDiceKeys;
    private int triggeredDice = 0;
    private Coroutine diceStopWatcher = null;

    private void Awake()
    {
        activeDice = new Dictionary<int, Die>();
        activeDiceKeys = new List<int>();
    }

    public void RollDice(Die[] dice)
    {
        Die die;
        for(int i = 0; i < dice.Length; i++)
        {
            die = dice[i];

            RollDie(die);
        }
    }

    public void RollDie(Die die)
    {
        if (activeDice.ContainsKey(die.GetInstanceID())) return;

        if(activeDice.Count == 0)
        {
            onRollStarted?.Invoke();
        }

        AddActiveDie(die);
        StartCoroutine(AnimateDieToDropPoint(die, dieAnimationTime));
    }

    private void AddActiveDie(Die die)
    {
        int key = die.GetInstanceID();

        if (activeDice.TryAdd(key, die))
        {
            activeDiceKeys.Add(key);
        }
        else if(Debug.isDebugBuild)
        {
            Debug.LogError($"Already tracking active die {die.name}");
        }
    }

    private int GetActiveDiceValue()
    {
        int value = 0;

        for(int i = 0; i < activeDiceKeys.Count; i++)
        {
            value += activeDice[activeDiceKeys[i]].GetScoringSide();
        }

        return value;
    }

    private IEnumerator AnimateDieToDropPoint(Die die, float time)
    {
        die.transform.SetParent(diceDropPt);

        die.DisablePhysics();

        float currentTime = 0;
        float progress;
        Vector3 start = die.transform.localPosition;
        Vector3 end = new Vector3(start.x, 0, 0);

        while(currentTime < time)
        {
            progress = currentTime / time;

            die.transform.localPosition = Vector3.Lerp(start, end, progress);

            currentTime += Time.deltaTime;
            yield return null;
        }

        die.transform.localPosition = end;

        die.transform.SetParent(null);

        die.EnablePhysics();

        // Create some random torque for the die as it falls
        Vector3 torque = new Vector3(Random.Range(-diceMinMaxTorque, diceMinMaxTorque), Random.Range(-diceMinMaxTorque, diceMinMaxTorque), Random.Range(-diceMinMaxTorque, diceMinMaxTorque));

        die.TumbleDie(Vector3.zero, torque);
    }

    public void OnEvent_DieLeftBaseTrigger(Die die)
    {
        triggeredDice++;

        if (diceStopWatcher == null && triggeredDice == activeDice.Count)
        {
            diceStopWatcher = StartCoroutine(WatchForDiceToStop());
        }
    }

    private IEnumerator WatchForDiceToStop()
    {
        while (ActiveDiceHaveStopped() == false)
        {
            yield return new WaitForSeconds(0.2f);
        }

        int score = GetActiveDiceValue();

        onDiceScored?.Invoke(score);

        ResetTower();
    }

    private bool ActiveDiceHaveStopped()
    {
        int stoppedCount = 0;

        for (int i = 0; i < activeDiceKeys.Count; i++)
        {
            if (activeDice[activeDiceKeys[i]].IsMoving == false)
            {
                stoppedCount++;
            }
        }

        return stoppedCount == activeDice.Count;
    }

    public void ResetTower()
    {
        StopAllCoroutines();

        triggeredDice = 0;
        diceStopWatcher = null;
        activeDice.Clear();
        activeDiceKeys.Clear();
    }
}
