using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class ExploreEnemyControllerTrace : ExploreEnemyController
    {
        public GameObject Exclamation;

        protected ExploreEnemyContext _context = new ExploreEnemyContext();

        void OnControllerColliderHit(ControllerColliderHit hit)
        {
            ((ExploreEnemyState)_context.CurrentState).OnControllerColliderHit(hit);
        }

        private void Update()
        {
            ((ExploreEnemyState)_context.CurrentState).Update();

            //if (ExploreManager.Instance.PlayerSpeed > 0)
            //{
            //    CharacterController.Move(transform.forward * ExploreManager.Instance.PlayerSpeed);
            //}
        }

        public override void Init(ExploreFileEnemy file)
        {
            File = file;
            _context.Init(this);
            _context.AddState(new TraceState(_context));
            _context.AddState(new DefaultState(_context));
            _context.SetState<DefaultState>();
        }

        protected class DefaultState : ExploreEnemyState
        {
            public DefaultState(ExploreEnemyContext context) : base(context)
            {
            }

            public override void Begin()
            {
                Enemy.Arrow.color = Color.yellow;
                ((ExploreEnemyControllerTrace)Enemy).Exclamation.SetActive(false);
            }

            public override void Update()
            {
                if (ExploreManager.Instance.PlayerSpeed > 0)
                {
                    if (!ChangeState())
                    {
                        Enemy.CharacterController.Move(Enemy.transform.forward * ExploreManager.Instance.PlayerSpeed * 0.5f);
                    }
                }
            }

            private bool ChangeState()
            {
                //if (Physics.Raycast(Enemy.transform.position, Enemy.transform.forward, out RaycastHit hit, 10))
                //{
                //    if (hit.collider.tag == "Player")
                //    {
                //        _context.SetState<TraceState>();

                //        return true;
                //    }
                //    else
                //    {
                //        return false;
                //    }
                //}
                //else
                //{
                //    return false;
                //}

                if(Utility.LookPlayer(Enemy.transform, 60, 3)) 
                {
                    _context.SetState<TraceState>();
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public override void OnControllerColliderHit(ControllerColliderHit hit)
            {
                if (hit.collider.tag == "Player")
                {
                    Enemy.CharacterController.Move(Vector3.zero);
                    ExploreManager.Instance.EnterBattle(Enemy.File);
                }
                else
                {
                    Enemy.transform.eulerAngles += new Vector3(0, 90, 0);
                }
            }
        }

        protected class TraceState : ExploreEnemyState
        {
            private Vector3Int _target = new Vector3Int();

            public TraceState(ExploreEnemyContext context) : base(context)
            {
            }

            public override void Begin()
            {
                _target =Vector3Int.RoundToInt(Enemy.transform.position);
                Enemy.transform.position = _target;
                Enemy.Arrow.color = Color.red;
                ((ExploreEnemyControllerTrace)Enemy).Exclamation.SetActive(true);
            }

            public override void Update()
            {
                if (Vector3.Distance(_target, Enemy.transform.position) < 0.1f)
                {
                    Enemy.transform.position = _target;
                    if (!ChangeState())
                    {
                        Vector2Int playerPosition = Utility.ConvertToVector2Int(ExploreManager.Instance.Player.transform.position);
                        float minDistance = -1;
                        Vector2Int forward = Utility.ConvertToVector2Int(Enemy.transform.position + Vector3.forward);
                        if (ExploreManager.Instance.TileDic[forward].IsWalkable && (minDistance == -1 || Vector2Int.Distance(playerPosition, forward) < minDistance))
                        {
                            minDistance = Vector2Int.Distance(playerPosition, forward);
                            Enemy.transform.eulerAngles = new Vector3(0, 0, 0);
                            _target = Vector3Int.RoundToInt(Enemy.transform.position + Vector3.forward);
                        }
                        Vector2Int back = Utility.ConvertToVector2Int(Enemy.transform.position + Vector3.back);
                        if (ExploreManager.Instance.TileDic[back].IsWalkable && (minDistance == -1 || Vector2Int.Distance(playerPosition, back) < minDistance))
                        {
                            minDistance = Vector2Int.Distance(playerPosition, back);
                            Enemy.transform.eulerAngles = new Vector3(0, 180, 0);
                            _target = Vector3Int.RoundToInt(Enemy.transform.position + Vector3.back);
                        }
                        Vector2Int left = Utility.ConvertToVector2Int(Enemy.transform.position + Vector3.left);
                        if (ExploreManager.Instance.TileDic[left].IsWalkable && (minDistance == -1 || Vector2Int.Distance(playerPosition, left) < minDistance))
                        {
                            minDistance = Vector2Int.Distance(playerPosition, left);
                            Enemy.transform.eulerAngles = new Vector3(0, -90, 0);
                            _target = Vector3Int.RoundToInt(Enemy.transform.position + Vector3.left);
                        }
                        Vector2Int right = Utility.ConvertToVector2Int(Enemy.transform.position + Vector3.right);
                        if (ExploreManager.Instance.TileDic[right].IsWalkable && (minDistance == -1 || Vector2Int.Distance(playerPosition, right) < minDistance))
                        {
                            minDistance = Vector2Int.Distance(playerPosition, right);
                            Enemy.transform.eulerAngles = new Vector3(0, 90, 0);
                            _target = Vector3Int.RoundToInt(Enemy.transform.position + Vector3.right);
                        }
                    }
                }
                else if (ExploreManager.Instance.PlayerSpeed > 0)
                {
                    Enemy.CharacterController.Move((_target - Enemy.transform.position).normalized * ExploreManager.Instance.PlayerSpeed * 0.5f);
                }
            }

            private bool ChangeState()
            {
                if (Vector3.Distance(ExploreManager.Instance.Player.transform.position, Enemy.transform.position) > 5f)
                {
                    _context.SetState<DefaultState>();

                    return true;
                }
                else
                {
                    return false;
                }
            }

            public override void OnControllerColliderHit(ControllerColliderHit hit)
            {
                if (hit.collider.tag == "Player")
                {
                    Enemy.CharacterController.Move(Vector3.zero);
                    ExploreManager.Instance.EnterBattle(Enemy.File);
                }
            }
        }
    }
}