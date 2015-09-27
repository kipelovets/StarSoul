using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Sun : MonoBehaviour 
{
	public Transform ray1;
	public Transform ray2;
	public Transform ray3;
	public Transform sun;
	public Color reddish;
	public Color red;
	
	public float lifeTime;
	public float uv;
	public float ir;
	public float gamma;
	public int energyLeft;
	
	public bool dead = false;

	void Start () 
	{
		StartCoroutine(RotateRays());
		StartCoroutine(Life());
	}
	
	IEnumerator RotateRays()
	{
		while (true) {
			ray1.Rotate(0, 0, 0.2f);
			ray2.Rotate(0, 0, -0.2f);
			ray3.Rotate(0, 0, 0.1f);
			yield return null;
		}
	}
	
	IEnumerator Life()
	{
		float lifeThird = lifeTime / 3f;
		yield return new WaitForSeconds(lifeThird);
		//ray3.GetComponent<SpriteRenderer>().DOColor(Color.black, 0.5f);
		sun.GetComponent<SpriteRenderer>().DOColor(reddish, 0.5f);
		
		yield return new WaitForSeconds(lifeThird);
		//ray2.GetComponent<SpriteRenderer>().DOColor(Color.black, 0.5f);
		sun.GetComponent<SpriteRenderer>().DOColor(red, 0.5f);
		
		yield return new WaitForSeconds(lifeThird);
		//ray1.GetComponent<SpriteRenderer>().DOColor(Color.black, 0.5f);
		sun.GetComponent<SpriteRenderer>().DOColor(Color.red, 0.6f);
		
		yield return new WaitForSeconds(0.5f);
		dead = true;		
	}
	
	// breed
	public float GetUVLevel()
	{
		return dead ? 0 : uv;
	}
	
	// death
	public float GetIRLevel()
	{
		return dead ? 1f : ir;
	}
	
	// spawn / death
	public float GetGammaLevel()
	{
		return dead ? 0 : gamma;
	}
}
