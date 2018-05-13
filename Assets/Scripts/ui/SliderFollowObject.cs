using UnityEngine;

public class SliderFollowObject : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0,0,0);

    public void FixedUpdate()
    {
        UpdatePos();
    }

    public void UpdatePos()
    {
        if (target == null)
            return;

        GetComponent<RectTransform>().position = target.position + offset;
    }

    public void Flip() {
        Vector3 old = offset;
        old.x *= -1;
        offset = old;
        Debug.Log("fliped: " + offset);
    }
}