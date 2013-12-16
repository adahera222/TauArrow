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
	public int enemyCount = 1;
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
		while(!hero.isInit)
		{
			yield return null;
		}

		TauObject arrow = world.CreateArrow();
		hero.AddWeapon(arrow);

		StartCoroutine(StartBaddies(enemyCount));
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

		if (!isSpawning && world.ActorCount < desiredCount)
		{
			StartCoroutine(StartBaddies(desiredCount - world.ActorCount));
		}
	}

}
