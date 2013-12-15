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
    public TauAI ai;
	public bool isHuman = true;
    public int HP = 10;
    public int weapon = 0;
	
	public TauObject currentArrow;
    public TauObject retrieveArrow;
	

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
    	CheckRetrieveArrow();
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
    }

    public void RetrieveArrow()
    {
        if (retrieveArrow == null)
        {
            retrieveArrow = TauWorld.Instance.FindArrow(this);
            retrieveArrow.rigidbody2D.velocity *= 0f;
        }

    }
    public void CheckRetrieveArrow()
    {
        if (retrieveArrow != null)
        {
            float deltaX = controller.posX - retrieveArrow.gameObject.transform.position.x;
            float deltaY = controller.posY - retrieveArrow.gameObject.transform.position.y;
            if (Mathf.Abs(deltaX) < 1f && Mathf.Abs(deltaY) < 1f)
            {
                AddArrow(retrieveArrow);
                retrieveArrow = null;
            }
            else
            {
                Vector2 deltaDir = new Vector2(deltaX, deltaY);
                deltaDir.Normalize();
                float forceX = deltaDir.x*30f;
                float forceY = deltaDir.y*30f;
                retrieveArrow.rigidbody2D.isKinematic = false;
                retrieveArrow.collider2D.enabled = false;
                retrieveArrow.rigidbody2D.AddForce(new Vector2(forceX, forceY));
            }
        }
    }


}
