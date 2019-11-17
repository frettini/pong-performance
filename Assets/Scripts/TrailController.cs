using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;

public class TrailController : MonoBehaviour
{

    [SerializeField]
    private string trailAddress = "/ball/trail/length";
    private float trailLen;

    private OSCReceiver _receiver;
    private TrailRenderer tr;

    private static float trailLenBall = 0f;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<TrailRenderer>();

        _receiver = GameObject.Find("OSCRx").GetComponent<OSCReceiver>();
        _receiver.Bind(trailAddress, ChangeTrailLen);

        tr.time = trailLenBall;

    }

    float map(float s, float from1, float from2, float to1, float to2)
    {
        return to1 + (s - from1) * (to2 - to1) / (from2 - from1);
    }

    private void ChangeTrailLen(OSCMessage message)
    {
        //float from 0 to 120
        float x = (float)message.Values[0].Value;

        x = map(x, 0f, 1f, 0f, 40f);

        //from 0 to 1
        tr.time = x;
        trailLenBall = x;
        
    }
}
