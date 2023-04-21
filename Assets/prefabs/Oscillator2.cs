using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator2 : MonoBehaviour {
    // Start is called before the first frame update
    public double frequency = 540.0;
    private double increment;
    private double phase;
    private double sampFreq = 48000.0;

    public double gain;

    //filtre, emvolope, LFO

    public static Oscillator2 Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }


    private void OnAudioFilterRead(float[] data, int channels)
    {
        increment = frequency * 2.0 * Mathf.PI / sampFreq;
        for (int i = 0; i < data.Length; i += channels)
        {
            phase += increment;
            data[i] = (float)(gain * Mathf.Sin((float)phase)); // For sine wave


            // * Square Wave
            /*
            if(gain * Mathf.Sin((float)phase) >= 0 * gain)
            {
                data[i] = (float)gain * 0.6f;
            }
            else
            {
                data[i] = (-(float)gain) * 0.6f;
            }
            

            data[i] = (float)(gain * (double)Mathf.PingPong((float)phase, 1.0f));

            if (channels == 2)
            {
                data[i + 1] = data[i];
            }
            if (phase > (Mathf.PI * 2))
            {
                phase = 0.0;
            }*/
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
