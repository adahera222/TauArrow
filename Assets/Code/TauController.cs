﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct ControllerData
{
	public float normalJumpForce;
	public float crouchJumpForce;
	public float runForce;
	public float flyForce;
	public float jumpDuration;
	public float landDuration;
	public float crouchDuration;
}

public class TauController : MonoBehaviour
{
	private TauActor actor;
	private ControllerData cData;
	public bool isInit = false;
	public bool isFlying = true;
	public HashSet<GameObject> contactFloors = new HashSet<GameObject>();

	public float timeAlive = -1f;
	public float jumpDuration = -1f;
	public float landDuration = -1f;
	public float crouchDuration = -1f;
	public bool timeDirty = false;
	public bool didCrouch = false;

	public bool CanJump { get { return !isFlying || contactFloors.Count > 0; } }


	public virtual void InitStart(TauActor p_actor)
	{
		actor = p_actor;
		if (actor.isHuman)
		{
			InputManager.Instance.AddInput(HandleAxis);
		}
		cData = Globals.Hero;
		rigidbody2D.fixedAngle = true;
	}
	public virtual void InitFinish()
	{
		isInit = true;
	}
	
    public virtual void Update()
    {
    	float deltaTime = Time.deltaTime;
    	timeAlive += deltaTime;
    	if (jumpDuration > 0f) { jumpDuration -= deltaTime; }
    	if (landDuration > 0f) { landDuration -= deltaTime; }
    	if (crouchDuration > 0f) 
    	{ 
    		crouchDuration -= deltaTime; 
    		didCrouch = crouchDuration <= 0f; 
    	}

    	bool hasY = Mathf.Abs(rigidbody2D.velocity.y) > 0.01f;
    	if (hasY != isFlying)
    	{
    		isFlying = hasY;
    		if (!isFlying && jumpDuration <= 0f && landDuration <= 0f)
    		{
    			landDuration = cData.landDuration;
				timeDirty = true;
    		}
    	}
    }

    public void HandleAxis(float x, float y)
    {
    	
    	if (Mathf.Abs(x) > 0 || Mathf.Abs(y) > 0)
    	{
    		float forceX = x;
    		float forceY = y;
    		forceX *= isFlying ? cData.flyForce : cData.runForce;
    		if (forceY > 0.01f)
    		{
    			if (!CanJump || jumpDuration > 0f || landDuration > 0f)
		    	{
		    		forceY = 0f;
		    	}
		    	else
		    	{
		    		forceY *= didCrouch ? cData.crouchJumpForce : cData.normalJumpForce;
    				didCrouch = false;
	    			jumpDuration = cData.jumpDuration;
	    			timeDirty = true;
	    			rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0f);
	    		}
    		}
    		else if (forceY < -0.01f)
    		{
    			if (isFlying || crouchDuration > 0f)
    			{
    				forceY = 0f;
    			}
    			else
    			{
    				forceY = 0f;
	    			crouchDuration = cData.crouchDuration;
	    			timeDirty = true;	
	    		}
    		}

    		rigidbody2D.AddForce(new Vector2(forceX,forceY));
    	}
    }

    public void OnCollisionEnter2D(Collision2D coll)
    {
    	if (coll.gameObject.tag == Globals.TERRAIN)
    	{
	    	if (contactFloors.Contains(coll.gameObject))
	    	{
	    		return;
	    	}
	    	contactFloors.Add(coll.gameObject);
    	}
    }

    public void OnCollisionExit2D(Collision2D coll)
    {
    	if (coll.gameObject.tag == Globals.TERRAIN)
    	{
	    	if (!contactFloors.Contains(coll.gameObject))
	    	{
	    		return;
	    	}
	    	contactFloors.Remove(coll.gameObject);
	    }
    }
}
