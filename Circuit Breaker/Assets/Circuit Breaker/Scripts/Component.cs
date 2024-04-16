using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Component
{
    public ParticleSystem sparkParticles;

    private int inputSignalStrength;
    private bool isActivated;

    // Take an input signal strength, do some calculations, and produce and output signal strength
    // This function will be over overridden by each component to do a different action
    public int CalculateOutput() {
        return inputSignalStrength;
    }

    public void setInputSignal(int inputSignalStrength) { this.inputSignalStrength = inputSignalStrength; }

    void Update() {
        if (isActivated) {
            sparkParticles.Play();
        } else {
            sparkParticles.Stop();
        }
    }
}
