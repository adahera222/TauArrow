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

public struct ActorData
{
    public int HP;
    public int weapon;
}


public class TauActor : TauObject
{
    public ActorData aData;
	public TauController controller;
    public TauBody body;
	public bool isHuman = true;
    public int HP = 10;
    public int weapon = 0;
	
	public TauObject currentArrow;
	

	public override void Awake()
	{
		base.Awake();
	}
	public override void InitStart()
	{
		controller = this.GetComponent<TauController>();
		controller.InitStart(this);
        body = this.GetComponent<TauBody>();
        body.InitStart(this);
        if (isHuman)
        {
            aData = Globals.HeroAData;
        }
        else
        {
            aData = Globals.BaddieAData;    
        }
		base.InitStart();
	}
	public override void InitFinish()
	{
        HP = aData.HP;
		controller.InitFinish();
        body.InitFinish();
		base.InitFinish();
	}

	public override void SetActive(bool val)
	{

	}


    public override void Update()
    {
    	
    }

    public void AddArrow(TauObject obj)
    {
    	obj.SetPhysicsEnabled(false);
    	currentArrow = obj;
    	body.CheckFacing();
    	CheckArrow();
    }

    public void CheckArrow()
    {

    	if (currentArrow != null)
    	{
    		currentArrow.transform.localScale = body.bodySprite.gameObject.transform.localScale;

    		Vector3 arrowPos = this.transform.position;
    		Vector3 arrowAngles = Vector3.zero;
    		float arrowScaleX = currentArrow.transform.localScale.x;
    		switch (body.currentAction)
    		{
    			case SpriteAction.AIMDOWN:
    			{
    				arrowPos.x += arrowScaleX*Globals.ARROW_POS_DOWN.x;
    				arrowPos.y += Globals.ARROW_POS_DOWN.y;
    				arrowAngles.z += arrowScaleX*Globals.ARROW_ROT_DOWN;
    				currentArrow.objSprite.enabled = true;
    				break;
    			}
    			case SpriteAction.AIMUP:
    			{
    				arrowPos.x += arrowScaleX*Globals.ARROW_POS_UP.x;
    				arrowPos.y += Globals.ARROW_POS_UP.y;
    				arrowAngles.z += arrowScaleX*Globals.ARROW_ROT_UP;
    				currentArrow.objSprite.enabled = true;
    				break;
    			}
    			case SpriteAction.AIMSTRAIGHT:
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
