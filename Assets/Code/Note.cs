using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Note : MonoBehaviour
{
    /// <summary>
    /// The time at which the note was spawned spawned.
    /// Ensures no duplicates at the same time
    /// </summary>
    public float m_TimeSpawned;

    /// <summary>
    /// The note sprite we are going to override the prefabs sprite with
    /// </summary>
    public Image m_NoteSprite;

    /// <summary>
    /// The note manager.
    /// </summary>
    public NoteManager m_NoteManager;

    /// <summary>
    /// Requests the placement in node grid.
    /// </summary>
    public void RequestPlacementInNodeGrid(XyloKey key)
    {
        RectTransform rect = GetComponent<RectTransform>();
        rect.SetParent( m_NoteManager.m_NoteGrid.GetComponent<RectTransform>(), false);
        rect.anchoredPosition = m_NoteManager.m_Bar.anchoredPosition;
        m_NoteManager.RequestHeightInGrid(key, this);
    }
}