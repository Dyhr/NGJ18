using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Boss : MonoBehaviour {

    public AudioClip DeathSound;
    public float ExpectedDeathTime = 45;
    public float Health = 10;

    private float spawnTime;
    public float health = 10;

    private void Start() {
        spawnTime = Time.time;
        ExpectedDeathTime += Time.time;
        health = Health;
    }

    public void Hurt() {

        health -= 1;

        if (health <= 0) Die();
    }

    private void Die() {
        GetComponentInChildren<Animator>().SetTrigger("Die");
        GetComponent<AudioSource>().PlayOneShot(DeathSound);
    }
}