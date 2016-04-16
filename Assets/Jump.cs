using UnityEngine;
using System.Collections;

public class Jump : MonoBehaviour {
	
	// Update is called once per frame
	void Update ()
	{
	    transform.position = Vector3.up*Mathf.Abs(Mathf.Sin(Time.time));
	}
}
