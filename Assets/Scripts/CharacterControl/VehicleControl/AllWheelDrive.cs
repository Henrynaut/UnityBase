using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
//using System.Collections.Concurrent.ConcurrentQueue<string>;
using UnityEngine;
using System.IO;
using System.Threading;
using NetMQ;
using NetMQ.Sockets;

public class NetMqListener
{
    private readonly Thread _listenerWorker;

    private bool _listenerCancelled;

    public delegate void MessageDelegate(string message);

    private readonly MessageDelegate _messageDelegate;

    private string incomming_msg = "";

    private void ListenerWork()
    {
        AsyncIO.ForceDotNet.Force();
        using (var subSocket = new SubscriberSocket())
        {
            subSocket.Options.ReceiveHighWatermark = 1000;
            subSocket.Connect("tcp://localhost:12345");
            subSocket.Subscribe("");
            while (!_listenerCancelled)
            {
                string frameString;
                if (!subSocket.TryReceiveFrameString(out frameString)) continue;
                Debug.Log(frameString);
                incomming_msg = frameString;
            }
            subSocket.Close();
        }
        NetMQConfig.Cleanup();
    }

    public void Update()
    {
        while (!(incomming_msg.Length==0))
        {
            string message = incomming_msg;
			
            _messageDelegate(message);
			incomming_msg = "";
        }
    }

    public NetMqListener(MessageDelegate messageDelegate)
    {
        _messageDelegate = messageDelegate;
        _listenerWorker = new Thread(ListenerWork);
    }

    public void Start()
    {
        _listenerCancelled = false;
        _listenerWorker.Start();
    }

    public void Stop()
    {
        _listenerCancelled = true;
        _listenerWorker.Join();
    }
}
public class AllWheelDrive : MonoBehaviour {

	private WheelCollider[] wheels;

	public float maxAngle = 2;
	public float maxTorque = 1;
	public GameObject wheelShape;
    public GameObject boxCollider;
	[SerializeField]
	public string msg = "";
	
	
	private NetMqListener _netMqListener;

    private void HandleMessage(string message)
    {
		  
		msg = message;
		float[] temp_torques = new float[6];
        var splittedStrings = message.Split(',');
        //if (splittedStrings.Length != 3) return;
		if (splittedStrings[0] == "r"){
			
			transform.eulerAngles = new Vector3 (0,0,0);   
			transform.position = new Vector3 (41.12f,2.53f,5.603f);
			
			for(int i = 0;i<6;i++){
			for(int j = 0;j<wheels[i].transform.childCount;i++){
				var wheeler = wheels[i].transform.GetChild (j);
				wheeler.rotation = Quaternion.Euler(0,0,0);
				//wheeler.dynamicFriction = 1;
				//wheeler.material.staticFriction = 1;
			}
			}
			transform.eulerAngles = new Vector3 (0,0,0);   
			transform.position = new Vector3 (41.12f,2.53f,5.603f);
			
			
			//transform.position.y = 2.53;
			//tranform.position.z = 5.603;
			//splittedStrings = new Vector5("0","0","0","0","0");
		}
		int ind = 0;
        foreach(string i in splittedStrings){
			temp_torques[ind] = float.Parse(i);
			ind++;
		}
		for(int i = 0;i<6;i++){
			wheels[i].motorTorque = temp_torques[i];
		}
    }    

	// here we find all the WheelColliders down in the hierarchy
	public void Start()
	{
		wheels = GetComponentsInChildren<WheelCollider>();

		for (int i = 0; i < wheels.Length; ++i) 
		{
			var wheel = wheels [i];

			// create wheel shapes only when needed
			if (wheelShape != null)
			{
				var ws = GameObject.Instantiate (wheelShape);
				ws.transform.parent = wheel.transform;

                var bc = GameObject.Instantiate (boxCollider);
                bc.transform.parent = ws.transform;
			}
			// create box colliders only when needed
			// if (boxCollider != null)
			// {
			// 	var ws = GameObject.Instantiate (boxCollider);
			// 	ws.transform.parent = wheel.transform;
			// }

		}
		_netMqListener = new NetMqListener(HandleMessage);
		_netMqListener.Start();
	}

	// this is a really simple approach to updating wheels
	// here we simulate a rear wheel drive car and assume that the car is perfectly symmetric at local zero
	// this helps us to figure our which wheels are front ones and which are rear
	public void Update()
	{
		  _netMqListener.Update();
		float angle = maxAngle * Input.GetAxis("Horizontal");
		//float torque = maxTorque * Input.GetAxis("Vertical");

		foreach (WheelCollider wheel in wheels)
		{
			// a simple car where front wheels steer and all wheels drive

            
			if (wheel.transform.localPosition.z > 0)
				wheel.steerAngle = angle;

			// Add in functionality for MMSEV 6 wheel control

			// update visual wheels if any
			if (wheelShape) 
			{
				Quaternion q;
				Vector3 p;
				wheel.GetWorldPose (out p, out q);

				// assume that the only child of the wheelcollider is the wheel shape
				Transform shapeTransform = wheel.transform.GetChild (0);
				shapeTransform.position = p;
				shapeTransform.rotation = q;
			}

		}
		
	}
	private void OnDestroy()
    {
        _netMqListener.Stop();
    }
}
