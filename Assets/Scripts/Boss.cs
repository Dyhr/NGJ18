using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Boss : MonoBehaviour {

    public AudioClip DeathSound;
    public float ExpectedDeathTime = 45;
    public float Health = 10;

    public bool Dead;

    public List<GameObject> Minions = new List<GameObject>();

    private float spawnTime;
    private float health = 10;

    private void Start() {
        spawnTime = Time.time;
        ExpectedDeathTime += Time.time;
        health = Health;
    }

    public void Hurt() {
        if (health <= 0) return;
        
        health -= 1;

        if (health <= 0) StartCoroutine(Die());
        Debug.Log(health);
    }

    private IEnumerator Die() {
        GetComponentInChildren<Animator>().SetTrigger("Die");
        GetComponent<AudioSource>().PlayOneShot(DeathSound);
        Dead = true;

        foreach (var minion in Minions) {
            yield return new WaitForSeconds(Random.Range(0,0.2f));
            minion.GetComponentInChildren<Animator>().SetTrigger("Die");
        }
    }
}