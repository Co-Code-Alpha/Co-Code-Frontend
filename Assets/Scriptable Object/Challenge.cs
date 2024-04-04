using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Challenge", fileName="Challenge")]
public class Challenge : Problem
{
    public string firstClear;
    public DateTime day;
}
