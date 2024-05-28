using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Problem", fileName="Problem")]
public class Problem : ScriptableObject
{
    public class Condition
    {
        
    }

    public class Reward
    {
        public int money;
        public string[] items;
    }
    
    public enum Category
    {
        IO, Calc, If, Loop, Array, Stack, Queue, Search
    }
    
    public int mapId;
    public int problemId;
    public string title;
    public string description;
    public Category category;
    public bool isSolved;
    public Condition condition;
    public Reward reward;
}