using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour {
    public string Id;
    public string Name;
    public GameGrid Grid;
    public bool Moving;

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
}