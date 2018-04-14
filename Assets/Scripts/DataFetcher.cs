using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DataFetcher : MonoBehaviour {
    private void Start() {
        StartCoroutine(Fetch());
    }

    private IEnumerator Fetch() {
        while (true) {
            var req = UnityWebRequest.Get("http://34.240.78.46/fetch/");
            yield return req.SendWebRequest();
            if (req.isNetworkError || req.isHttpError) {
                Debug.LogError(req.error);
            } else {
                var actions = JsonUtility.FromJson<ActionList>(req.downloadHandler.text);
                foreach (var action in actions.actions) {
                    Debug.Log(action);
                }
            }
        }
    }
}

[Serializable]
public struct ActionList {
    public PlayerAction[] actions;
}
[Serializable]
public struct PlayerAction {
    public string type;
    public string id;
    public string name;

    public override string ToString() {
        return $"[{id } {type} {name}]";
    }
}