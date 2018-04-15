using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour {

	public GameObject BossPrefab;
	public GameObject MinionPrefab;

	public int NumberOfMinions;

	private bool spawning;
	
	private void Update () {
		if (Input.GetKey(KeyCode.Space) && !spawning) {
			spawning = true;
			StartCoroutine(Spawn());
		}
	}

	private IEnumerator Spawn() {
		Debug.Log("Starting...");
		var boss = Instantiate(BossPrefab, transform.position, Quaternion.Euler(0,180,0));

		for (int i = 0; i < NumberOfMinions; i++) {
			yield return new WaitForSeconds(Random.Range(0.1f,0.4f));
			boss.GetComponent<Boss>().Minions.Add(
				Instantiate(
					MinionPrefab, 
					transform.position + new Vector3(Random.Range(-8f,8f),0,Random.Range(-2f,3f)),
					Quaternion.Euler(0, Random.Range(0f, 360f), 0)));
		}
			
		Destroy(this);
	}
}
