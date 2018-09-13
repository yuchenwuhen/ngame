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
    private Image img_resultfail1;
    private Image img_resultfail2;
    private Image img_resultfail3;

    private Image img_result;

    private Button btn_successback;
    private Button btn_failrecord;
    private Button btn_failback;

    public override void OnAwake()
    {
        base.OnAwake();
        img_result1 = transform.Find("img_result1").GetComponent<Image>();
        img_result2 = transform.Find("img_result2").GetComponent<Image>();
        img_result3 = transform.Find("img_result3").GetComponent<Image>();
        img_resultfail1 = transform.Find("img_resultfail1").GetComponent<Image>();
        img_resultfail2 = transform.Find("img_resultfail2").GetComponent<Image>();
        img_resultfail3 = transform.Find("img_resultfail3").GetComponent<Image>();
        img_result = transform.Find("img_result").GetComponent<Image>();
        btn_successback = transform.Find("btn_successback").GetComponent<Button>();
        btn_successback.onClick.AddListener(BackToScene);
        btn_failrecord = transform.Find("btn_failrecord").GetComponent<Button>();
        btn_failrecord.onClick.AddListener(RecordAgain);
        btn_failback = transform.Find("btn_failback").GetComponent<Button>();
        btn_failback.onClick.AddListener(BackToScene);
    }

    public override void Init(object[] parameters)
    {
        base.Init(parameters);
        int index = (int)parameters[0];
        SetResult(index);
    }

    public void SetResult(int index)
    {
        if(index == 0)
        {
            SetFail();
        }else if(index == 1)
        {
            img_result1.gameObject.SetActive(true);
            img_result2.gameObject.SetActive(false);
            img_result3.gameObject.SetActive(false);
            img_resultfail1.gameObject.SetActive(false);
            img_resultfail2.gameObject.SetActive(true);
            img_resultfail3.gameObject.SetActive(true);
            SetSuccess();
        }else if(index == 2)
        {
            img_result1.gameObject.SetActive(true);
            img_result2.gameObject.SetActive(true);
            img_result3.gameObject.SetActive(false);
            img_resultfail1.gameObject.SetActive(false);
            img_resultfail2.gameObject.SetActive(false);
            img_resultfail3.gameObject.SetActive(true);
            SetSuccess();
        }else if(index == 3)
        {
            img_result1.gameObject.SetActive(true);
            img_result2.gameObject.SetActive(true);
            img_result3.gameObject.SetActive(true);
            img_resultfail1.gameObject.SetActive(false);
            img_resultfail2.gameObject.SetActive(false);
            img_resultfail3.gameObject.SetActive(false);
            SetSuccess();
        }
        
    }

    private void SetFail()
    {
        img_result1.gameObject.SetActive(false);
        img_result2.gameObject.SetActive(false);
        img_result3.gameObject.SetActive(false);
        img_resultfail1.gameObject.SetActive(true);
        img_resultfail2.gameObject.SetActive(true);
        img_resultfail3.gameObject.SetActive(true);
        img_result.sprite = resultimg[1];
        btn_successback.gameObject.SetActive(false);
        btn_failrecord.gameObject.SetActive(true);
        btn_failback.gameObject.SetActive(true);
    }

    private void SetSuccess()
    {
        img_result.sprite = resultimg[0];
        btn_successback.gameObject.SetActive(true);
        btn_failrecord.gameObject.SetActive(false);
        btn_failback.gameObject.SetActive(false);
    }

    private void BackToScene()
    {
        UIManager.instance.ShowUIFade(UIState.Scene);
    }

    private void RecordAgain()
    {
        
    }
}
