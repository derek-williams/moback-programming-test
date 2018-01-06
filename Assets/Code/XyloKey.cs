using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections.Generic;

public class XyloKey : MonoBehaviour
{
    /// <summary>
    /// The sprite that will represent the note of the xylophone key pressed in the note grid
    /// </summary>
    public Sprite m_Note;

    /// <summary>
    /// The grid that the note will be added to
    /// </summary>
    public NoteManager m_NoteLayoutGroup;

    /// <summary>
    /// The audio source on our key
    /// </summary>
    public AudioSource m_Source;

    /// <summary>
    /// The  note reference to the prefab in the project
    /// </summary>
    public Note m_NoteRef;

    /// <summary>
    /// The animation time playback to be set in our animation event
    /// </summary>
    public float m_AnimationTimePlayback;

    /// <summary>
    /// The  bar animator we used to assign animation events to
    /// </summary>
    public Animator m_BarAnimator;

    /// <summary>
    /// The play back manager.
    /// </summary>
    public PlaybackManager m_PlayBackManager;

    /// <summary>
    /// The dentifier for our notes, we use this in multiple places
    /// </summary>
    public int m_Identifier;

    /// <summary>
    /// Sets the note attributes.
    /// </summary>
    /// <param name="note">Note.</param>
    public void SetNoteAttributes(Note note)
    {
        note.m_TimeSpawned = Time.time +.5f;
        note.m_NoteSprite.overrideSprite = m_Note;
        note.m_NoteManager = m_NoteLayoutGroup;

        if (!m_NoteLayoutGroup.m_ActiveNotes.Contains(note))
        {
            m_NoteLayoutGroup.m_ActiveNotes.Add(note);
        }
        note.RequestPlacementInNodeGrid(this);
    }

    /// <summary>
    /// Assigns the animation time.
    /// </summary>
    public void AssignAnimationTime()
    {
        m_PlayBackManager.RequestRecord();

        if (!m_NoteLayoutGroup.m_ActiveNotes.Any(x => Mathf.Approximately(x.m_TimeSpawned, Time.time)))
        {
            Note spawnedNote = Instantiate(m_NoteRef);
            m_NoteLayoutGroup.m_ActiveNotes.Add(spawnedNote);
            SetNoteAttributes(spawnedNote);
        }

        if (m_PlayBackManager.m_IsRecording)
        {
            m_AnimationTimePlayback = Time.time - m_PlayBackManager.m_TimeStarted;
            Debug.LogFormat("{0} is the normalized time of the current animation", m_BarAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);

            AnimationEvent evt;
            evt = new AnimationEvent();

            evt.time = m_AnimationTimePlayback;
            evt.functionName = "Playback";
            evt.intParameter = m_Identifier;

            m_BarAnimator.runtimeAnimatorController.animationClips[0].AddEvent(evt);
        }
    }
}