using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TauDirector : MonoBehaviour 
{
	public static TauDirector instance;
	public static TauDirector Instance { get { return instance; } }

	public TauFactory factory;
	public TauLevel level;

	public bool isInit = false;

	public TauDirector()
	{
		instance = this;
	}

	// Use this for initialization
	IEnumerator Start () 
	{
		Globals.Load();
		//Time.timeScale = 0.2f;

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
		TauObject arrow = factory.GetNextArrow();
		hero.AddArrow(arrow);

		for(int i=0; i<3; ++i)
		{
			TauActor baddie = factory.GetNextBaddie();
			while(!baddie.isInit)
			{
				yield return null;
			}
		}
		isInit = true;

		audio.loop = true;
		audio.Play();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
