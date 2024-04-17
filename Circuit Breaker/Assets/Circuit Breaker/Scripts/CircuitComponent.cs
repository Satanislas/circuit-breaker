using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitComponent : MonoBehaviour
{
    public ParticleSystem sparkParticles;

    private Collider collider;
    private int inputSignalStrength;
    private bool isActivated;
    private bool isHovered;

    // Take an input signal strength, do some calculations, and produce and output signal strength
    // This function will be over overridden by each component to do a different action
    public void CalculateSparkOutput(int inputSpark) {
    }

    void OnMouseOver() {
        Debug.Log("Hovering...");
    }

    void Update() {
        if (isActivated) {
            sparkParticles.Play();
        } else {
            sparkParticles.Stop();
        }
    }


}
