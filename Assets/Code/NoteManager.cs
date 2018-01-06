using UnityEngine;
using System.Collections.Generic;

public class NoteManager : MonoBehaviour
{
    /// <summary>
    /// This is the gameObject that we use as the parent for the spawned notes
    /// </summary>
    public GameObject m_NoteGrid;

    /// <summary>
    /// This is a running list of actively spawned note objects
    /// </summary>
    public List<Note> m_ActiveNotes;

    /// <summary>
    /// This is the head of the music where the note will be spawned when it is pressed
    /// </summary>
    public RectTransform m_Bar;

    /// <summary>
    /// This places the note on the note grid
    /// </summary>
    /// <param name="key">This is the xylophone key that spawned the note.</param>
    /// <param name="note">This is a reference to the spawned note.</param>
    public void RequestHeightInGrid(XyloKey key, Note note)
    {
        RectTransform rect = note.GetComponent<RectTransform>();

        rect.anchoredPosition= new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y + (key.m_Identifier * 50));
    }
}