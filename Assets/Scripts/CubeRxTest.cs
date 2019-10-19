using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;

public class CubeRxTest : MonoBehaviour
{

    string Address;

    private float test;

    [Header("OSC Settings")]
    public OSCReceiver Receiver;


    // Start is called before the first frame update
    void Start()
    {

        if (gameObject.name == "CubeR1")
        {
            Address = "/cube/t1";
        }
        else
        {
            Address = "/cube/t2";
        }

        Receiver.Bind(Address, ReceivedMessage);
    }

    private void ReceivedMessage(OSCMessage message)
    {
        float x = (float)message.Values[0].Value;
        float y = (float)message.Values[1].Value;

        float move = y - transform.position.y;

        transform.Translate(move * Vector2.up * 0.01f);
    }

}
