using UnityEngine;
using System.Collections;

public class ParticleSystemAutoDestroy : MonoBehaviour 
{
     IEnumerator Start()
     {
         yield return new WaitForSeconds(GetComponent<ParticleSystem>().duration);
         Destroy(gameObject); 
     }
}
