using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookPanel : UIBase {

    public Button m_click;
    public GameObject m_image;
    private Button m_event;
    private Button m_collect;

    private RectTransform m_rectTransform;
    private Button btn_book;
    private BookPro m_bookPro;
    private AutoFlip m_autoFlip;

    public override void OnAwake()
    {
        base.OnAwake();
        m_click.onClick.AddListener(Click);
        m_rectTransform = this.GetComponent<RectTransform>();
        m_event = transform.Find("btn_event").GetComponent<Button>();
        m_collect = transform.Find("btn_collect").GetComponent<Button>();
        btn_book = transform.Find("btn_book").GetComponent<Button>();
        m_bookPro = transform.Find("BookPro").GetComponent<BookPro>();
        m_autoFlip = transform.Find("BookPro").GetComponent<AutoFlip>();
        btn_book.onClick.AddListener(() => StartCoroutine(DynamicTansform(1f, new Vector3(100, -200, 0), 1f)));
        m_event.onClick.AddListener(SetEventPage);
        m_event.gameObject.SetActive(false);
        m_collect.gameObject.SetActive(false);
    }

    public override void Appear()
    {
        base.Appear();
        transform.SetSiblingIndex(transform.GetComponentsInChildren<UIBase>().Length-1);
    }

    private void Click()
    {
        UIManager.instance.ShowFadeTransition();
    }

    void SetEventPage()
    {
        int dif =  m_bookPro.currentPaper - 1;
        m_autoFlip.PageFlipTime = m_autoFlip.PageFlipTime / 3;
        if (dif>0)
        {
            m_autoFlip.FlipLeftPage();
            m_bookPro.currentPaper =2;
        }
        else if(dif<0)
        {
            m_autoFlip.FlipRightPage();
        }
        m_autoFlip.PageFlipTime = 1f;
    }

    private IEnumerator DynamicTansform(float scale,Vector3 pos,float time)
    {
        float dur = 0f;
        Vector3 startPos = m_rectTransform.localPosition;
        Vector2 startScale = m_rectTransform.localScale;
        Vector3 endPos = m_rectTransform.localPosition + pos;
        Vector2 endScale = new Vector2(m_rectTransform.localScale.x + scale, m_rectTransform.localScale.y + scale);
        while (dur<=time)
        {
            dur += Time.deltaTime;
            m_rectTransform.localPosition = Vector3.Lerp(startPos, endPos, dur / time);
            m_rectTransform.localScale = Vector2.Lerp(startScale, endScale, dur / time);
            yield return null;
        }
        m_event.gameObject.SetActive(true);
        m_collect.gameObject.SetActive(true);
    }

    void Fade()
    {
        UIManager.instance.ShowFadeTransition();
    }
}
