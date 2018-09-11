using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Control : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public GameObject prefabRippleEffect;

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        CreateNewRipple(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        CheckTap();
    }
    private void CheckTap()
    {
        foreach (Touch item in Input.touches)
        {
            if (item.phase == TouchPhase.Began)
            {
                CreateNewRipple(item.position);
            }
        }
    }
    /// <summary>
    /// 创建新的波纹
    /// </summary>
    /// <param name="pos"></param>
    private void CreateNewRipple(Vector2 pos)
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(pos);
        GameObject tempNewRipple = Instantiate(prefabRippleEffect, worldPos, Quaternion.identity, transform);
    }
}
