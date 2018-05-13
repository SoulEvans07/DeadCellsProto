using UnityEngine;

public class DamageOverTime : MonoBehaviour {
    public int damage;
    public float duration;
    public float speed;
    [ShowOnly] public float timeLeft;
    [ShowOnly] public float timer;
    [ShowOnly] public Living target;

    void Start() {
        timeLeft = duration;
    }

    public int Damage() {
        timeLeft--;
        return damage;
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
        target.DotAffect(damage);
    }

    public void Remove() {
        target.dotList.Remove(this);
        Destroy(this.gameObject);
    }

    public void Apply(Living living) {
        target = living;
        target.AddDot(this);
        this.gameObject.SetActive(true);
    }
}