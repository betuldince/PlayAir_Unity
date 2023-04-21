using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SynthControl))]

public class TouchKey2 : MonoBehaviour
{

	public Camera gameCam;
	[Range(0.003f, 3.0f)] public float env_atk = 1.0f;
	[Range(0.003f, 3.0f)] public float env_rel = 1.0f;
	[Range(24, 48)] public int scale = 24;
	public LayerMask touchInputMask;
	private GameObject[] touchesOld;
	private int lastNoteDegree = -1;
	private List<GameObject> touchList = new List<GameObject>();
	private Lope envelope;
	private RaycastHit2D hit;
	private string lastNoteDegreeString;
	private SynthControl synth;
	private GameObject recipient;
	public NReverb reverb;

	public void Start()
	{
		synth = GetComponent<SynthControl>();
		envelope = GetComponent<Lope>();

		envelope.sustain = false;
		reverb = GetComponent<NReverb>();


		// barTexture = new Texture2D(8, 8);
		//    noise = FindObjectOfType(NoiseController);
		//    seq = GetComponent.<AutoSequencer>();
		//  SYNTH.LOPE.SUSTAIN = false;
	}

	public void Update()
	{



		touchesOld = new GameObject[touchList.Count];
		touchList.CopyTo(touchesOld);
		touchList.Clear();

		RaycastHit2D hit = Physics2D.Raycast(
											 (BodySourceView2.Instance.targetPosition1), Vector2.zero);
		GameObject recipient = hit.transform.gameObject;
		var tDetails = recipient.GetComponent<TileDetails>();
		reverb.wetMix = hit.transform.position.x / 24;
		Debug.Log(recipient.gameObject.name + "hit");
		if (hit.collider != null)
		{


			touchList.Add(recipient);


			lastNoteDegree = scale + (int)tDetails.note;
			envelope = synth.KeyOn(lastNoteDegree, envelope);
			tDetails.OnTouchStay();


		}

		foreach (GameObject g in touchesOld)
		{
			if (!touchList.Contains(g))
			{
				g.SendMessage("OnTouchExit", hit.point,
							  SendMessageOptions.DontRequireReceiver);
			}
		}


		envelope.attack = env_atk;
		envelope.release = env_rel;







	}

}
