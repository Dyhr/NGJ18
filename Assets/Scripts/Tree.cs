using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour {

	public Beater Beat;

	private void Start () {
		transform.eulerAngles = new Vector3(Random.Range(-5f,5f), Random.Range(0f,360f), Random.Range(-5f, 5f));
		transform.position -= Vector3.up * Random.Range(0f, 0.2f);
		transform.localScale = Vector3.one * Random.Range(0.9f, 1.1f);
		foreach (var meshRenderer in GetComponentsInChildren<MeshRenderer>()) {
			meshRenderer.material.color -= new Color(Random.Range(0f,0.2f),Random.Range(0f,0.2f),Random.Range(0f,0.2f));
		}
	}

	private void Update() {
		
	}
}
