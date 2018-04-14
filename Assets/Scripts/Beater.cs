using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Beater : MonoBehaviour {

	public float Bpm = 120;
	public AudioSource Source;

	private float loudness;
	public float Loudness => loudness;

	private float[] clipData;
	private float lastUpdate;
	private float updateStep = 0.01f;
	private int sampleDataLength = 1024;
	
	private void Start () {
		if (!Source) {
			Debug.LogError(GetType() + "Awake: there was no audioSource set.");
		}
		clipData = new float[sampleDataLength];
	}
    private void Update () {
	    if (!(Time.time - lastUpdate >= updateStep)) return;
	    
	    lastUpdate = Time.time;
	    Source.clip.GetData(clipData, Source.timeSamples); //I read 1024 samples, which is about 80 ms on a 44khz stereo clip, beginning at the current sample position of the clip.
	    loudness = clipData.Average(sample => Mathf.Abs(sample));

	    transform.localScale = Vector3.one * (.5f + loudness);
    }
}
