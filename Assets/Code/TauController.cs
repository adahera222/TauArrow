using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TauController : MonoBehaviour
{
	private TauActor actor;
	public bool isInit = false;
	public bool isOnFloor = false;
	public HashSet<GameObject> contactFloors = new HashSet<GameObject>();

	public float timeAlive = -1f;
	public float timeJumping = -1f;
	public float timeLanding = -1f;
	public bool timeDirty = false;

	public virtual void InitStart(TauActor p_actor)
	{
		actor = p_actor;
		if (actor.isHuman)
		{
			InputManager.Instance.AddInput(HandleAxis);
		}
	}
	public virtual void InitFinish()
	{
		isInit = true;
	}
	
    public virtual void Update()
    {
    	timeAlive += Time.deltaTime;
    	rigidbody2D.angularVelocity *= 0f;
    }

    public void HandleAxis(float x, float y)
    {
    	if (!isOnFloor)
    	{
    		x *= 0.1f; // low air control
    		y = Mathf.Min(y, 0f); // no jump 
    	}
    	if (Mathf.Abs(x) > 0 || Mathf.Abs(y) > 0)
    	{
    		rigidbody2D.AddForce(new Vector2(x*10f,y*100f));
    		if (y > 0.01f)
    		{
    			timeJumping = timeAlive;
    			timeDirty = true;
    		}
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
    		//if (coll.gameObject.transform.position.y < this.gameObject.transform.position.y)
    		{
    			isOnFloor = true;
    			if (timeAlive > timeJumping+0.01f)
    			{
    				timeLanding = timeAlive;
    				timeDirty = true;
    			}
    		}
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
	    	if (contactFloors.Count == 0)
	    	{
	    		isOnFloor = false;
	    	}
	    }
    }
}
