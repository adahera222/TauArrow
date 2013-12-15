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


}
