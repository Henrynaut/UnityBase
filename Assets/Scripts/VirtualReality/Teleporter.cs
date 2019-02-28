using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Teleporter : MonoBehaviour {

	public GameObject m_Pointer;
	public SteamVR_Action_Boolean m_TeleportAction;
	public SteamVR_ActionSet m_ActionSet;

	private SteamVR_Behaviour_Pose m_pose = null;
	private bool m_HasPosition = false;

	private void Awake()
	{
		m_pose = GetComponent<SteamVR_Behaviour_Pose>();
	}
	
	// Update is called once per frame
	void Update () {
		//Pointer
		m_HasPosition = UpdatePointer();
		m_Pointer.SetActive(m_HasPosition);


		//Teleport
		if (SteamVR_Actions._default.Teleport.GetStateUp(SteamVR_Input_Sources.Any)){


		//if(m_TeleportAction.GetStateUp(m_pose.inputSource))
			TryTeleport();
		}
	}

	private void TryTeleport()
	{

	}

	private IEnumerator MoveRig(Transform cameraRig, Vector3 translation)
	{
		yield return null;
	}

	private bool UpdatePointer()
	{
		//Ray from the controller
		Ray ray = new Ray(transform.position, transform.forward);
		RaycastHit hit;


		//If it's a hit
		if(Physics.Raycast(ray, out hit))
		{
			m_Pointer.transform.position = hit.point;
			return true;
		}

		//If not a hit
		return false;
	}


}
