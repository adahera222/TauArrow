using UnityEngine;

public class Utilities
{
	
	public static float DistanceSquared(TauObject a, TauObject b)
	{
		Vector3 delta = a.gameObject.transform.position - b.gameObject.transform.position;
		return delta.sqrMagnitude;
	}
}