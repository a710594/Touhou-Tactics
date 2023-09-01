using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class MashroomAI : MonoBehaviour
    {

        public SpriteRenderer Sprite;
        public Vector3 MoveTo;

        public void Init(List<Generator2D.Room> rooms)
        {
            Generator2D.Room room = rooms[Random.Range(0, rooms.Count)];
            Vector3 position = new Vector3(Random.Range(room.bounds.xMin, room.bounds.xMax), 1, Random.Range(room.bounds.yMin, room.bounds.yMax));
            transform.position = position;
        }

        public void Step() 
        {
            if (ExploreManager.Instance.IsWalkable(transform.position + transform.forward))
            {
                MoveTo = transform.position + transform.forward;
                transform.DOMove(transform.position + transform.forward, 0.5f).SetEase(Ease.Linear);
            }
            else if (ExploreManager.Instance.IsWalkable(transform.position + transform.right))
            {
                MoveTo = transform.position + transform.right;
                transform.DOMove(transform.position + transform.right, 0.5f).SetEase(Ease.Linear);
                transform.DORotate(transform.localEulerAngles + Vector3.up * 90, 0.5f).SetEase(Ease.Linear);
            }
            else if (ExploreManager.Instance.IsWalkable(transform.position - transform.right))
            {
                MoveTo = transform.position - transform.right;
                transform.DOMove(transform.position - transform.right, 0.5f).SetEase(Ease.Linear);
                transform.DORotate(transform.localEulerAngles - Vector3.up * 90, 0.5f).SetEase(Ease.Linear);
            }
            else if (ExploreManager.Instance.IsWalkable(transform.position - transform.forward))
            {
                MoveTo = transform.position - transform.forward;
                transform.DOMove(transform.position - transform.forward, 0.5f).SetEase(Ease.Linear);
                transform.DORotate(transform.localEulerAngles + Vector3.up * 180, 0.5f).SetEase(Ease.Linear);
            }

            Vector3 v = ExploreManager.Instance.Player.transform.position - transform.position;
            float angle = Vector2.Angle(new Vector2(v.x, v.z), new Vector2(transform.forward.x, transform.forward.z));
            if (angle >= 90)
            {
                Sprite.transform.DORotate(ExploreManager.Instance.Player.transform.eulerAngles, 0.5f);
            }
            else
            {
                Sprite.transform.DORotate(ExploreManager.Instance.Player.transform.eulerAngles + Vector3.up * 180, 0.5f);
            }
        }
    }
}