using UnityEngine;
using System.Collections;

public class GravityHandle : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.05f);
    }
}
