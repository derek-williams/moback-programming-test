using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PlaybackManager : MonoBehaviour 
{
    /// <summary>
    /// The stop button.
    /// </summary>
    public GameObject m_StopButton;

    /// <summary>
    /// The keys to play.
    /// </summary>
    public List<XyloKey> m_KeysToPlay;

    /// <summary>
    /// The length of the song.
    /// </summary>
    public static float SONG_LENGTH = 5f;

    /// <summary>
    /// The  bar animator.
    /// </summary>
    public Animator m_BarAnimator;

    /// <summary>
    /// The began recording bool
    /// </summary>
    public bool m_BeganRecording;

    /// <summary>
    /// The  is recording bool
    /// </summary>
    public bool m_IsRecording;

    /// <summary>
    /// The time we started
    /// </summary>
    public float m_TimeStarted;

    /// <summary>
    /// The echo toggle.
    /// </summary>
    public Toggle m_EchoToggle;

    /// <summary>
    /// The echo audio group.
    /// </summary>
    public AudioMixerGroup echoGroup;

    /// <summary>
    /// The main audio group.
    /// </summary>
    public AudioMixerGroup mainGroup;

    /// <summary>
    /// The reverse toggle.
    /// </summary>
    public Toggle m_ReverseToggle;

    /// <summary>
    /// The  quantization objects
    /// </summary>
    public List<Quantization> m_Quantization;

    /// <summary>
    /// Requests to record and sets the state of the animator
    /// </summary>
    public void RequestRecord()
    {
        if(!m_BeganRecording)
        {
            foreach(Note note in m_KeysToPlay[0].m_NoteLayoutGroup.m_ActiveNotes)
            {
                Destroy(note.gameObject);
            }
            m_KeysToPlay[0].m_NoteLayoutGroup.m_ActiveNotes.Clear();

            m_IsRecording = true;
            m_BarAnimator.Play("StartToEnd");
            m_BarAnimator.SetBool("Stop", true);
            CleanAnimationEvents();
            m_TimeStarted = Time.time;
            m_BeganRecording = true;
        }
    }

    /// <summary>
    /// Sets the echo.
    /// </summary>
    public void SetEcho()
    {
        foreach(XyloKey key in m_KeysToPlay)
        {
            if (m_EchoToggle.isOn)
            {
                key.m_Source.outputAudioMixerGroup = echoGroup;
            }
            else
            {
                key.m_Source.outputAudioMixerGroup = mainGroup;
            }
        }
    }

    /// <summary>
    /// Sets the reverse.
    /// </summary>
    public void SetReverse()
    {
        if (m_ReverseToggle.isOn && m_StopButton.activeSelf)
        {
            m_BarAnimator.SetFloat("Reverse", -1);
        }
        else
        {
            m_BarAnimator.SetFloat("Reverse", 1);
        }
    }

    /// <summary>
    /// Update this instance.
    /// </summary>
    public void Update()
    {
        if(m_BeganRecording)
        {
            //need better heuristic for figuring out what animation i'm in
            if(Time.time < m_TimeStarted + SONG_LENGTH)
            {
                m_IsRecording = true;
            }
            else
            {
                ResetRecord();
            }
        }
    }

    /// <summary>
    /// Resets the active notes
    /// </summary>
    public void ResetKey()
    {
        for (int i = 0; i < m_KeysToPlay[0].m_NoteLayoutGroup.m_ActiveNotes.ToArray().Length; i++)
        {
            if (m_KeysToPlay[0].m_NoteLayoutGroup.m_ActiveNotes[i] != null)
            {
                Destroy(m_KeysToPlay[0].m_NoteLayoutGroup.m_ActiveNotes[i].gameObject);
            }
        }

        m_KeysToPlay[0].m_NoteLayoutGroup.m_ActiveNotes.Clear();
    }

    /// <summary>
    /// Resets the recording state
    /// </summary>
    public void ResetRecord()
    {
        m_BeganRecording = false;
        m_IsRecording = false;
    }

/// <summary>
///  Playback of the recorded keys
/// </summary>
/// <returns></returns>
/// <param name="index"></param>
    public void Playback(int index)
    {
        float delay = 0f;
        if(m_Quantization.Any(x => x.m_Toggle.isOn))
        {
            delay = m_Quantization.First(x => x.m_Toggle.isOn).m_Delay;
        }

        ReturnXyloKeyByIndex(index).m_Source.PlayDelayed(delay);
    }

    /// <summary>
    /// Returns the xylo key by index using the identifier
    /// </summary>
    /// <returns>The xylo key by index.</returns>
    /// <param name="index">m_Identifier.</param>
    public XyloKey ReturnXyloKeyByIndex(int index)
    {
        return m_KeysToPlay.First(x => x.m_Identifier == index);
    }

    /// <summary>
    /// Playback tells the UI how to play back the animation.
    /// </summary>
    /// <param name="stop">If set to <c>true</c> sets animator bool stop.</param>
    public void Playback(bool stop)
    {
        if(stop)
        {
            m_BarAnimator.SetBool("Stop", true);
        }
        else
        {
            m_BarAnimator.SetBool("Stop", false);
        }
    }

    /// <summary>
    /// Cleans the animation events.
    /// </summary>
    public void CleanAnimationEvents()
    {
        if (m_BarAnimator.runtimeAnimatorController.animationClips[0].events.Length > 0)
        {
            List<AnimationEvent> empyEvents = new List<AnimationEvent>();
            m_BarAnimator.runtimeAnimatorController.animationClips[0].events = empyEvents.ToArray();
        }
    }

    /// <summary>
    /// Stop the animator
    /// </summary>
    public void Stop()
    {
        m_BarAnimator.Play("Default");
    }
}