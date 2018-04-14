using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour {
    public int Width, Height;
    public float Stride;

    public void Move(Character player, Vector3 input) {
        var p = Pos(player.transform.position + input * Stride);
        Debug.Log(p);
        if (p.x < 0 || p.x >= Width || p.y < 0 || p.y >= Height) return;
        player.transform.position += input * Stride;
    }

    public Vector2 Pos(Vector3 pos) {
        return new Vector2(Mathf.Round(pos.x / Stride), Mathf.Round(pos.z / Stride));
    }

    private void OnDrawGizmos() {
        for (var x = 0; x < Width; x++)
        for (var y = 0; y < Height; y++)
            Gizmos.DrawWireCube(new Vector3(x * Stride, 0, y * Stride), new Vector3(Stride, 0.1f, Stride));
    }
}