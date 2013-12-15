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

public enum SpriteType
{
	AIMDOWN,
	AIMUP,
	AIMSTRAIGHT,
	JUMP,
	CROUCH,
	LAND,
	YAY,
}
public class TauActor : TauObject
{
	public TauController controller;
	public bool isHuman = true;
	public Sprite[] bodySprites;
	public Sprite[] bowSprites;
	public SpriteType currentSprite;
	public SpriteRenderer bodySprite;
	public SpriteRenderer bowSprite;
	public bool faceLeft = true;

	public TauObject currentArrow;
	

	public override void Awake()
	{
		base.Awake();
	}
	public override void InitStart()
	{
		controller = this.GetComponent<TauController>();
		controller.InitStart(this);
		SetSprite(SpriteType.AIMSTRAIGHT);
		base.InitStart();
	}
	public override void InitFinish()
	{
		controller.InitFinish();
		base.InitFinish();
	}

	public override void SetActive(bool val)
	{

	}

	public void CheckFacing()
	{
		bool shouldFaceLeft = (controller.aimX > 0f);
		if (faceLeft == shouldFaceLeft)
		{
			return;
		}
		faceLeft = shouldFaceLeft;
		Vector3 newScale = Vector3.one;
		newScale.x = faceLeft ? 1f : -1f;
		bodySprite.gameObject.transform.localScale = newScale;
    	bowSprite.gameObject.transform.localScale = newScale;
    	
	}

	public SpriteType GetAim()
	{
		float absAngle = Mathf.Abs(controller.aimAngle);
		if (absAngle > 120f)
		{
			return SpriteType.AIMDOWN;
		}
		else if (absAngle < 60f)
		{
			return SpriteType.AIMUP;
		}
		else
		{
			return SpriteType.AIMSTRAIGHT;
		}
	}

    public override void Update()
    {
    	CheckFacing();
    	//if (controller.timeDirty || currentSprite != SpriteType.NORMAL)
    	{
    		if (controller.landDuration > 0f)
    		{
    			SetSprite(SpriteType.LAND);
    		}
    		else if (controller.jumpDuration > 0f)
    		{
    			SetSprite(SpriteType.JUMP);
    		}
    		else if (controller.crouchDuration > 0f)
    		{
    			SetSprite(SpriteType.CROUCH);
    		}
    		else if (controller.isFlying)
    		{
    			SetSprite(GetAim());		
    		}
    		else
    		{
    			SetSprite(GetAim());	
    		}
    	}
    	CheckArrow();
    }

    public void SetSprite(SpriteType val)
    {
    	if (val == currentSprite)
    	{
    		return;
    	}
    	currentSprite = val;
    	int mappedIndex = (int)val;
    	bodySprite.sprite = bodySprites[mappedIndex];
    	bowSprite.sprite = bowSprites[mappedIndex];

    }

    public void AddArrow(TauObject obj)
    {
    	obj.SetPhysicsEnabled(false);
    	currentArrow = obj;
    	CheckFacing();
    	CheckArrow();
    }

    public void CheckArrow()
    {

    	if (currentArrow != null)
    	{
    		currentArrow.transform.localScale = bowSprite.gameObject.transform.localScale;

    		Vector3 arrowPos = this.transform.position;
    		Vector3 arrowAngles = Vector3.zero;
    		float arrowScaleX = currentArrow.transform.localScale.x;
    		switch (currentSprite)
    		{
    			case SpriteType.AIMDOWN:
    			{
    				arrowPos.x += arrowScaleX*Globals.ARROW_POS_DOWN.x;
    				arrowPos.y += Globals.ARROW_POS_DOWN.y;
    				arrowAngles.z += arrowScaleX*Globals.ARROW_ROT_DOWN;
    				currentArrow.objSprite.enabled = true;
    				break;
    			}
    			case SpriteType.AIMUP:
    			{
    				arrowPos.x += arrowScaleX*Globals.ARROW_POS_UP.x;
    				arrowPos.y += Globals.ARROW_POS_UP.y;
    				arrowAngles.z += arrowScaleX*Globals.ARROW_ROT_UP;
    				currentArrow.objSprite.enabled = true;
    				break;
    			}
    			case SpriteType.AIMSTRAIGHT:
    			{
    				arrowPos.x += arrowScaleX*Globals.ARROW_POS_STRAIGHT.x;
    				arrowPos.y += Globals.ARROW_POS_STRAIGHT.y;
    				arrowAngles.z += arrowScaleX*Globals.ARROW_ROT_STRAIGHT;
    				currentArrow.objSprite.enabled = true;
    				break;
    			}
    			default:
    			{
    				currentArrow.objSprite.enabled = false;
    				break;
    			}
    		}
    		currentArrow.transform.position = arrowPos;
    		currentArrow.transform.localEulerAngles = arrowAngles;
    	}
    }

    public void ShootArrow(bool hasCharged)
    {
    	if (currentArrow != null)
    	{
    		float arrowScaleX = currentArrow.transform.localScale.x;
	    	float launchZ = arrowScaleX * currentArrow.transform.localEulerAngles.z * Mathf.Deg2Rad;
	    	float launchStrength = hasCharged ? 70f : 40f;
	    	float forceX = arrowScaleX * launchStrength * Mathf.Cos(launchZ);
	    	float forceY = launchStrength * Mathf.Sin(launchZ);
	    	currentArrow.rigidbody2D.isKinematic = false;
	    	currentArrow.rigidbody2D.AddForce(new Vector2(forceX,forceY));
			currentArrow.DelayedPhysics(0.2f);
	    	currentArrow = null;
	    }
	    else
	    {
	    	AddArrow(TauDirector.Instance.factory.GetNextArrow());
	    }
    }


}
