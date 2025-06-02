using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{

    public class NewFloatingNumber : MonoBehaviour
    {
        public Action<NewFloatingNumber> RecycleHandler;

        public Text Label;
        public float Height;

        public void Play(float duration, Vector2 position, FloatingNumberData data)
        {
            transform.position = position;
            transform.DOMoveY(position.y + Height, duration).SetEase(Ease.OutCubic);
            Label.text = data.Text;
            Label.color = data.Color;
            Label.DOFade(0, duration).SetEase(Ease.InCubic).OnComplete(() =>
            {
                if (RecycleHandler != null)
                {
                    RecycleHandler(this);
                }
            });
        }

        void Awake()
        {
            Label.color = Color.clear;
        }
    }
}