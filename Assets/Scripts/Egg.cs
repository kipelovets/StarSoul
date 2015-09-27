using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Egg : MonoBehaviour 
{
	public Transform linePrefab;
	public Transform egg;

	// Use this for initialization
	void Start () 
	{
		StartCoroutine(Grow());
	}
	
	IEnumerator Grow() 
	{
		int count = 5;
		var lines = new Transform[count];
		for (var i = 0; i < count; i++) {
			float angle = 360f * Random.value - 180f;
			Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
			Transform line = (Transform)Instantiate(linePrefab, Vector3.zero, rotation);
			line.localScale = Vector3.one * (1f + Random.value) / 2;
			line.DORotate(rotation.eulerAngles * 5f, 5f, RotateMode.LocalAxisAdd)
				.SetEase(Ease.Linear);
			lines[i] = line;
				
			yield return new WaitForSeconds(0.1f);
		}
		
		for (var i = 0; i < count; i++) {
			Destroy(lines[i].gameObject);
			yield return new WaitForSeconds(0.1f);
		}
		
		egg.DOPunchScale(Vector3.one, 1f)
			.OnComplete(() => {
				var galaxy = GameObject.Find("Galaxy");
				galaxy.GetComponent<Galaxy>().Explode();
				egg.gameObject.SetActive(false);
			});
		
		yield return null;
	}
}
