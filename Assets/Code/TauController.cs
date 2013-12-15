using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct ControllerData
{
	public float normalJumpForce;
	public float crouchJumpForce;
	public float runForce;
	public float flyForce;
	public float maxRunVelocity;
	public float jumpDuration;
	public float landDuration;
	public float crouchDuration;
	public float chargeDuration;
}

public class TauController : MonoBehaviour
{
	private TauActor actor;
	private ControllerData cData;
	public bool isInit = false;
	public bool isFlying = true;
	public float aimX = 0f;
	public float aimY = 0f;
	public float aimAngle = 0f;

	public float velX = 0f;
	public float velY = 0f;


	public HashSet<GameObject> contactFloors = new HashSet<GameObject>();

	public float timeAlive = -1f;
	public float jumpDuration = -1f;
	public float landDuration = -1f;
	public float crouchDuration = -1f;
	public float chargeDuration = -1f;
	public bool timeDirty = false;
	public bool didCrouch = false;

	public bool CanJump { get { return !isFlying || contactFloors.Count > 0; } }


	public void InitStart(TauActor p_actor)
	{
		actor = p_actor;
		if (actor.isHuman)
		{
			InputManager.Instance.AddInput(HandleAxis);
			InputManager.Instance.AddInput(HandleButton);
			cData = Globals.HeroCData;
		}
		else
		{
			cData = Globals.BaddieCData;	
		}
		
		rigidbody2D.fixedAngle = true;
	}
	public void InitFinish()
	{
		isInit = true;
	}
	
    public void Update()
    {
    	float deltaTime = Time.deltaTime;
    	timeAlive += deltaTime;
    	if (jumpDuration > 0f) { jumpDuration -= deltaTime; }
    	if (landDuration > 0f) { landDuration -= deltaTime; }
    	if (chargeDuration > 0f) { chargeDuration -= deltaTime; }
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

    	if (actor.isHuman)
    	{
	    	aimX = InputManager.Instance.MouseVec.x - gameObject.transform.position.x;
	    	aimY = InputManager.Instance.MouseVec.y - gameObject.transform.position.y;
	    	aimAngle = Mathf.Atan2(aimX, aimY) * Mathf.Rad2Deg;
	    }
	    velX = Mathf.Clamp(rigidbody2D.velocity.x, -cData.maxRunVelocity, cData.maxRunVelocity);
	    velY = rigidbody2D.velocity.y;
	    rigidbody2D.velocity = new Vector2(velX, velY);
    }


	public void HandleButton(InputButtonType btype, bool isDown)
	{
		if (isDown)
		{
			chargeDuration = cData.chargeDuration;
		}
		else
		{
			actor.ShootArrow(chargeDuration <= 0f);
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
    		else
    		{
    			chargeDuration = -1f;
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
