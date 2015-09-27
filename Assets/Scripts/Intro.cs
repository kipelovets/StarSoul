using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Intro : MonoBehaviour 
{	
	public Transform eyeSockets;
	public Transform eyePupils;
	public Transform closedEyes;
	public Transform beard;
	public Transform mask;
	
	public float eyeRadius;
	public float eyeDown;
	public float eyeUp;
	public float maskMinScale;

	// Use this for initialization
	void Start () {
		eyeSockets.gameObject.SetActive(false);
		eyePupils.gameObject.SetActive(false);
		beard.gameObject.SetActive(false);
		StartCoroutine(GodAnimation());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	IEnumerator GodAnimation()
	{		
		yield return new WaitForSeconds(1f);
		
		GetComponent<AudioSource>().Play();
		
		eyePupils.gameObject.SetActive(true);
		eyeSockets.gameObject.SetActive(true);
		
		yield return new WaitForSeconds(1f);
		
		var seq = DOTween.Sequence();
		seq.Append(eyePupils.DOMoveX(eyeRadius, 0.2f));
		seq.AppendInterval(0.8f);
		seq.Append(eyePupils.DOMoveX(-eyeRadius, 0.3f));
		seq.AppendInterval(0.8f);
		seq.Append(eyePupils.DOMoveX(0f, 0.2f));

		seq.OnComplete(() => {
			StartCoroutine(BeardAnimation());
		});
	}
	
	IEnumerator BeardAnimation()
	{
		yield return new WaitForSeconds(1f);
		
		yield return StartCoroutine(Blink());
		yield return StartCoroutine(Blink());
		
		yield return new WaitForSeconds(1f);
		
		GetComponent<AudioSource>().Play();
		
		beard.gameObject.SetActive(true);
		
		yield return new WaitForSeconds(1.5f);

		var seq = DOTween.Sequence();
		seq.Append(eyePupils.DOMoveY(eyeDown, 0.3f));
		seq.AppendInterval(0.8f);
		seq.Append(eyePupils.DOMoveY(0f, 0.3f));
		seq.AppendInterval(2.0f);
		seq.AppendCallback(StartIrisAnimation);
		seq.Append(eyePupils.DOMoveY(eyeUp, 3f));
	}
	
	IEnumerator Blink()
	{
		eyePupils.gameObject.SetActive(false);
		eyeSockets.gameObject.SetActive(false);
		closedEyes.gameObject.SetActive(true);
		
		yield return new WaitForSeconds(0.1f);
		
		eyePupils.gameObject.SetActive(true);
		eyeSockets.gameObject.SetActive(true);
		closedEyes.gameObject.SetActive(false);
		
		yield return new WaitForSeconds(0.3f);
	}
	
	void StartIrisAnimation()
	{
		StartCoroutine(IrisAnimation());
	}
	
	IEnumerator IrisAnimation()
	{	
		mask.DOScale(Vector3.one * maskMinScale, 3f)
			.OnComplete(() => {
				Application.LoadLevel(Application.loadedLevel + 1);
			})
		;
		yield return null;

	}
}
