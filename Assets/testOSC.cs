using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;

public class testOSC : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Creating a receiver.
        var receiver = gameObject.AddComponent<OSCReceiver>();

        // Set local port.
        receiver.LocalPort = 8000;

        receiver.Bind("/cubereceive", MessageReceived);
    }
    

    protected void MessageReceived(OSCMessage message)
    {
        // Any code...
        Debug.Log(message);
    }
}
