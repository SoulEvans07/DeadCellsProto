using UnityEngine;

[CreateAssetMenu(fileName = "New Food", menuName = "Consumables/Food")]
public class Food : ScriptableObject
{
    public int healValue = 30;
    public string spriteName;
}