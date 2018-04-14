using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
    public string Id;
    public float Stride;

    public void Init(PlayerAction action) {
        Id = action.id;
    }

    public void Action(PlayerAction action) {
        if (action.type != "button") return;

        switch (action.name) {
            case "up":
                Move(Vector3.forward);
                break;
            case "right":
                Move(Vector3.right);
                break;
            case "down":
                Move(Vector3.back);
                break;
            case "left":
                Move(Vector3.left);
                break;
            case "a":
                break;
            case "b":
                break;
            default:
                Debug.LogWarning($"Unknown button: {action.name}");
                break;
        }
    }

    public void Move(Vector3 input) {
        transform.position += input * Stride;
    }
}