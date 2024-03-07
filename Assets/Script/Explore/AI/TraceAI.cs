using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class TraceAI : ExploreEnemyController
    {
        public GameObject ExclamationMark;

        private bool _rest = true;

        public override void Move()
        {
            ExploreManager.Instance.Info.WalkableList.Add(Utility.ConvertToVector2Int(MoveTo));

            Vector2Int playerPosition = Utility.ConvertToVector2Int(ExploreManager.Instance.Player.MoveTo);
            Vector2Int currentPosition = Utility.ConvertToVector2Int(transform.position);

            if(CanSee(playerPosition, currentPosition)) 
            {
                int distance = ExploreManager.Instance.GetDistance(playerPosition, currentPosition);
                if (distance != -1 && distance < 10)
                {
                    if (!_rest)
                    {
                        int min = distance;
                        Vector3 r = Vector3.zero;
                        Vector3 v3;
                        Vector2Int v2;

                        v3 = transform.position + transform.forward;
                        v2 = Utility.ConvertToVector2Int(v3);
                        if (ExploreManager.Instance.IsWalkableNew(v2))
                        {
                            distance = ExploreManager.Instance.GetDistance(playerPosition, v2);
                            if (distance < min)
                            {
                                min = distance;
                                r = transform.localEulerAngles + Vector3.up * 0;
                                MoveTo = v3;
                            }
                        }

                        v3 = transform.position - transform.forward;
                        v2 = Utility.ConvertToVector2Int(v3);
                        if (ExploreManager.Instance.IsWalkableNew(v2))
                        {
                            distance = ExploreManager.Instance.GetDistance(playerPosition, v2);
                            if (distance < min)
                            {
                                min = distance;
                                r = transform.localEulerAngles + Vector3.up * 180;
                                MoveTo = v3;
                            }
                        }

                        v3 = transform.position + transform.right;
                        v2 = Utility.ConvertToVector2Int(v3);
                        if (ExploreManager.Instance.IsWalkableNew(v2))
                        {
                            distance = ExploreManager.Instance.GetDistance(playerPosition, v2);
                            if (distance < min)
                            {
                                min = distance;
                                r = transform.localEulerAngles + Vector3.up * 90;
                                MoveTo = v3;
                            }
                        }

                        v3 = transform.position - transform.right;
                        v2 = Utility.ConvertToVector2Int(v3);
                        if (ExploreManager.Instance.IsWalkableNew(v2))
                        {
                            distance = ExploreManager.Instance.GetDistance(playerPosition, v2);
                            if (distance < min)
                            {
                                min = distance;
                                r = transform.localEulerAngles - Vector3.up * 90;
                                MoveTo = v3;
                            }
                        }

                        transform.DOMove(MoveTo, 0.5f).SetEase(Ease.Linear);
                        transform.DORotate(r, 0.5f).SetEase(Ease.Linear);
                    }
                    _rest = !_rest;
                }
            }
            else
            {
                _rest = true;
            }
            Arrow.SetActive(_rest);
            ExclamationMark.SetActive(!_rest);
            ExclamationMark.transform.eulerAngles = new Vector3(90, 0, 0);

            ExploreManager.Instance.Info.WalkableList.Remove(Utility.ConvertToVector2Int(MoveTo));
        }
    }
}