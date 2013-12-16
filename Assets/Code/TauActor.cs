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
    public SpriteFace mainSprite;
    public SpriteFace painSprite;
}


public class TauActor : TauObject
{
    public ActorData aData;
	public TauController controller;
    public TauBody body;
    public TauAI ai;
	public bool isHuman = false;
    public bool isAlive = false;
    public int HP = 10;
    public int weapon = 0;

	
	public TauObject currentWeapon;
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
		base.InitStart();
	}
	public override void InitFinish()
	{
        HP = aData.HP;
        isAlive = true;
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
        CheckFallBottom();
    }

    public void CheckFallBottom()
    {
        if (controller.posY < -5)
        {
            TauWorld.Instance.Kill(this);
        }
    }

    public void AddWeapon(TauObject obj)
    {
    	obj.SetPhysicsEnabled(false);
    	currentWeapon = obj;
    	body.CheckFacing();
    	CheckWeapon();
    }

    public void CheckWeapon()
    {

    	if (currentWeapon != null)
    	{
    		currentWeapon.transform.localScale = currentWeapon.scale*body.bodySprite.gameObject.transform.localScale;

    		Vector3 arrowPos = this.transform.position;
    		Vector3 arrowAngles = Vector3.zero;
    		float arrowScaleX = currentWeapon.transform.localScale.x;
    		switch (body.currentAction)
    		{
    			case SpriteAction.AIMDOWN:
    			{
    				arrowPos.x += arrowScaleX*Globals.ARROW_POS_DOWN.x;
    				arrowPos.y += Globals.ARROW_POS_DOWN.y;
    				arrowAngles.z += arrowScaleX*Globals.ARROW_ROT_DOWN;
    				currentWeapon.objSprite.enabled = true;
    				break;
    			}
    			case SpriteAction.AIMUP:
    			{
    				arrowPos.x += arrowScaleX*Globals.ARROW_POS_UP.x;
    				arrowPos.y += Globals.ARROW_POS_UP.y;
    				arrowAngles.z += arrowScaleX*Globals.ARROW_ROT_UP;
    				currentWeapon.objSprite.enabled = true;
    				break;
    			}
    			case SpriteAction.AIMSTRAIGHT:
    			{
    				arrowPos.x += arrowScaleX*Globals.ARROW_POS_STRAIGHT.x;
    				arrowPos.y += Globals.ARROW_POS_STRAIGHT.y;
    				arrowAngles.z += arrowScaleX*Globals.ARROW_ROT_STRAIGHT;
    				currentWeapon.objSprite.enabled = true;
    				break;
    			}
    			default:
    			{
    				currentWeapon.objSprite.enabled = false;
    				break;
    			}
    		}
    		currentWeapon.transform.position = arrowPos;
    		currentWeapon.transform.localEulerAngles = arrowAngles;
    	}
    }

    public void ShootArrow(bool hasCharged)
    {
    	if (currentWeapon != null)
    	{
    		float arrowScaleX = currentWeapon.transform.localScale.x;
	    	float launchZ = arrowScaleX * currentWeapon.transform.localEulerAngles.z * Mathf.Deg2Rad;
	    	float launchStrength = hasCharged ? 70f : 40f;
	    	float forceX = arrowScaleX * launchStrength * Mathf.Cos(launchZ);
	    	float forceY = launchStrength * Mathf.Sin(launchZ);
	    	currentWeapon.rigidbody2D.isKinematic = false;
	    	currentWeapon.rigidbody2D.AddForce(new Vector2(forceX,forceY));
			currentWeapon.DelayedPhysics(0.2f);
	    	currentWeapon = null;
	    }
    }

    public void RetrieveArrow()
    {
        if (retrieveArrow == null)
        {
            retrieveArrow = TauWorld.Instance.FindArrow(this);
            if (retrieveArrow != null)
            {
                retrieveArrow.FlushPhysics();
            }
        }

    }
    public void CheckRetrieveArrow()
    {
        if (retrieveArrow != null)
        {
            float arrowPosX = retrieveArrow.gameObject.transform.position.x;
            float arrowPosY = retrieveArrow.gameObject.transform.position.y;
            float deltaX = controller.posX - arrowPosX;
            float deltaY = controller.posY - arrowPosY;
            bool isClose = Mathf.Abs(deltaX) < 1f && Mathf.Abs(deltaY) < 1f;
            bool isOutside = arrowPosX < -10f || arrowPosX > 10f || arrowPosY < -10f || arrowPosY > 10f;
            if (isClose || isOutside)
            {
                AddWeapon(retrieveArrow);
                retrieveArrow = null;
            }
            else
            {
                Vector2 deltaDir = new Vector2(deltaX, deltaY);
                deltaDir.Normalize();
                float forceX = deltaDir.x*22f;
                float forceY = deltaDir.y*22f;
                retrieveArrow.rigidbody2D.isKinematic = false;
                retrieveArrow.collider2D.enabled = false;
                retrieveArrow.rigidbody2D.AddForce(new Vector2(forceX, forceY));
            }
        }
    }

    public void TakeDamage()
    {
        HP -= 10;
        controller.TakeDamage();
        if (HP <= 0)
        {
            TauWorld.Instance.Kill(this);
        }
    }


}
