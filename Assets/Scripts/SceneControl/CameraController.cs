using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public Camera View;
	private Vector3 mouseOriginPoint;
	private Vector3 offset;
	public float offsetAmount;
	private bool dragging;


	// Use this for initialization
	void Start () {
		View = this.gameObject.GetComponent<Camera>();
		offsetAmount = 0.1f;
        // Debug.Log (View);
	}

	public Ray cameraRaycast(Vector3 position){
		Ray ray = View.ScreenPointToRay(position);
		return ray;
	}
	
	// Update is called once per frame
	void Update () {
		// Debug.Log(Input.GetAxis("Mouse ScrollWheel"));

		// Change size to zoom in/out
		// Mouse wheel scrolling
		// Clamp values to min and max
		View.orthographicSize = Mathf.Clamp(
			View.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") 
			* .2f * View.orthographicSize, 0.5f, 5000f);

		//Change Panning Offsset based on ouse wheel
		offsetAmount = Mathf.Clamp(
			offsetAmount -= Input.GetAxis("Mouse ScrollWheel") 
			* .2f * offsetAmount, 0.01f, 30f);

		Debug.Log(offsetAmount);

		if (Input.GetMouseButton(2))
		{
			offset = (View.ScreenToWorldPoint(Input.mousePosition) - transform.position);
			if (!dragging)
			{
				dragging = true;
				mouseOriginPoint = View.ScreenToWorldPoint(Input.mousePosition);
			}
		}
		else
		{
			dragging = false;
		}
		if (dragging) transform.position = mouseOriginPoint - offset;

		//Arrow key Camera Panning controls
			if (Input.GetKey("up"))
			{
				offset.x = offsetAmount;
				offset.z = offsetAmount;
				transform.position = transform.position + offset;
			}

			if (Input.GetKey("down"))
			{
				offset.x = offsetAmount;
				offset.z = offsetAmount;
				transform.position = transform.position - offset;
			}
			if (Input.GetKey("left"))
			{
				offset.x = -offsetAmount;
				offset.z = offsetAmount;
				transform.position = transform.position + offset;
			}

			if (Input.GetKey("right"))
			{
				offset.x = -offsetAmount;
				offset.z = offsetAmount;
				transform.position = transform.position - offset;
			}
	}
}
