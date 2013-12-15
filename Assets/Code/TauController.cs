using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TauController : MonoBehaviour
{
	public bool isInit = false;
	public virtual void Awake()
	{
		InitStart();
	}
	public virtual void InitStart()
	{
		
	}
	public virtual void InitFinish()
	{
		isInit = true;
	}
	
    public virtual void Update()
    {

    }
}
