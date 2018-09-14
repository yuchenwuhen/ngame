using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCommand : ICommand
{

    private GameObject m_HitObj = null;        //交互对象
    private DialogueNote m_musicNote;

    public DialogueCommand(GameObject hit)
    {
        m_HitObj = hit;
    }

    public void Execute()
    {
        if (m_HitObj != null)
        {
            m_musicNote = m_HitObj.GetComponent<DialogueNote>();

            if (m_musicNote != null)
            {
                m_musicNote.EnterDialoguePlay();
            }
        }
    }

    public void Undo()
    {

    }
}
