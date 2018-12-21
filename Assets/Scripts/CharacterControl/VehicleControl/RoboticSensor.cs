﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Threading;
using NetMQ;
using NetMQ.Sockets;

public class NetMqPublisher
{
    private readonly Thread _listenerWorker;

    private bool _listenerCancelled;

    public delegate string MessageDelegate(string message);

    private readonly MessageDelegate _messageDelegate;

    private readonly Stopwatch _contactWatch;

    private const long ContactThreshold = 1000;

    public bool Connected;
	private void RestartWatch(Stopwatch _contactWatch){
			 _contactWatch.Stop();
			  _contactWatch.Start();
	}
    private void ListenerWork()
    {
        AsyncIO.ForceDotNet.Force();
        using (var server = new ResponseSocket())
        {
            server.Bind("tcp://*:12346");

            while (!_listenerCancelled)
            {
                Connected = _contactWatch.ElapsedMilliseconds < ContactThreshold;
                string message;
                if (!server.TryReceiveFrameString(out message)) continue;
					RestartWatch(_contactWatch);

                var response = _messageDelegate(message);
                server.SendFrame(response);
            }
        }
        NetMQConfig.Cleanup();
    }

    public NetMqPublisher(MessageDelegate messageDelegate)
    {
        _messageDelegate = messageDelegate;
        _contactWatch = new Stopwatch();
        _contactWatch.Start();
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

public class RoboticSensor : MonoBehaviour {
public bool Connected;
private NetMqPublisher _netMqPublisher;
private string _response;

[SerializeField]
public Vector3 position;
[SerializeField]
public Vector3 rotation;

	// Use this for initialization
	void Start () {
		 _netMqPublisher = new NetMqPublisher(HandleMessage);
        _netMqPublisher.Start();
	}
	
	// Update is called once per frame
	void Update () {
		position = this.transform.position;
		rotation = (this.transform.rotation).eulerAngles;
		 
        _response = rotation.x + ","+ rotation.y + "," + rotation.z;
        Connected = _netMqPublisher.Connected;
	}

	private string HandleMessage(string message)
    {
        // Not on main thread
        return _response;
    }

	private void OnDestroy()
    {
        _netMqPublisher.Stop();
    }
}
