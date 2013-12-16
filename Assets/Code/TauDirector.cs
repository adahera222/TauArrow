using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TauDirector : MonoBehaviour 
{
	public static TauDirector instance;
	public static TauDirector Instance { get { return instance; } }

	public TauFactory factory;
	public TauWorld world;
	public TauLevel level;
	public TauActor hero;

	public bool isInit = false;
	private int enemyCount = 4;
	public bool isSpawning = false;

	public TauDirector()
	{
		instance = this;
	}

	// Use this for initialization
	IEnumerator Start () 
	{
		Globals.Load();
		//Time.timeScale = 0.2f;

		world = TauWorld.Instance;
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
		StartCoroutine(StartHero());
		yield return new WaitForSeconds(1f);
		while(!hero.isInit)
		{
			yield return null;
		}

		TauObject arrow = world.CreateArrow();
		hero.AddWeapon(arrow);

		
		isInit = true;

		audio.loop = true;
		audio.Play();
	}

	IEnumerator StartHero()
	{
		hero = world.CreateHero();
		while(!hero.isInit)
		{
			yield return null;
		}
		TauHUD.Instance.focused = hero.controller;

	}

	IEnumerator StartBaddies(int count)
	{
		isSpawning = true;
		for(int i=0; i<count; ++i)
		{
			yield return new WaitForSeconds(2f);
			TauActor baddie = world.CreateBaddie();	
			while(!baddie.isInit)
			{
				yield return null;
			}
			
		}
		isSpawning = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		int desiredCount = enemyCount;
		if (hero.isAlive)
		{
			desiredCount++;
		}
		else
		{
			StartCoroutine(StartHero());
		}

		int diff = desiredCount - world.ActorCount;
		if (!isSpawning && diff > 0)
		{
			Debug.Log("spawn "+diff+"actor="+world.ActorCount);
			StartCoroutine(StartBaddies(diff));
		}
	}

}
