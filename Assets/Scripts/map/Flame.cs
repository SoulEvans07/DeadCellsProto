using UnityEngine;

public class Flame : MonoBehaviour
{
    public GameObject flameDot;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") && PlayerController.instance.flameDotObject == null)
            return;


        GameObject dotObject = Instantiate(flameDot, other.transform);
        DamageOverTime dot = dotObject.GetComponent<DamageOverTime>();
        other.GetComponent<PlayerController>().Affect(dot);
        Destroy(dotObject, dot.duration);
    }
}