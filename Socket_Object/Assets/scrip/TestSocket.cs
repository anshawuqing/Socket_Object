using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YWSNet;

public class TestSocket : MonoBehaviour {

	// Use this for initialization
	void Start () {
        ClientSocket mSocket = new ClientSocket();
        mSocket.ConnectServer("192.168.1.240", 8088);
        mSocket.SendMessage("服务器傻逼！");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
