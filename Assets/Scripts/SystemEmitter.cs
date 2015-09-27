using UnityEngine;
using System.Collections;

public class SystemEmitter : MonoBehaviour 
{
	public Transform planetarySystemPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			Debug.Log("Emit");
			
			
			var cam = Camera.main;
			Transform tr = (Transform)Instantiate(planetarySystemPrefab);
			
			tr.position = (cam.transform.rotation * tr.localPosition) 
				+ cam.transform.position 
				+ cam.transform.forward * 10f;
				
			tr.rotation = cam.transform.rotation; 
			
		}
	}
}
