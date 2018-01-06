using UnityEngine;
using UnityEngine.UI;

public class Quantization : MonoBehaviour
{
    /// <summary>
    /// The  delay applied to the sound we are playing
    /// </summary>
    public float m_Delay;

    /// <summary>
    /// The toggle on this game Object 
    /// </summary>
    public Toggle m_Toggle;

    /// <summary>
    /// The  play back manager.
    /// </summary>
    public PlaybackManager m_PlayBackManager;

    /// <summary>
    /// Turns  off others quantization objects that are not being used
    /// </summary>
    public void TurnOffOthers()
    {
        if(m_Toggle.isOn)
        {
            foreach(Quantization quantization in m_PlayBackManager.m_Quantization)
            {
                if(quantization != this)
                {
                    quantization.m_Toggle.isOn = false;
                }
            }
        }
    }
}