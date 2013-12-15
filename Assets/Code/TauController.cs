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
	public float reloadDuration;
}

public class TauController : MonoBehaviour
{
	private TauActor actor;
	private ControllerData cData;

	public InputButtonType shootButton;
	public InputButtonType retrieveButton;
	public bool isInit = false;
	public bool isFlying = true;
	public float aimX = 0f;
	public float aimY = 0f;
	public float aimAngle = 0f;

	public float velX = 0f;
	public float velY = 0f;

	public float posX = 0f;
	public float posY = 0f;

	public HashSet<GameObject> contactFloors = new HashSet<GameObject>();

	public float timeAlive = -1f;
	public float jumpDuration = -1f;
	public float landDuration = -1f;
	public float crouchDuration = -1f;
	public float chargeDuration = -1f;
	public float reloadDuration = -1f;
	public bool timeDirty = false;
	public bool didCrouch = false;
	public bool isShooting = false;

	public bool ValidJump { get { return !isFlying || contactFloors.Count > 0; } }
	public bool CanJump { get { return ValidJump && jumpDuration <= 0f && landDuration <= 0f; } }
	public bool CanLand { get { return !isFlying && jumpDuration <= 0f && landDuration <= 0f; } }
	public bool CanCrouch { get { return !isFlying && crouchDuration <= 0f; } }
	public bool CanReload { get { return reloadDuration <= 0f; } }
	


	public void InitStart(TauActor p_actor)
	{
		actor = p_actor;
		shootButton = InputButtonType.MOUSE_LEFT;
		retrieveButton = InputButtonType.MOUSE_RIGHT;
		if (actor.isHuman)
		{
			InputManager.Instance.AddInput(HandleAxis);
			InputManager.Instance.AddInput(HandleButton);
			cData = Globals.HeroCData;
		}
		else
		{
			cData = Globals.BaddieCData;	
			actor.ai = this.gameObject.AddComponent<TauAI>();
			actor.ai.InitStart(actor, this);
		}
		
		rigidbody2D.fixedAngle = true;

		InitFinish();
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
    	if (reloadDuration > 0f) { reloadDuration -= deltaTime; }
    	if (crouchDuration > 0f) 
    	{ 
    		crouchDuration -= deltaTime; 
    		didCrouch = crouchDuration <= 0f; 
    	}

    	posX = gameObject.transform.position.x;
    	posY = gameObject.transform.position.y;

    	bool hasY = Mathf.Abs(rigidbody2D.velocity.y) > 0.01f;
    	if (hasY != isFlying)
    	{
    		isFlying = hasY;
    		if (CanLand)
    		{
    			landDuration = cData.landDuration;
				timeDirty = true;
    		}
    	}

    	if (actor.isHuman)
    	{
	    	SetAim(InputManager.Instance.MouseVec.x - posX, InputManager.Instance.MouseVec.y - posY);
	    	
	    }
	    velX = Mathf.Clamp(rigidbody2D.velocity.x, -cData.maxRunVelocity, cData.maxRunVelocity);
	    velY = rigidbody2D.velocity.y;
	    rigidbody2D.velocity = new Vector2(velX, velY);
    }

    public void SetAim(float x, float y)
    {
    	aimX = x;
    	aimY = y;
    	aimAngle = Mathf.Atan2(aimX, aimY) * Mathf.Rad2Deg;
    }


	public void HandleButton(InputButtonType bType, bool isDown)
	{
		if (bType == this.shootButton)
		{
			if (actor.currentArrow != null)
			{
				if (isDown && !isShooting)
				{
					chargeDuration = cData.chargeDuration;
					isShooting = true;
				}
				else if (!isDown && isShooting)
				{
					actor.ShootArrow(chargeDuration <= 0f);
					reloadDuration = cData.reloadDuration;
					chargeDuration = -1f;
					isShooting = false;
				}
			}
			else
			{
				if (isDown && CanReload)
				{
					actor.AddArrow(TauDirector.Instance.factory.GetNextArrow());
				}
			}
		}
		else if (bType == this.retrieveButton)
		{
			if (isDown)
			{
				isShooting = false;
				actor.RetrieveArrow();
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
    			if (CanJump)
		    	{
		    		forceY *= didCrouch ? cData.crouchJumpForce : cData.normalJumpForce;
    				didCrouch = false;
	    			jumpDuration = cData.jumpDuration;
	    			timeDirty = true;
	    			rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0f);
	    		}
	    		else
	    		{
	    			forceY = 0f;
		    	}
    		}
    		else if (forceY < -0.01f)
    		{
    			if (CanCrouch)
    			{
    				forceY = 0f;
    				crouchDuration = cData.crouchDuration;
	    			timeDirty = true;	
    			}
    			else
    			{
    				forceY = 0f;
	    		}
    		}
    		else
    		{
    			crouchDuration = -1f;
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
