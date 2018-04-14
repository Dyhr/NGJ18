using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameGrid : MonoBehaviour {
    public int Width, Height;
    public float Stride;

    public AnimationCurve MoveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public AnimationCurve FallCurve = AnimationCurve.Linear(0, 0, 1, 1);

    private List<Character>[,] tiles;

    private void Start() {
        tiles = new List<Character>[Width, Height];
        for (var x = 0; x < Width; x++)
        for (var y = 0; y < Height; y++)
            tiles[x, y] = new List<Character>();
    }

    public IEnumerator Move(Character player, Vector3 input) {
        if (player.Moving) yield break;
        
        var origin = player.transform.position;
        var target = player.transform.position + input * Stride;
        var old = Pos(origin);
        var p = Pos(target);
        if (old == p) yield break;
        if (p.x < 0 || p.x >= Width || p.y >= Height) yield break;
        player.Moving = true;

        target.x = Mathf.Round(target.x / Stride) * Stride;
        target.z = Mathf.Round(target.z / Stride) * Stride;
        target.y = p.y >= 0 ? tiles[(int) p.x, (int) p.y].Count : 0;

        foreach (var character in tiles[(int) old.x, (int) old.y].SkipWhile(c => c != player).Skip(1))
            character.StartCoroutine(Fall(character));
        
        var tMax = MoveCurve.keys.Last().time;
        tiles[(int) old.x, (int) old.y].Remove(player);
        if(p.y >= 0)tiles[(int) p.x, (int) p.y].Add(player);
        for (float t = 0; t < tMax; t += Time.deltaTime) {
            yield return null;
            player.transform.position = Vector3.LerpUnclamped(origin, target, MoveCurve.Evaluate(t));
        }
        player.transform.position = target;

        if (p.y < 0) {
            tMax = FallCurve.keys.Last().time;
            origin = target;
            target += Vector3.down * 50;
            for (float t = 0; t < tMax; t += Time.deltaTime) {
                yield return null;
                player.transform.position = Vector3.LerpUnclamped(origin, target, FallCurve.Evaluate(t));
            }
            Destroy(player.gameObject);
        }

        player.Moving = false;
    }
    public IEnumerator Fall(Character player) {
        if (player.Moving) yield break;
        player.Moving = true;
        
        var origin = player.transform.position;
        var target = player.transform.position;
        var p = Pos(target);
        target.y = tiles[(int) p.x, (int) p.y].IndexOf(player)-1;
        
        var tMax = FallCurve.keys.Last().time;
        for (float t = 0; t < tMax; t += Time.deltaTime) {
            yield return null;
            player.transform.position = Vector3.LerpUnclamped(origin, target, FallCurve.Evaluate(t));
        }

        player.transform.position = target;
        player.Moving = false;
    }

    public Vector2 Pos(Vector3 pos) {
        return new Vector2(Mathf.Round(pos.x / Stride), Mathf.Round(pos.z / Stride));
    }

    private void OnDrawGizmos() {
        for (var x = 0; x < Width; x++)
        for (var y = 0; y < Height; y++)
            Gizmos.DrawWireCube(new Vector3(x * Stride, 0, y * Stride), new Vector3(Stride, 0.1f, Stride));
    }

    public void Add(Character character) {
        var p = Pos(character.transform.position);
        character.transform.position += Vector3.up * tiles[(int) p.x, (int) p.y].Count;
        tiles[(int) p.x, (int) p.y].Add(character);
    }

    public IEnumerator Push(Character character) {
        foreach (var other in Physics.OverlapSphere(character.transform.position, Stride * 2)
            .Where(c => c?.transform.parent?.GetComponent<Character>())
            .Select(c => c?.transform.parent?.GetComponent<Character>())) {
            if(character == other) continue;

            StartCoroutine(Move(other, (other.transform.position - character.transform.position).normalized));
        }

        yield return null;
    }
}