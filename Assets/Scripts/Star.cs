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
		p.sunSecondsLeft = 30f + Random.value * 60f;
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
	public Galaxy galaxy;
	
	PlanetarySystem planetarySystemParams;
	Transform planetarySystem;
	
	Vector3 initialScale;
	Vector3 bigScale;
	bool exhausted = false;
	bool systemCreated = false;
	bool lit = false;

	// Use this for initialization
	void Start () {
		initialScale = transform.localScale;
		bigScale = 2f * initialScale;
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.LookAt(Camera.main.transform);
	}
	
	public void Attention()
	{
		transform.DOScale(bigScale, 0.3f)
			.SetLoops(-1, LoopType.Yoyo);	
	}
	
	public void OnPointerDown(PointerEventData eventData)
	{
		if (exhausted || !lit) {
			Debug.Log(this);
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
			gameObject.GetComponent<SpriteRenderer>().enabled = false;
			
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
			
			FindObjectOfType<Galaxy>().SetCurrentStar(this);
		}
	}
	
	public void HidePlanetarySystem()
	{
		GetComponent<SpriteRenderer>().enabled = true;
		var sun = planetarySystem.GetComponentInChildren<Sun>();
		var planet = planetarySystem.GetComponentInChildren<Planet>();
		planetarySystemParams.sunEnergyLeft = sun.energyLeft;
		planetarySystemParams.creatureCounts = planet.SerializeLife();
		Destroy(planetarySystem.gameObject);
		planetarySystem = null;
	}
	
	public bool CanExplode()
	{
		return !systemCreated && !exhausted && !lit;
	}
	
	public void Explode()
	{
		GetComponent<Collider2D>().enabled = true;
		lit = true;
		var nova = transform.GetChild(0).GetComponent<SpriteRenderer>();
		nova.enabled = true;
		
		var seq = DOTween.Sequence();
		seq.AppendInterval(0.3f);
		seq.Append(nova.transform.DOPunchScale(nova.transform.localScale * 5f, 0.3f));
		seq.OnComplete(() => {
			nova.enabled = false;
			GetComponent<SpriteRenderer>().enabled = true;
		});
	}
}
