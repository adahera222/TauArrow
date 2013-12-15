using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TerrainType
{
    FLOOR_SQUARE,
}

public class TauTerrain : TauObject
{
	public TerrainType mType;
	

	public override void Awake()
	{
		base.Awake();
	}
	public override void InitStart()
	{
		base.InitStart();
	}
	public override void InitFinish()
	{
		base.InitFinish();
	}

	public override void SetActive(bool val)
	{
		gameObject.SetActive(val);
	}
    public override void Update()
    {

    }
}
