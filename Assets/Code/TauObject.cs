using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*
     _
  _.;_'-._
 {`--.-'_,}
{; \,__.-'/}
{.'-`._;-';
 `'--._.-'
    .-\\,-"-.
    `- \( '-. \
        \;---,/
    .-""-;\
   /  .-' )\   
   \,---'` \\
            \|
*/

public class TauObject : MonoBehaviour
{
	//public Rigidbody2D rigidbody;
	//public BoxCollider2D collider;
	
	public bool isInit = false;
	public virtual void Awake()
	{
		InitStart();
	}
	public virtual void InitStart()
	{
		InitFinish();
	}
	public virtual void InitFinish()
	{
		isInit = true;
	}

	public virtual void SetActive(bool val)
	{

	}
    public virtual void Update()
    {

    }
}
