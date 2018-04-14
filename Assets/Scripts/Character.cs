using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour {
    public string Id;
    public string Name;
    public GameGrid Grid;
    public bool Moving;
    
    [Header("Settings")]
    public Transform ArrowPrefab;
    public float ArrowFlightTime;

    private Boss boss;

    private void FixedUpdate() {
        if (boss == null) boss = FindObjectOfType<Boss>();
    }

    public void Init(PlayerAction action) {
        Id = action.id;
        Name = action.name;
        Grid = FindObjectOfType<GameGrid>();

        name = Name;
        GetComponentInChildren<Text>().text = name.ToUpper();
        
        transform.position = new Vector3(Random.Range(0,Grid.Width),0,Random.Range(0,Grid.Height)) * Grid.Stride;
        Grid.Add(this);
        
        GetComponentInChildren<MeshRenderer>().material.color = Color.HSVToRGB(Random.Range(0f,1f),Random.Range(.4f,.6f),Random.Range(.6f,.9f));
    }

    public void Action(PlayerAction action) {
        if (action.type != "button") return;

        switch (action.name) {
            case "up":
                StartCoroutine(Grid.Move(this, Vector3.forward));
                break;
            case "right":
                StartCoroutine(Grid.Move(this, Vector3.right));
                break;
            case "down":
                StartCoroutine(Grid.Move(this, Vector3.back));
                break;
            case "left":
                StartCoroutine(Grid.Move(this, Vector3.left));
                break;
            case "a":
                StartCoroutine(Attack());
                break;
            case "b":
                StartCoroutine(Grid.Push(this));
                break;
            case "join":
                break;
            default:
                Debug.LogWarning($"Unknown button: {action.name}");
                break;
        }
    }

    private IEnumerator Attack() {
        if (boss == null) yield break;

        var dir = boss.transform.position - transform.position;
        var arrow = Instantiate(ArrowPrefab, transform.position, Quaternion.LookRotation(dir) * Quaternion.Euler(0,90,0));

        var origin = transform.position;
        var target = boss.transform.position + Vector3.up * 5;
        for (float t = 0; t < ArrowFlightTime; t += Time.deltaTime) {
            yield return null;
            arrow.transform.position = Vector3.LerpUnclamped(origin, target,  t/ArrowFlightTime);
        }
        
        Destroy(arrow.gameObject);
        boss.Hurt();
    }
}