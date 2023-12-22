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
        private Vector2Int _selectedPosition;
        private CameraDraw _cameraController;
        private CameraRotate _cameraRotate;
        private StateContext _context = new StateContext();
        private BattleUI _battleUI;
        private DragCameraUI _dragCameraUI;
        private SelectBattleCharacterUI _selectBattleCharacterUI;
        private BattleResultUI _battleResultUI;
        private GameObject _arrow;
        private EnemyGroupModel _enemyGroup;
        private List<Vector2Int> _areaList = new List<Vector2Int>();
        private Dictionary<int, BattleCharacterController> _controllerDic = new Dictionary<int, BattleCharacterController>();

        public void Init(int floor, int lv, BattleInfo info)
        {
            _battleUI = GameObject.Find("BattleUI").GetComponent<BattleUI>();
            _selectBattleCharacterUI = GameObject.Find("SelectBattleCharacterUI").GetComponent<SelectBattleCharacterUI>();
            _battleResultUI = GameObject.Find("BattleResultUI").GetComponent<BattleResultUI>();
            _cameraController = Camera.main.GetComponent<CameraDraw>();
            _cameraRotate = Camera.main.GetComponent<CameraRotate>();

            Info = info;
            //CharacterList.Add(new BattleCharacterInfo(CharacterManager.Instance.CharacterInfoGroup.CharacterList[0]));
            //CharacterList[0].ID = 1;
            //CharacterList[0].Position = new Vector3(0, 1, 2);
            //CharacterList.Add(new BattleCharacterInfo(CharacterManager.Instance.CharacterInfoGroup.CharacterList[1]));
            //CharacterList[1].ID = 2;
            //CharacterList[1].Position = new Vector3(1, 1, 0);
            //CharacterList.Add(new BattleCharacterInfo(CharacterManager.Instance.CharacterInfoGroup.CharacterList[2]));
            //CharacterList[2].ID = 3;
            //CharacterList[2].Position = new Vector3(0, 1, 1);
            //CharacterList.Add(new BattleCharacterInfo(CharacterManager.Instance.CharacterInfoGroup.CharacterList[3]));
            //CharacterList[3].ID = 4;
            //CharacterList[3].Position = new Vector3(1, 1, 1);
            //CharacterList.Add(new BattleCharacterInfo(CharacterManager.Instance.CharacterInfoGroup.CharacterList[4]));
            //CharacterList[4].ID = 5;
            //CharacterList[4].Position = new Vector3(0, 1, 0);

            KeyValuePair<int, EnemyGroupModel> pair = DataContext.Instance.EnemyGroupDic[floor].ElementAt(UnityEngine.Random.Range(0, DataContext.Instance.EnemyGroupDic[floor].Count));
            _enemyGroup = pair.Value;
            List<int> enemyList = _enemyGroup.EnemyList;
            int index = 0;
            CharacterList.Clear();
            for (int i=0; i< enemyList.Count; i++) 
            {
                CharacterList.Add(new BattleCharacterInfo(lv, DataContext.Instance.EnemyDic[enemyList[i]]));
                CharacterList[index + i].Index = index + 1 + i;
                if (enemyList[i] == 5)
                {
                    CharacterList[index + i].AI = new MonkeyAI(CharacterList[index + i]);
                }
                else
                {
                    CharacterList[index + i].AI = new MashroomAI(CharacterList[index + i]);
                }
                CharacterList[index + i].Position = RandomCharacterPosition(BattleCharacterInfo.FactionEnum.Enemy);
            }
            //CharacterList.Add(new BattleCharacterInfo(DataContext.Instance.EnemyDic[1]));
            //CharacterList[4].ID = 5;
            //CharacterList[4].AI = new MashroomAI(CharacterList[4]);
            //CharacterList[4].Position = /*new Vector3(3, 1, 3)*/ RandomCharacterPosition(BattleCharacterInfo.FactionEnum.Enemy);
            //CharacterList.Add(new BattleCharacterInfo(DataContext.Instance.EnemyDic[1]));
            //CharacterList[5].ID = 6;
            //CharacterList[5].AI = new MashroomAI(CharacterList[5]);
            //CharacterList[5].Position = /*new Vector3(4, 1, 3)*/ RandomCharacterPosition(BattleCharacterInfo.FactionEnum.Enemy);

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

        //public void SetSelectSkillState()
        //{
        //    _context.SetState<SkillState>();
        //}

        //public void SetSupportState() 
        //{
        //    _context.SetState<SupportState>();
        //}

        //public void SetItemState()
        //{
        //    _context.SetState<ItemState>();
        //}

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

        //public void SelectSkill(Skill skill)
        //{
        //    if (_context.CurrentState is SkillState)
        //    {
        //        _selectedCharacter.SelectedSkill = skill;
        //        _selectedCharacter.SelectedSupport = null;
        //        _selectedCharacter.SelectedCard = null;
        //        _selectedCharacter.SelectedConsumables = null;
        //        _selectedCharacter.SelectedFood = null;
        //        _selectedCharacter.SelectedEffect = skill.Effect;
        //        _context.SetState<TargetState>();
        //    }
        //}

        //public void SelectSupport(Support support)
        //{
        //    if (_context.CurrentState is SupportState)
        //    {
        //        _selectedCharacter.SelectedSupport = support;
        //        _selectedCharacter.SelectedSkill = null;
        //        _selectedCharacter.SelectedCard = null;
        //        _selectedCharacter.SelectedConsumables = null;
        //        _selectedCharacter.SelectedFood = null;
        //        _selectedCharacter.SelectedEffect = support.Effect;
        //        _context.SetState<TargetState>();
        //    }
        //}


        //public void SelectItem(Item item)
        //{
        //if (_context.CurrentState is ItemState)
        //{
        //    if (item == null) //返回 
        //    {
        //        Instance._battleUI.SetSkillVisible(false);
        //        Instance.SetActionState();
        //    }
        //    else if (item.Data.PP == -1 || _selectedCharacter.CurrentPP >= item.Data.PP)
        //    {
        //        _selectedCharacter.SelectedItem = item;
        //        _selectedCharacter.SelectedSkill = null;
        //        _selectedCharacter.SelectedSupport = null;
        //        _context.SetState<TargetState>();
        //    }
        //    else
        //    {
        //        Instance._battleUI.TipLabel.SetLabel("PP不足 " + (item.Data.PP - _selectedCharacter.CurrentPP));
        //    }

        //}
        //}

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
            if (!SelectedCharacter.HasUseSkill && SelectedCharacter.LastPosition != BattleCharacterInfo.DefaultLastPosition && SelectedCharacter.ActionCount < 2)
            {
                SelectedCharacter.ActionCount = 2;
                SelectedCharacter.Position = SelectedCharacter.LastPosition;
                _controllerDic[SelectedCharacter.Index].transform.position = SelectedCharacter.LastPosition;
                SelectedCharacter.LastPosition = BattleCharacterInfo.DefaultLastPosition;
            }
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