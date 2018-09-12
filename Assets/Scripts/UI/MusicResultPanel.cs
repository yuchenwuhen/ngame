using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MusicResultPanel : UIBase {

    [SerializeField]
    private Sprite[] resultimg;
    private Image img_result1;
    private Image img_result2;
    private Image img_result3;

    private Text txt_result;

    private Button btn_successback;
    private Button btn_failrecord;
    private Button btn_failback;

    public override void OnAwake()
    {
        base.OnAwake();
        img_result1 = transform.Find("img_result1").GetComponent<Image>();
        img_result2 = transform.Find("img_result2").GetComponent<Image>();
        img_result3 = transform.Find("img_result3").GetComponent<Image>();
        txt_result = transform.Find("txt_result").GetComponent<Text>();
        btn_successback = transform.Find("btn_successback").GetComponent<Button>();
        btn_successback.onClick.AddListener(BackToScene);
        btn_failrecord = transform.Find("btn_failrecord").GetComponent<Button>();
        btn_failrecord.onClick.AddListener(RecordAgain);
        btn_failback = transform.Find("btn_failback").GetComponent<Button>();
        btn_failback.onClick.AddListener(BackToScene);
    }

    private void BackToScene()
    {
        GameManager.instance.BackToScene();
    }

    private void RecordAgain()
    {
        
    }
}
