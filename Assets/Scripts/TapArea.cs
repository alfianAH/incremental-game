﻿using UnityEngine;
using UnityEngine.EventSystems;

public class TapArea : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        // On pointer down, collect gold
        GameManager.Instance.CollectByTap(eventData.position, transform);
    }
}
