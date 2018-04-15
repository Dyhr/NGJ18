using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour {

	public Beater Beat;
	public Boss Boss;

	private Vector3 scale;
	private float beatFactor;

	private void Start () {
		transform.eulerAngles = new Vector3(Random.Range(-5f,5f), Random.Range(0f,360f), Random.Range(-5f, 5f));
		transform.position -= Vector3.up * Random.Range(0f, 0.2f);
		transform.localScale = scale = Vector3.one * Random.Range(0.9f, 1.1f);
		foreach (var meshRenderer in GetComponentsInChildren<MeshRenderer>()) {
			meshRenderer.material.color -= new Color(Random.Range(0f,0.2f),Random.Range(0f,0.2f),Random.Range(0f,0.2f));
		}

		beatFactor = Random.Range(0f, 1f);
	}

	private void Update() {
		if (Beat == null) {
			Beat = FindObjectOfType<Beater>();
			return;
		}
		if (Boss == null) {
			Boss = FindObjectOfType<Boss>();
			return;
		}
		if (Boss.Dead) return;

		transform.localScale = scale + Vector3.one * Beat.Loudness * 0.15f * beatFactor;
		transform.localRotation *= Quaternion.Euler(0,Beat.Loudness * 10f * beatFactor,0);
	}
}
