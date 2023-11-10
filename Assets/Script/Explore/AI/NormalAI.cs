using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class NormalAI : AI //¼²¨ìÀð´NÂàÅs
    {
        public override void Move() 
        {
            if (ExploreManager.Instance.IsWalkable(this, transform.position + transform.forward))
            {
                MoveTo = transform.position + transform.forward;
                transform.DOMove(transform.position + transform.forward, 0.5f).SetEase(Ease.Linear);
            }
            else if (ExploreManager.Instance.IsWalkable(this, transform.position + transform.right))
            {
                MoveTo = transform.position + transform.right;
                transform.DOMove(transform.position + transform.right, 0.5f).SetEase(Ease.Linear);
                transform.DORotate(transform.localEulerAngles + Vector3.up * 90, 0.5f).SetEase(Ease.Linear);
            }
            else if (ExploreManager.Instance.IsWalkable(this, transform.position - transform.right))
            {
                MoveTo = transform.position - transform.right;
                transform.DOMove(transform.position - transform.right, 0.5f).SetEase(Ease.Linear);
                transform.DORotate(transform.localEulerAngles - Vector3.up * 90, 0.5f).SetEase(Ease.Linear);
            }
            else if (ExploreManager.Instance.IsWalkable(this, transform.position - transform.forward))
            {
                MoveTo = transform.position - transform.forward;
                transform.DOMove(transform.position - transform.forward, 0.5f).SetEase(Ease.Linear);
                transform.DORotate(transform.localEulerAngles + Vector3.up * 180, 0.5f).SetEase(Ease.Linear);
            }
        }
    }
}