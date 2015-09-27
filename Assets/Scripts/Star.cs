using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

struct PlanetarySystem {
	public float sunSecondsLeft;
	public float sunUV;
	public float sunIR;
	public float sunGamma;
	public int sunEnergyLeft;
	public Dictionary<int, int> creatureCounts;
	
	public static PlanetarySystem create()
	{
		var p = new PlanetarySystem();
		p.sunSecondsLeft = 10f;
		p.sunUV = 0.25f;
		p.sunIR = 0.15f;
		p.sunGamma = 0.15f;
		p.sunEnergyLeft = 3;
		p.creatureCounts = new Dictionary<int, int>();
		return p;
	}
}

public class Star : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
	public Transform planetarySystemPrefab;
	
	PlanetarySystem planetarySystemParams;
	Transform planetarySystem;
	
	Vector3 initialScale;
	Vector3 bigScale;
	bool exhausted = false;
	bool systemCreated = false;

	// Use this for initialization
	void Start () {
		initialScale = transform.localScale;
		bigScale = 2f * initialScale;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Attention()
	{
		transform.DOScale(bigScale, 0.3f)
			.SetLoops(-1, LoopType.Yoyo);
			
		gameObject.GetComponent<Collider2D>().enabled = true;
	}
	
	public void OnPointerDown(PointerEventData eventData)
	{
		if (exhausted) {
			return;
		}
		Camera.main.transform.DOMove(transform.position * 1.1f, 1f);
		Camera.main.transform.DOLookAt(-transform.position, 1f)
			.OnComplete(ShowPlanetarySystem);
	}
	
	public void OnPointerEnter(PointerEventData eventData)
	{
		transform.DOKill(false);
		
		transform.localScale = initialScale;
		transform.DOScale(bigScale, 0.2f);
	}
	
	public void OnPointerExit(PointerEventData eventData)
	{
		transform.DOKill(false);
		
		transform.localScale = bigScale;
		transform.DOScale(initialScale, 0.2f);
	}
	
	void ShowPlanetarySystem()
	{
		if (!systemCreated && !exhausted) {
			planetarySystemParams = PlanetarySystem.create();
			
			var cam = Camera.main;
			planetarySystem = (Transform)Instantiate(planetarySystemPrefab);
			
			planetarySystem.position = (cam.transform.rotation * planetarySystem.localPosition) 
				+ cam.transform.position 
				+ cam.transform.forward * 10f;
				
			planetarySystem.rotation = cam.transform.rotation;
			
			var sun = planetarySystem.GetComponentInChildren<Sun>();
			sun.uv = planetarySystemParams.sunUV;
			sun.ir = planetarySystemParams.sunIR;
			sun.gamma = planetarySystemParams.sunGamma;
			sun.lifeTime = planetarySystemParams.sunSecondsLeft;
			sun.energyLeft = planetarySystemParams.sunEnergyLeft;
			planetarySystem.GetComponentInChildren<Planet>().Init(planetarySystemParams.creatureCounts);
		}
	}
}
