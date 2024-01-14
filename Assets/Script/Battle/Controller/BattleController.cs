using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

namespace Battle
{
    public partial class BattleController
    {
        private static BattleController _instance;
        public static BattleController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BattleController();
                }
                return _instance;
            }
        }

        public BattleInfo Info;
        public BattleCharacterInfo SelectedCharacter;
        public List<BattleCharacterInfo> CharacterList = new List<BattleCharacterInfo>();

        private readonly Color _white = new Color(1, 1, 1, 0.5f);
        private readonly Color _yellow = new Color(1, 1, 0, 0.5f);

        private bool _canClick = true;
        private int _exp;
        private Vector2Int _selectedPosition;
        private CameraDraw _cameraController;
        private CameraRotate _cameraRotate;
        private StateContext _context = new StateContext();
        private BattleUI _battleUI;
        private DragCameraUI _dragCameraUI;
        private SelectBattleCharacterUI _selectBattleCharacterUI;
        private BattleResultUI _battleResultUI;
        public GameObject Arrow;
        private List<int> _enemyList = new List<int>();
        private List<Vector2Int> _areaList = new List<Vector2Int>();
        private Dictionary<int, BattleCharacterController> _controllerDic = new Dictionary<int, BattleCharacterController>();

        public void Init(int floor, int lv, BattleInfo info)
        {
            _battleUI = GameObject.Find("BattleUI").GetComponent<BattleUI>();
            _selectBattleCharacterUI = GameObject.Find("SelectBattleCharacterUI").GetComponent<SelectBattleCharacterUI>();
            _selectBattleCharacterUI.Init(info);
            _battleResultUI = GameObject.Find("BattleResultUI").GetComponent<BattleResultUI>();
            _cameraController = Camera.main.GetComponent<CameraDraw>();
            _cameraRotate = Camera.main.GetComponent<CameraRotate>();
            Info = info;

            if (info.EnemyDic.Count == 0)
            {
                EnemyGroupModel enemyGroup = DataContext.Instance.EnemyGroupDic[floor].ElementAt(UnityEngine.Random.Range(0, DataContext.Instance.EnemyGroupDic[floor].Count)).Value;
                List<int> enemyList = enemyGroup.EnemyList;
                for (int i = 0; i < enemyList.Count; i++)
                {
                    CharacterList.Add(new BattleCharacterInfo(lv, DataContext.Instance.EnemyDic[enemyList[i]]));
                    CharacterList[i].Index = i + 1;
                    if (enemyList[i] == 5)
                    {
                        CharacterList[i].AI = new MonkeyAI(CharacterList[i]);
                    }
                    else
                    {
                        CharacterList[i].AI = new MashroomAI(CharacterList[i]);
                    }
                    CharacterList[i].Position = RandomCharacterPosition(BattleCharacterInfo.FactionEnum.Enemy);
                }
            }
            else
            {
                int index = 0;
                foreach(KeyValuePair<Vector3Int, int> pair in info.EnemyDic) 
                {
                    CharacterList.Add(new BattleCharacterInfo(lv, DataContext.Instance.EnemyDic[pair.Value]));
                    CharacterList[index].Index = index + 1;
                    if (pair.Value == 5)
                    {
                        CharacterList[index].AI = new MonkeyAI(CharacterList[index]);
                    }
                    else
                    {
                        CharacterList[index].AI = new MashroomAI(CharacterList[index]);
                    }
                    CharacterList[index].Position = pair.Key;
                    index++;
                }
            }

            _dragCameraUI = GameObject.Find("DragCameraUI").GetComponent<DragCameraUI>();
            _dragCameraUI.Init(info.Width, info.Height);

            GameObject obj;
            _controllerDic.Clear();
            for (int i = 0; i < CharacterList.Count; i++)
            {
                info.TileInfoDic[Utility.ConvertToVector2Int(CharacterList[i].Position)].HasCharacter = true;
                obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Character/" + CharacterList[i].Controller), Vector3.zero, Quaternion.identity);
                obj.transform.position = CharacterList[i].Position;
                _controllerDic.Add(CharacterList[i].Index, obj.GetComponent<BattleCharacterController>());
                _controllerDic[CharacterList[i].Index].MoveEndHandler += OnMoveEnd;
                _controllerDic[CharacterList[i].Index].SetDirectionHandler += SetDirection;
                _battleUI.SetLittleHpBarAnchor(CharacterList[i].Index, _controllerDic[CharacterList[i].Index]);
                _battleUI.SetLittleHpBarValue(CharacterList[i].Index, CharacterList[i]);
                _battleUI.SetFloatingNumberPoolAnchor(CharacterList[i].Index, _controllerDic[CharacterList[i].Index]);
            }
            _battleUI.gameObject.SetActive(false);
            _battleResultUI.gameObject.SetActive(false);

            //SortCharacterList(true);

            //_battleUI.CharacterListGroupInit(CharacterList);

            //_arrow = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Other/Arrow"), Vector3.zero, Quaternion.identity);

            _context.ClearState();
            _context.AddState(new PrepareState(_context));
            _context.AddState(new CharacterState(_context));
            _context.AddState(new ActionState(_context));
            _context.AddState(new MoveState(_context));
            //_context.AddState(new SkillState(_context));
            //_context.AddState(new SupportState(_context));
            //_context.AddState(new ItemState(_context));
            _context.AddState(new TargetState(_context));
            _context.AddState(new ConfirmState(_context));
            _context.AddState(new EffectState(_context));
            _context.AddState(new EndState(_context));
            _context.AddState(new WinState(_context));

            _context.SetState<PrepareState>();
        }

        public void Click(Vector2Int position)
        {
            if (!_canClick)
            {
                return;
            }

            ((BattleControllerState)_context.CurrentState).Click(position);
        }

        public GameObject PlaceCharacter(Vector2Int position, CharacterInfo characterInfo)
        {
            if(_context.CurrentState is PrepareState) 
            {
                return ((PrepareState)_context.CurrentState).PlaceCharacter(position, characterInfo);
            }
            else
            {
                return null;
            }
        }

        public void HideCharacter(CharacterInfo characterInfo) 
        {
            if (_context.CurrentState is PrepareState)
            {
                ((PrepareState)_context.CurrentState).HideCharacter(characterInfo);
            }
        }

        public void SetCharacterState() 
        {
            _context.SetState<CharacterState>();
        }

        public void SetActionState()
        {
            _context.SetState<ActionState>();
        }

        public void SetMoveState()
        {
            _context.SetState<MoveState>();
        }

        public void Idle()
        {
            SelectedCharacter.ActionCount = 0;
            _context.SetState<EndState>();
        }

        public void SelectObject(object obj)
        {
            SelectedCharacter.SelectedObject = obj;
            _context.SetState<TargetState>();
        }

        public void SetQuad(List<Vector2Int> list, Color color)
        {
            ClearQuad();

            for (int i = 0; i < list.Count; i++)
            {
                Info.TileComponentDic[list[i]].Quad.gameObject.SetActive(true);
                Info.TileComponentDic[list[i]].Quad.material.SetColor("_Color", color);
            }
        }

        public void SetSkillArea(Effect effect)
        {
            if (effect.Area == "Through")
            {
                _areaList = GetTroughAreaList(SelectedCharacter.Position, new Vector3(_selectedPosition.x, Instance.Info.TileInfoDic[_selectedPosition].Height, _selectedPosition.y));
            }
            else
            {
                _areaList = GetNormalAreaList(Utility.ConvertToVector2Int(SelectedCharacter.Position), _selectedPosition, effect);
            }

            RemoveByFaction(effect, _areaList);
            SetQuad(_areaList, _yellow);
        }

        public void ClearQuad()
        {
            foreach (KeyValuePair<Vector2Int, TileComponent> pair in Info.TileComponentDic)
            {
                pair.Value.Quad.gameObject.SetActive(false);
            }
        }

        public void Skip()
        {
            _context.SetState<EndState>();
        }

        public void OnMoveEnd()
        {
            _canClick = true;
            SelectedCharacter.ActionCount--;

            if (SelectedCharacter.AI != null)
            {
                SelectedCharacter.AI.OnMoveEnd();
            }
            else
            {
                if (SelectedCharacter.ActionCount > 0)
                {
                    _context.SetState<ActionState>();
                }
                else
                {
                    _context.SetState<EndState>();
                }
            }
        }

        public void SetDirection(Vector2Int direction) 
        {
            SelectedCharacter.Direction = direction;
        }

        public bool SetCharacterInfoUI_2(Vector2 position)
        {
            //顯示角色資料
            Instance._battleUI.SetCharacterInfoUI_2(null);
            for (int i = 0; i < CharacterList.Count; i++)
            {
                if (CharacterList[i] != Instance.SelectedCharacter && position == new Vector2(CharacterList[i].Position.x, CharacterList[i].Position.z))
                {
                    Instance._battleUI.SetCharacterInfoUI_2(CharacterList[i]);
                    return true;
                }
            }
            return false;
        }

        public void ResetAction()
        {
            SelectedCharacter.ActionCount = 2;
            SelectedCharacter.Position = SelectedCharacter.LastPosition;
            SelectedCharacter.HasMove = false;
            _controllerDic[SelectedCharacter.Index].transform.position = SelectedCharacter.LastPosition;
            SelectedCharacter.LastPosition = BattleCharacterInfo.DefaultLastPosition;
        }

        private void SortCharacterList(bool isStart)
        {
            CharacterList.Sort((x, y) =>
            {
                if (isStart && !Passive.Contains<TankPassive>(x.PassiveList) && Passive.Contains<TankPassive>(y.PassiveList))
                {
                    return 1;
                }
                else if (isStart && Passive.Contains<TankPassive>(x.PassiveList) && !Passive.Contains<TankPassive>(y.PassiveList))
                {
                    return -1;
                }
                else 
                {
                    if (x.CurrentWT > y.CurrentWT)
                    {
                        return 1;
                    }
                    else
                    {
                        if (x.CurrentWT == y.CurrentWT)
                        {
                            if (x.Index > y.Index)
                            {
                                return 1;
                            }
                            else
                            {
                                return -1;
                            }
                        }
                        else
                        {
                            return -1;
                        }
                    }
                }
            });
        }

        public void SetWin() 
        {
            _context.SetState<WinState>();
        }

        private class BattleControllerState : State
        {
            protected BattleInfo _info;
            protected BattleCharacterInfo _character;
            protected List<BattleCharacterInfo> _characterList;

            public BattleControllerState(StateContext context) : base(context)
            {
            }

            public virtual void Next() { }

            public virtual void Click(Vector2Int position)
            {
            }
        }
    }
}