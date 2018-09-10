using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeTransition : UIBase {

    public float fadeSpeed = 0.3f;

    private float alpha = 0f;
    private bool isFade = false;
    private int fadeDir = 1;

    private Image m_fadeImage;
    public UICompleteHandle m_FadeOutEnd;

    public override void OnAwake()
    {
        base.OnAwake();
        m_fadeImage = this.GetComponent<Image>();
        m_uiType = UIType.FadeWindow;
    }

    public override void Appear()
    {
        base.Appear();
        transform.SetAsLastSibling();
        isFade = true;
        FadeIn();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if(isFade)
        {
            alpha += fadeDir*fadeSpeed * Time.deltaTime;
            alpha = Mathf.Clamp01(alpha);
            if(alpha>=1f)
            {
                EventArgs eventArgs = new EventArgs();
                m_FadeOutEnd(this.gameObject,eventArgs);
                FadeOut();
            }
            else if(alpha<=0 && fadeDir==-1)
            {
                OnDisAppear();
            }
            m_fadeImage.color = new Color(m_fadeImage.color.r, m_fadeImage.color.g,m_fadeImage.color.b, alpha);
        }
    }

    private void FadeIn()
    {
        fadeDir = 1;
    }

    private void FadeOut()
    {
        fadeDir = -1;
        fadeSpeed *= 2;
    }

    public override void OnDisAppear()
    {
        base.OnDisAppear();
        isFade = false;
    }
}
