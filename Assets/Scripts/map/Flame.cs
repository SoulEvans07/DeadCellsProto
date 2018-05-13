using UnityEngine;

public class Flame : MonoBehaviour
{
    public GameObject flameDot;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.tag);
        if (!other.CompareTag("Enemy") && !other.CompareTag("Player"))
            return;


        GameObject dotObject = Instantiate(flameDot, other.transform);
        DamageOverTime dot = dotObject.GetComponent<DamageOverTime>();
        dot.Apply(other.GetComponent<Headless>());
        Destroy(dotObject, dot.duration);
    }
}