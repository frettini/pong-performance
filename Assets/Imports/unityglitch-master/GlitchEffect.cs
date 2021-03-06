/**
This work is licensed under a Creative Commons Attribution 3.0 Unported License.
http://creativecommons.org/licenses/by/3.0/deed.en_GB

You are free:

to copy, distribute, display, and perform the work
to make derivative works
to make commercial use of the work
*/

using UnityEngine;
using extOSC;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/GlitchEffect")]
[RequireComponent(typeof(Camera))]
public class GlitchEffect : MonoBehaviour
{
    #region public variables

    public Texture2D displacementMap;
	public Shader Shader;
	[Header("Glitch Intensity")]

	[Range(0, 1)]
	public float intensity;

	[Range(0, 1)]
	public float flipIntensity;

	[Range(0, 1)]
	public float colorIntensity;

    #endregion

    #region private variables

    private float _glitchup;
	private float _glitchdown;
	private float flicker;
	private float _glitchupTime = 0.05f;
	private float _glitchdownTime = 0.05f;
	private float _flickerTime = 0.5f;
	private Material _material;

    private OSCReceiver _receiver;
    private OSCTransmitter _transmitterRight, _transmitterLeft;
    [SerializeField]
    private string glitchBang = "/glitch/bang";

    [SerializeField]
    private string glitchIntensity = "/glitch/intensity";
    [SerializeField]
    private string glitchFlip = "/glitch/Flip";
    [SerializeField]
    private string glitchColor = "/glitch/Color";

    #endregion

    void Start()
	{
		_material = new Material(Shader);

        _receiver = GameObject.Find("OSCRx").GetComponent<OSCReceiver>();
        _transmitterLeft = GameObject.Find("OSCTxLeft").GetComponent<OSCTransmitter>();
        _transmitterRight = GameObject.Find("OSCTxRight").GetComponent<OSCTransmitter>();

        _receiver.Bind(glitchIntensity, ReceiveGlitch);
        _receiver.Bind(glitchFlip, ReceiveGlitch);
        _receiver.Bind(glitchColor, ReceiveGlitch);
    }

	// Called by camera to apply image effect
	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		_material.SetFloat("_Intensity", intensity);
		_material.SetFloat("_ColorIntensity", colorIntensity);
		_material.SetTexture("_DispTex", displacementMap);

		flicker += Time.deltaTime * colorIntensity;
		if (flicker > _flickerTime)
		{
			_material.SetFloat("filterRadius", Random.Range(-3f, 3f) * colorIntensity);
			_material.SetVector("direction", Quaternion.AngleAxis(Random.Range(0, 360) * colorIntensity, Vector3.forward) * Vector4.one);
			flicker = 0;
			_flickerTime = Random.value;
            
        }

		if (colorIntensity == 0)
			_material.SetFloat("filterRadius", 0);

		_glitchup += Time.deltaTime * flipIntensity;
		if (_glitchup > _glitchupTime)
		{
			if (Random.value < 0.1f * flipIntensity)
				_material.SetFloat("flip_up", Random.Range(0, 1f) * flipIntensity);
			else
				_material.SetFloat("flip_up", 0);

			_glitchup = 0;
			_glitchupTime = Random.value / 10f;
		}

		if (flipIntensity == 0)
			_material.SetFloat("flip_up", 0);

		_glitchdown += Time.deltaTime * flipIntensity;
		if (_glitchdown > _glitchdownTime)
		{
			if (Random.value < 0.1f * flipIntensity)
				_material.SetFloat("flip_down", 1 - Random.Range(0, 1f) * flipIntensity);
			else
				_material.SetFloat("flip_down", 1);

			_glitchdown = 0;
			_glitchdownTime = Random.value / 10f;
            
        }

		if (flipIntensity == 0)
			_material.SetFloat("flip_down", 1);

		if (Random.value < 0.05 * intensity)
		{
			_material.SetFloat("displace", Random.value * intensity);
			_material.SetFloat("scale", 1 - Random.value * intensity);
            SendBang(_transmitterRight, glitchBang);
            SendBang(_transmitterLeft, glitchBang);

        }
        else
			_material.SetFloat("displace", 0);

		Graphics.Blit(source, destination, _material);
	}

    private void ReceiveGlitch(OSCMessage message)
    {
        float x = (float)message.Values[0].Value;
        string address = message.Address;

        if (address.Contains("intensity"))
        {

            intensity = x;
        }
        else if (address.Contains("color"))
        {
            colorIntensity = x;
        }
        else if (address.Contains("flip"))
        {
            flipIntensity = x;
        }
    }

    private void SendBang(OSCTransmitter _transmitter, string address)
    {

        //Send OSC message
        var message = new OSCMessage(string.Format("{0}", address));
        // Populate values.
        message.AddValue(OSCValue.Impulse());
        _transmitter.Send(message);

    }
}
