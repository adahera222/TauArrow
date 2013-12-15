using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour 
{
	public static InputManager instance;
	public static InputManager Instance { get { return instance; } }


	public delegate void OnInputKeyDelegate(KeyCode key);
	public delegate void OnInputAxisDelegate(float deltaX, float deltaY);
	public OnInputKeyDelegate HandleKey;
	public OnInputAxisDelegate HandleAxis;

	protected Vector2 mouseVec = new Vector2();
	Vector2[] touchListAnchor;

	public InputManager()
	{
		instance = this;
	}
	

	void Start () 
	{
		HandleKey = DefaultHandleKey;
		HandleAxis = DefaultHandleAxis;
	}
	
	
	void Update () 
	{
		UpdateMouseWorld();
		UpdateKeys();
		MouseDrag();
	}

	public void UpdateMouseWorld()
	{

		Camera cam = Camera.main;
		Vector3 screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane);
		Vector3 worldPoint = cam.ScreenToWorldPoint(screenPoint);
		mouseVec.x = worldPoint.x;
		mouseVec.y = worldPoint.y;
	}

	public void UpdateKeys()
	{
		float deltaX = 0f;
		float deltaY = 0f;
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
		{ 
			deltaX = -1f;
		}
		else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
		{
			deltaX = 1f;
		}
		if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
		{
			deltaY = -1f;
		}
		else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
		{
			deltaY = 1f;
		} 
		HandleAxis(deltaX,deltaY);

		if (Input.GetKey(KeyCode.Space))
		{
			HandleKey(KeyCode.Space);
		} 
	}

	public void UpdateSwipe()
	{
#if UNITY_ANDROID
		int i = 0;
		while (i < Input.touchCount && i < 4) 
		{
			Touch touch = Input.GetTouch(i);
            if (touch.phase == TouchPhase.Began)
            {
            	touchListAnchor[i] = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
            	bool isDpad = touch.position.x < Screen.width*0.3f;

            	//bool isDpad = Math.Abs(diff.x) > Math.Abs(diff.y);
            	
            	if (isDpad)
            	{
            		Vector2 diff = touch.position - dpadPos;
	            	HandleAxis(Mathf.Clamp(diff.x/30f, -2f, 2f),0);
	            }
	            else
	            {
	            	//Vector2 diff = touch.position - touchListAnchor[i];
	            	Vector2 diff = touch.position - waterPos;
	            	HandleAxis(0, Mathf.Clamp(diff.y/30f, -2f, 2f));
	            }
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
            	touchListAnchor[i] = Vector2.zero;
            }
            ++i;
        }
#endif  
	}

	public void AddInput(OnInputAxisDelegate del)
	{
		HandleAxis += del;
	}
	public void AddInput(OnInputKeyDelegate del)
	{
		HandleKey += del;
	}

	public void RemoveInput(OnInputAxisDelegate del)
	{
		HandleAxis -= del;
	}
	public void RemoveInput(OnInputKeyDelegate del)
	{
		HandleKey -= del;
	}

	void DefaultHandleKey(KeyCode key)
	{
		//Debug.Log("key "+key);
	}

	void DefaultHandleAxis(float x, float y)
	{
		if (Mathf.Abs(x) > 0.1 || Mathf.Abs(y) > 0.1)
		{
			//Debug.Log(x+" "+y);
		}
	}

	public void MouseDrag()
	{
		
	}
}
