using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class Boss : MonoBehaviour {
    public AudioClip DeathSound;
    public float ExpectedDeathTime = 45;
    public float Health = 10;

    public bool Dead;

    public List<GameObject> Minions = new List<GameObject>();

    public Transform FirePrefab;
    public Transform ExplosionPrefab;
    public Transform DangerPrefab;
    public float FireFlightTime;

    private float spawnTime;
    private float health = 10;

    private void Start() {
        spawnTime = Time.time;
        ExpectedDeathTime += Time.time;
        health = Health;

        StartCoroutine(Attack());
    }

    private IEnumerator Attack() {
        var fetcher = FindObjectOfType<DataFetcher>();
        var grid = FindObjectOfType<GameGrid>();
        while (!Dead) {
            var targets = new List<Vector3>();
            for (var i = 0; i < 2; i++) {
                var target = grid.RandomPopulated();
                if (target.HasValue)
                    targets.Add(target.Value);
            }

            targets = targets.Distinct().ToList();

            var indicators = (from position in grid.Positions()
                where targets.Any(t => Vector3.Distance(t, position) < grid.Stride * 2)
                select Instantiate(DangerPrefab, position, Quaternion.Euler(90, 0, 0))).ToList();

            for (var i = 0; i < 3; i++){
                yield return new WaitUntil(() => fetcher.Beater.Loudness >= fetcher.Threshold);
                yield return new WaitForSeconds(1);
            }

            foreach (var indicator in indicators)
                Destroy(indicator.gameObject);

            foreach (var target in targets) StartCoroutine(FireBall(target));

            yield return new WaitUntil(() => fetcher.Beater.Loudness >= fetcher.Threshold);
        }
    }

    public IEnumerator FireBall(Vector3 target) {
        var grid = FindObjectOfType<GameGrid>();
        var origin = transform.position;
        var fire = Instantiate(FirePrefab, transform.position, Quaternion.identity);
        GetComponentInChildren<Animator>().SetTrigger("Projectile Attack");
        for (float t = 0; t < FireFlightTime; t += Time.deltaTime) {
            yield return null;
            fire.transform.position = Vector3.LerpUnclamped(origin, target, t / FireFlightTime);
        }

        var explosion = Instantiate(ExplosionPrefab, fire.transform.position, Quaternion.identity);
        Destroy(fire.gameObject);
        Destroy(explosion.gameObject, 0.75f);

        foreach (var player in grid.InRange(fire.transform.position, grid.Stride * 2).ToList()) {
            grid.StartCoroutine(grid.Move(player, Vector3.back * 3));
        }
    }

    public void Hurt() {
        if (health <= 0) return;

        var t = (Time.time - spawnTime) / ExpectedDeathTime;
        var h = (1 - t) * Health;
        var d = health - h;

        health -= Mathf.Max(0.75f, d);

        GetComponentInChildren<Image>().transform.localScale = new Vector3(Mathf.Max(0, health / Health), 1, 1);

        if (health <= 0) StartCoroutine(Die());
        else GetComponentInChildren<Animator>().SetTrigger("Take damage");
    }

    private IEnumerator Die() {
        Dead = true;
        GetComponent<AudioSource>().PlayOneShot(DeathSound);
        var from = Camera.main.transform.rotation;
        var to = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        for (var t = 0f; t < 0.5f; t += Time.deltaTime) {
            yield return null;
            Camera.main.fieldOfView = Mathf.Lerp(70, 40, t / 0.5f);
            Camera.main.transform.rotation = Quaternion.Lerp(from, to, t / 0.5f);
        }

        Camera.main.fieldOfView = 40;
        Camera.main.transform.rotation = to;
        var fetcher = FindObjectOfType<DataFetcher>();
        var text = GameObject.Find("FINAL");
        text.GetComponent<Text>().enabled = true;
        var wobble = StartCoroutine(WobbleText(text));

        yield return new WaitUntil(() => {
            var nPlayers = (float) FindObjectsOfType<Character>().Length;
            return fetcher.Buffer.Count(pair => pair.Value.name == "a") / nPlayers >= 0.75f;
        });

        yield return new WaitUntil(() => fetcher.Beater.Loudness >= fetcher.Threshold);
        yield return new WaitForSeconds(.5f);

        StopCoroutine(wobble);
        Destroy(text);

        GetComponentInChildren<Animator>().SetTrigger("Die");
        GetComponent<AudioSource>().PlayOneShot(DeathSound);

        foreach (var minion in Minions) {
            yield return new WaitForSeconds(Random.Range(0, 0.2f));
            minion.GetComponentInChildren<Animator>().SetTrigger("Die");
        }
    }

    private IEnumerator WobbleText(GameObject text) {
        var curve = AnimationCurve.EaseInOut(0, 0, .5f, .2f);
        curve.postWrapMode = WrapMode.PingPong;
        while (true) {
            text.transform.localScale = Vector3.one * (1 + curve.Evaluate(Time.time));
            yield return null;
        }
    }
}