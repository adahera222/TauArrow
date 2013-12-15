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

public enum Spritetime
{
	NORMAL,
	JUMP,
	LAND,
	YAY,
}
public class TauActor : TauObject
{
	public TauController controller;
	public bool isHuman = true;
	public Sprite[] sprites;
	public Spritetime currentSprite;
	public SpriteRenderer spriteRenderer;
	

	public override void Awake()
	{
		base.Awake();
	}
	public override void InitStart()
	{
		controller = this.GetComponent<TauController>();
		spriteRenderer = this.GetComponent<SpriteRenderer>();
		controller.InitStart(this);
		SetSprite(Spritetime.NORMAL);
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
    public override void Update()
    {
    	if (controller.timeDirty || currentSprite != Spritetime.NORMAL)
    	{
    		float currentTime = controller.timeAlive;
    		if (currentTime - controller.timeLanding < 0.2f)
    		{
    			SetSprite(Spritetime.LAND);
    		}
    		else if (currentTime - controller.timeJumping < 0.2f)
    		{
    			SetSprite(Spritetime.JUMP);
    		}
    		else if (!controller.isOnFloor)
    		{
    			SetSprite(Spritetime.JUMP);		
    		}
    		else
    		{
    			SetSprite(Spritetime.NORMAL);	
    		}
    	}
    }

    public void SetSprite(Spritetime val)
    {
    	if (val == currentSprite)
    	{
    		return;
    	}
    	currentSprite = val;
    	int mappedIndex = (int)val;
    	spriteRenderer.sprite = sprites[mappedIndex];
    }


}
