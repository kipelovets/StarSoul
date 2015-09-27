using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Galaxy : MonoBehaviour 
{
	public Transform starPrefab;
	public float kernelRadius;
	public float armLength;
	public int count;
	public float maxRotationDegrees;
	
	Transform[] stars;
	Transform firstStar = null;

	// Use this for initialization
	public void Explode() 
	{
		Rect viewportRect = new Rect(0f, 0f, 1f, 1f);
		stars = new Transform[count];
		
		for (var i = 0; i < count; i++) {
			float height = Gaussian.Next(0f, armLength);
			
			var maxLocalRadius = height * kernelRadius / armLength;
			float localRadius = Mathf.Abs(Gaussian.Next(0f, maxLocalRadius)); 
			float angle = 2f * Mathf.PI * Random.value;
			float galaxyRotation = maxRotationDegrees * Mathf.Abs(height) / armLength;
			Vector3 pos = new Vector3(localRadius * Mathf.Cos(angle), localRadius * Mathf.Sin(angle), height);
			
			// Galaxy arm rotation
			pos = Quaternion.AngleAxis(galaxyRotation, Vector3.right) * pos;
			
			// Camera rotation
			pos = Quaternion.AngleAxis(90, Vector3.up) * pos;
			
			// Slight angle rotation
			pos = Quaternion.AngleAxis(50, Vector3.right) * pos;
			
			var star = (Transform)Instantiate(starPrefab, pos, Quaternion.identity);
			star.parent = transform;
			stars[i] = star;
			
			star.DOMove(Vector3.zero, 4f).From().SetEase(Ease.OutQuart);
			
			var fadeTime = 4.1f - Mathf.Abs(Gaussian.Next(0f, 1f));
			if (fadeTime < 0f) {
				fadeTime = 3.9f;
			}
			
			if (firstStar == null) {
				Vector3 viewportPos = Camera.current.WorldToViewportPoint(pos);
				if (viewportRect.Contains(viewportPos) && viewportPos.z > 0f) {
					firstStar = star;
				}
			}
			
			if (star != firstStar) {
				var seq = DOTween.Sequence();
				seq.AppendInterval(fadeTime);
				seq.OnComplete(() => { star.GetComponent<SpriteRenderer>().enabled = false; });
			}
		}
		
		DOTween.Sequence()
			.AppendInterval(6f)
			.OnComplete(() => {
				firstStar.GetComponent<Star>().Attention();
			});
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
