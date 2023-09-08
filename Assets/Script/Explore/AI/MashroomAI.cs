using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class MashroomAI : AI
    {
        public override void Init(List<Generator2D.Room> rooms)
        {
            Generator2D.Room room = rooms[/*Random.Range(0, rooms.Count)*/0];
            Vector3 position = new Vector3(Random.Range(room.bounds.xMin, room.bounds.xMax), 1, Random.Range(room.bounds.yMin, room.bounds.yMax));
            transform.position = position;
        }

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
            Back.transform.DORotate(ExploreManager.Instance.Player.RotateTo + Vector3.down * 180, 0.5f);
        }

        public override void Rotate() 
        {
            Back.transform.DORotate(ExploreManager.Instance.Player.RotateTo + Vector3.down * 180, 0.5f);
        }
    }
}