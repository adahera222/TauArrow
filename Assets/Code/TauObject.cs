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
	public float scale = 1f;
	public float disappearTimer = -1f;
	public float physicsTimer = -1f;
	
	public bool isInit = false;
	public virtual void Awake()
	{
		objSprite = this.GetComponent<SpriteRenderer>();
	}
	public virtual void InitStart()
	{
		
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
    	if (disappearTimer > 0f) 
    	{ 
    		disappearTimer -= deltaTime; 
    		if (disappearTimer <= 0f)
    		{
				TauWorld.Instance.Kill(this);    			
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

    public void FlushPhysics()
    {
    	rigidbody2D.velocity *= 0f;
    	DistanceJoint2D joint = this.gameObject.GetComponent<DistanceJoint2D>();
    	if (joint != null)
    	{
    		joint.anchor = Vector3.zero;
    		joint.enabled = false;
    		joint.connectedBody = null;
    	}
    	rigidbody2D.mass = 0.1f;
    }

    public void OnCollisionEnter2D(Collision2D coll)
    {
    	if (this.gameObject.tag == Globals.ARROW || this.gameObject.tag == Globals.KNIFE)
    	{
    		if (coll != null && coll.gameObject != null)
    		{
		    	if (coll.gameObject.tag == Globals.TERRAIN)
		    	{
		    		SetPhysicsEnabled(false);
		    		if (this.gameObject.tag == Globals.KNIFE)
		    		{
		    			disappearTimer = 2f;		
		    		}
		    	}
		    	if (coll.gameObject.tag == Globals.ACTOR)
		    	{
		    		this.gameObject.collider2D.enabled = false;
		    		this.rigidbody2D.mass = 0f;

		    		DistanceJoint2D joint = this.gameObject.GetComponent<DistanceJoint2D>();
		    		if (joint == null)
		    		{
		    			joint = this.gameObject.AddComponent<DistanceJoint2D>();
		    		}
		    		joint.enabled = true;
		    		joint.connectedBody = coll.gameObject.rigidbody2D;
		    		//joint.anchor = this.gameObject.transform.position - coll.gameObject.transform.position;    		
		    		//joint.anchor *= 0.5f;
		    		//joint.connectedAnchor = -joint.anchor;
		    		Vector3 delta = this.gameObject.transform.position - coll.gameObject.transform.position;
		    		joint.distance = delta.magnitude;


		    		TauActor hitActor = coll.gameObject.GetComponent<TauActor>();
		    		if (hitActor != null)
		    		{
		    			hitActor.TakeDamage();
		    		}
		    	}
		    }
	    }
    }
}
