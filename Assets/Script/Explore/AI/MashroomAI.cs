using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class MashroomAI : MonoBehaviour
    {
        private Vector3 position;

        public void Init(List<Generator2D.Room> rooms)
        {
            Generator2D.Room room = rooms[0];
            Vector3 position = new Vector3(Random.Range(room.bounds.xMin, room.bounds.xMax), 1, Random.Range(room.bounds.yMin, room.bounds.yMax));
            transform.position = position;
        }

        public void Step() 
        {
            position = transform.position + transform.forward;
            if (ExploreManager.Instance.IsWalkable(transform.position + transform.forward))
            {
                transform.DOMove(transform.position + transform.forward, 0.5f).SetEase(Ease.Linear);
            }
            else if (ExploreManager.Instance.IsWalkable(transform.position + transform.right))
            {
                transform.DOMove(transform.position + transform.right, 0.5f).SetEase(Ease.Linear);
                transform.DORotate(transform.localEulerAngles + Vector3.up * 90, 0.5f).SetEase(Ease.Linear);
            }
            else if (ExploreManager.Instance.IsWalkable(transform.position - transform.right))
            {
                transform.DOMove(transform.position - transform.right, 0.5f).SetEase(Ease.Linear);
                transform.DORotate(transform.localEulerAngles - Vector3.up * 90, 0.5f).SetEase(Ease.Linear);
            }
            else if (ExploreManager.Instance.IsWalkable(transform.position - transform.forward))
            {
                transform.DOMove(transform.position - transform.forward, 0.5f).SetEase(Ease.Linear);
                transform.DORotate(transform.localEulerAngles + Vector3.up * 180, 0.5f).SetEase(Ease.Linear);
            }
        }
    }
}