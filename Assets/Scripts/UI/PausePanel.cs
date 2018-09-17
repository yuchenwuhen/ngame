using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : UIBase {

    private Button m_btnContinue;
    private Button m_btnRecord;
    private Button m_btnExit;

    private MusicManager m_musicManager;

    public override void OnAwake()
    {
        base.OnAwake();
        m_btnContinue = transform.Find("btnContinue").GetComponent<Button>();
        m_btnRecord = transform.Find("btnRecord").GetComponent<Button>();
        m_btnExit = transform.Find("btnExit").GetComponent<Button>();
        m_btnContinue.onClick.AddListener(Continue);
        m_btnRecord.onClick.AddListener(Record);
        m_btnExit.onClick.AddListener(Exit);
    }

    public override void Init(object[] parameters)
    {
        base.Init(parameters);
        m_musicManager = parameters[0] as MusicManager;
    }

    public void Continue()
    {
        m_musicManager.Continue();
    }

    public void Record()
    {
        m_musicManager.Record();
    }

    public void Exit()
    {
        m_musicManager.Exit();
    }
}
