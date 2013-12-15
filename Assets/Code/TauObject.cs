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

public class TauObject : MonoBehaviour
{
	//public Rigidbody2D rigidbody;
	//public BoxCollider2D collider;
	public SpriteRenderer objSprite;
	public float physicsTimer = -1f;
	
	public bool isInit = false;
	public virtual void Awake()
	{
		objSprite = this.GetComponent<SpriteRenderer>();
	}
	public virtual void InitStart()
	{
		InitFinish();
	}
	public virtual void InitFinish()
	{
		isInit = true;
	}

	public virtual void SetActive(bool val)
	{

	}
	
    public virtual void Update()
    {
    	float deltaTime = Time.deltaTime;
    	if (physicsTimer > 0f) 
    	{ 
    		physicsTimer -= deltaTime; 
    		if (physicsTimer <= 0f)
    		{
				SetPhysicsEnabled(true);    			
    		}
    	}
    	if (collider2D.enabled && !rigidbody2D.isKinematic)
    	{
    		if (Mathf.Abs(rigidbody2D.velocity.x) > 0 || Mathf.Abs(rigidbody2D.velocity.y) > 0)
    		{
    			Vector3 velocityAngles = Vector3.zero;
	    		velocityAngles.z -= 90f+Mathf.Atan2(rigidbody2D.velocity.x, rigidbody2D.velocity.y) * Mathf.Rad2Deg; 
	    		if (rigidbody2D.velocity.x > 0f)
	    		{
	    			velocityAngles.z += 180f;
	    		}
				this.transform.localEulerAngles = velocityAngles;	    		
	    	}	    		 
    	}
    }

    public void SetPhysicsEnabled(bool val)
    {
    	rigidbody2D.isKinematic = !val;
    	collider2D.enabled = val;
    }

    public void DelayedPhysics(float timer)
    {
    	physicsTimer = timer;
    }
}
