using UnityEngine;

[CreateAssetMenu(fileName = "Block", menuName = "Scriptable Object/Block")]
public class Block : ScriptableObject
{
    public enum Type
    {
        move, control, work
    }

    public Type type;
    public string name;
    public int num;
}