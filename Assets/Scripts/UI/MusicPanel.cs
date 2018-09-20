using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPanel : UIBase {
    private PlayMusicManager m_musicManager;

    public override void OnAwake()
    {
        base.OnAwake();
        m_musicManager = this.GetComponent<PlayMusicManager>();
    }

    public override void Appear()
    {
        base.Appear();
        transform.SetAsLastSibling();
        m_musicManager.Init();
    }

    public void Reset()
    {
        m_musicManager.ResetClick();
    }

}
