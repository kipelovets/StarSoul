using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class Planet : MonoBehaviour 
{
	public int maxCreatures;
	public Sun sun;
	public Transform atmosphere;
	public Transform[] creatures;
	
	private List<Transform> life;
	private float atmosphereScale;

	// Use this for initialization
	void Start () 
	{
		life = new List<Transform>();
		InvokeRepeating("Breed", 1f, 0.6f);
		InvokeRepeating("Spawn", 1f, 1f);
		InvokeRepeating("Die", 1f, 1.3f);
		atmosphereScale = atmosphere.localScale.x;
	}
	
	public void Init(Dictionary<int, int> newCreatures)
	{
		foreach (KeyValuePair<int, int> creatureCount in newCreatures) {
			for (int i = 0; i < creatureCount.Value; i++) {
				AddCreature(creatures[creatureCount.Key]);
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		var aScale = atmosphereScale * (0.5f + 0.5f * (maxCreatures - life.Count) / maxCreatures);
		var scale = new Vector3(aScale, aScale, aScale);
		atmosphere.localScale = scale;
	}
	
	void Breed()
	{
		if (life.Count > 0 && sun.GetUVLevel() * Random.value >= 0.1f) {
			Debug.Log("Breed");
			var parent = life[Random.Range(0, life.Count)];
			AddCreature(parent);
		}
	}
	
	void Spawn()
	{
		if (sun.GetGammaLevel() * Random.value >= 0.1f) {
			Debug.Log("Spawn");
			var parent = creatures[Random.Range(0, creatures.Length)];
			AddCreature(parent);
		}
	}
	
	void Die()
	{
		float test = (sun.GetGammaLevel() + sun.GetIRLevel()) * Random.value / 2f;
		if (life.Count > 0 && test >= 0.1f) {
			Debug.Log("DIE");
			var creature = life[0];
			life.RemoveAt(0);
			Destroy(creature.gameObject);
		}
	}
	
	void AddCreature(Transform parent)
	{
		var rotationZ = Random.value * 360f; 
		var creature = (Transform)Instantiate(parent, parent.transform.position, Quaternion.identity);
		creature.SetParent(gameObject.transform);
		creature.localPosition = Vector3.zero;
		creature.eulerAngles = new Vector3(0f, 0f, rotationZ);
		float angle = (Random.Range(0, 2) * 2 - 1);
		var scale = creature.localScale;
		scale.y *= -angle;
		creature.localScale = scale;
		creature.DOKill();
		float speed = Random.value * 0.4f + 0.8f; 
		creature.DORotate(creature.rotation.eulerAngles + new Vector3(0f, 0f, angle*speed), 0.3f)
			.SetLoops(-1, LoopType.Incremental);
		life.Add(creature);
	}
}
