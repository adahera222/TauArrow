using UnityEngine;

public class Utilities
{
	
	public static float DistanceSquared(TauActor a, TauActor b)
	{
		Vector3 delta = a.gameObject.transform.position - b.gameObject.transform.position;
		return delta.sqrMagnitude;
	}
}