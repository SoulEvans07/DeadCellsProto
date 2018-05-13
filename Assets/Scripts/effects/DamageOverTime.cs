using UnityEngine;

public class DamageOverTime : MonoBehaviour {
    public int dps;
    public float duration;
    public float speed;
    [ShowOnly] public float timeLeft;
    [ShowOnly] public float timer;
    [ShowOnly] public Living target;

    void Start() {
        timeLeft = duration;
    }

    private void Update() {
        timer += Time.deltaTime;
        while (timer > speed) {
            Affect();
            timer -= speed;
            timeLeft -= speed;
            if(timeLeft < 0)
                Remove();
        }
    }

    public void Affect() {
        target.DotAffect((int) (dps * speed));
    }

    public void Remove() {
        target.dotList.Remove(this);
        Destroy(this.gameObject);
    }

    public void Apply(Living living) {
        if (!living.AddDot(this)) {
            Destroy(this.gameObject);
        }
        target = living;
        this.gameObject.SetActive(true);
    }
}