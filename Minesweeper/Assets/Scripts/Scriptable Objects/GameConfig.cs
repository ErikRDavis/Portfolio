using UnityEngine;

[CreateAssetMenu(fileName = "Game Config 1", menuName = "ScriptableObjects/Game Config", order = 1)]
public class GameConfig : ScriptableObject
{
    public int mineCount = 10;
    public int gridSize = 20;
}