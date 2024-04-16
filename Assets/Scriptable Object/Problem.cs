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
        public int[] items;
    }
    
    public enum Category
    {
        
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