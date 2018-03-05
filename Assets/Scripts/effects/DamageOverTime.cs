using UnityEngine;

public class DamageOverTime : MonoBehaviour
{
    public float damage;
    public int duration;
    public int timeLeft;

    public DamageOverTime()
    {
        timeLeft = duration;
    }

    public float Damage()
    {
        timeLeft--;
        return damage;
    }
}