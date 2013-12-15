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

public enum SpriteAction
{
	AIMDOWN,
	AIMUP,
	AIMSTRAIGHT,
	JUMP,
	CROUCH,
	LAND,
	YAY,
}

public enum SpriteFace
{
	GHOST,
	GHOST_HURT,
	BUG,
	BUG_HURT,
	SNEAK,
	SNEAK_HURT,
	ALIEN,
	ALIEN_HURT,
}


public class TauBody : MonoBehaviour
{
	private TauActor actor;
	public Sprite[] bodySprites;
	public Sprite[] bowSprites;
	public Sprite[] faceSprites;
	public SpriteFace currentFace;
	public SpriteAction currentAction;
	public SpriteRenderer bodySprite;
	public SpriteRenderer bowSprite;
	public SpriteRenderer faceSprite;
	public bool faceLeft = true;

	
	public void InitStart(TauActor p_actor)
	{
		actor = p_actor;
		SetSprite(SpriteAction.AIMSTRAIGHT);
		SetSprite(SpriteFace.BUG);
	}
	public void InitFinish()
	{
	}

	public void SetActive(bool val)
	{

	}

	public void CheckFacing()
	{
		bool shouldFaceLeft = (actor.controller.aimX > 0f);
		if (faceLeft == shouldFaceLeft)
		{
			return;
		}
		faceLeft = shouldFaceLeft;
		Vector3 newScale = Vector3.one;
		newScale.x = faceLeft ? 1f : -1f;
		bodySprite.gameObject.transform.localScale = newScale;
		if (bowSprite != null)
		{
    		bowSprite.gameObject.transform.localScale = newScale;
    	}
    	if (faceSprite != null)
    	{
    		faceSprite.gameObject.transform.localScale = newScale;	
    	}
    	
	}

	public SpriteAction GetAim()
	{
		float absAngle = Mathf.Abs(actor.controller.aimAngle);
		if (absAngle > 120f)
		{
			return SpriteAction.AIMDOWN;
		}
		else if (absAngle < 60f)
		{
			return SpriteAction.AIMUP;
		}
		else
		{
			return SpriteAction.AIMSTRAIGHT;
		}
	}

    public void Update()
    {
    	CheckFacing();
    	
    	{
    		if (actor.controller.landDuration > 0f)
    		{
    			SetSprite(SpriteAction.LAND);
    		}
    		else if (actor.controller.jumpDuration > 0f)
    		{
    			SetSprite(SpriteAction.JUMP);
    		}
    		else if (actor.controller.crouchDuration > 0f)
    		{
    			SetSprite(SpriteAction.CROUCH);
    		}
    		else if (actor.controller.isFlying)
    		{
    			SetSprite(GetAim());		
    		}
    		else
    		{
    			SetSprite(GetAim());	
    		}
    	}
    	actor.CheckArrow();
    }

    public void SetSprite(SpriteFace val)
    {
    	if (val == currentFace)
    	{
    		return;
    	}
    	currentFace = val;
    	int mappedIndex = (int)val;
    	if (faceSprite != null)
    	{
    		faceSprite.sprite = faceSprites[mappedIndex];
    	}
    }

    public void SetSprite(SpriteAction val)
    {
    	if (val == currentAction)
    	{
    		return;
    	}
    	currentAction = val;
    	int mappedIndex = (int)val;
    	bodySprite.sprite = bodySprites[mappedIndex];
    	if (bowSprite != null)
    	{
    		bowSprite.sprite = bowSprites[mappedIndex];
    	}
    }



}
