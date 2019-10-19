using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;

public class CubeTxTest : MonoBehaviour
{
    private string Address;

    public OSCTransmitter _transmitter;

    private Vector3 oldpos;

    // Start is called before the first frame update
    void Start()
    {
        if(gameObject.name == "CubeT1")
        {
            //_transmitter = GameObject.Find("OSCManagerLeft").GetComponent<OSCTransmitter>();
            Address = "/cube/t1";
        }
        else
        {
            //_transmitter = GameObject.Find("OSCManageRight").GetComponent<OSCTransmitter>();
            Address = "/cube/t2";
        }
      
      
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position != oldpos)
        {
            var message = new OSCMessage(Address);
            message.AddValue(OSCValue.Float(transform.position.x));
            message.AddValue(OSCValue.Float(transform.position.y));

            _transmitter.Send(message);
        }

        oldpos = transform.position;
    }
}
