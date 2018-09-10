using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicNoteCommand : ICommand
{
    private GameObject m_HitObj = null;        //交互对象
    private MusicNote m_musicNote;

    public MusicNoteCommand(GameObject hit)
    {
        m_HitObj = hit;
    }

    public void Execute()
    {
        if (m_HitObj != null)
        {
            m_musicNote = m_HitObj.GetComponent<MusicNote>();

            if (m_musicNote != null)
            {
                m_musicNote.EnterMusicPlay();
            }
        }
    }

    public void Undo()
    {
        
    }
}
