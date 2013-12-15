using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TauDirector : MonoBehaviour 
{
	public TauFactory factory;
	public TauLevel level;

	public bool isInit = false;

	// Use this for initialization
	IEnumerator Start () 
	{
		Globals.Load();
		
		level.LoadLevel();
		while(!level.isLoaded)
		{
			yield return null;
		}
		level.MakeLevel();
		while(!level.isInit)
		{
			yield return null;
		}
		TauActor hero = factory.GetNextHero();
		while(!hero.isInit)
		{
			yield return null;
		}
		isInit = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
