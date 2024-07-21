using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public partial class BattleController
    {
        public class PrepareState : BattleControllerState 
        {
            private Dictionary<CharacterInfo, BattleCharacterController> _tempDic = new Dictionary<CharacterInfo, BattleCharacterController>();

            public PrepareState(StateContext context) : base(context)
            {
                _info = Instance.Info;
                _characterList = Instance.CharacterList;
            }

            public override void Begin()
            {
                _info = Instance.Info;
                Camera.main.transform.localPosition = new Vector3(_info.NoAttachList[0].x-10, 10, _info.NoAttachList[0].y-10);

                Instance.DirectionGroup.SetActive(false);

                int needCount = _info.NeedCount;
                if (CharacterManager.Instance.SurvivalCount() < needCount) 
                {
                    needCount = CharacterManager.Instance.SurvivalCount();
                }

                List<CharacterInfo> tempCharacterList = new List<CharacterInfo>();
                if (Instance.IsTutorial)
                {
                    tempCharacterList = Instance.Tutorial.GetCharacterList();
                }
                else
                {
                    for (int i=0; i<tempCharacterList.Count; i++) 
                    {
                        if (tempCharacterList[i].CurrentHP == 0) 
                        {
                            tempCharacterList.RemoveAt(i);
                            i--;
                        }
                    }
                }
                Instance.SelectBattleCharacterUI.Init(needCount, _info.MustBeEqualToNeedCount, tempCharacterList);

                for (int i=0; i< _info.NoAttachList.Count; i++) 
                {
                    _info.TileComponentDic[_info.NoAttachList[i]].Quad.gameObject.SetActive(true);
                }

                if(Instance.PrepareStateBeginHandler!=null)
                {
                    Instance.PrepareStateBeginHandler();
                }
            }

            public override void End()
            {
                BattleCharacterInfo battleCharacter;
                foreach (KeyValuePair<CharacterInfo, BattleCharacterController> pair in _tempDic)
                {
                    battleCharacter = new BattleCharacterInfo(pair.Key, CharacterManager.Instance.Info.Lv);
                    Instance._maxIndex++;
                    battleCharacter.Index = Instance._maxIndex;
                    battleCharacter.Position = pair.Value.transform.position;
                    _characterList.Add(battleCharacter);
                    Instance._controllerDic.Add(battleCharacter.Index, pair.Value.GetComponent<BattleCharacterController>());
                    //Instance._controllerDic[battleCharacter.Index].SetSprite(battleCharacter.Sprite);
                    Instance._controllerDic[battleCharacter.Index].MoveEndHandler += Instance.OnMoveEnd;
                    Instance.BattleUI.SetLittleHpBarAnchor(battleCharacter.Index, Instance._controllerDic[battleCharacter.Index]);
                    Instance.BattleUI.SetLittleHpBarValue(battleCharacter.Index, battleCharacter);
                    Instance.BattleUI.SetFloatingNumberPoolAnchor(battleCharacter.Index, Instance._controllerDic[battleCharacter.Index]);
                }
                Instance.BattleUI.gameObject.SetActive(true);
                Instance.SelectBattleCharacterUI.gameObject.SetActive(false);

                Instance.SortCharacterList(true);
                Instance.CharacterListGroupInit();
            }

            public GameObject PlaceCharacter(Vector2Int position, CharacterInfo characterInfo)
            {
                if (_info.NoAttachList.Contains(position))
                {
                    //????M??L?????|
                    foreach (KeyValuePair<CharacterInfo, BattleCharacterController> pair in _tempDic)
                    {
                        if (pair.Key != characterInfo && Utility.ConvertToVector2Int(pair.Value.transform.position) == position)
                        {
                            return null;
                        }
                    }

                    if (!_tempDic.ContainsKey(characterInfo))
                    {
                        BattleCharacterController character = ((GameObject)GameObject.Instantiate(Resources.Load("Prefab/Character/" + characterInfo.Controller), Vector3.zero, Quaternion.identity)).GetComponent<BattleCharacterController>();
                        character.transform.position = new Vector3(position.x, _info.TileAttachInfoDic[position].Height, position.y);
                        character.transform.SetParent(Instance._root);
                        character.Init(characterInfo.Controller);
                        character.SetAngle();
                        _tempDic.Add(characterInfo, character);
                    }
                    else
                    {
                        _tempDic[characterInfo].gameObject.SetActive(true);
                        _tempDic[characterInfo].transform.position = new Vector3(position.x, _info.TileAttachInfoDic[position].Height, position.y);
                    }
                    Instance.DragCameraUI.DontDrag = false;

                    return _tempDic[characterInfo].gameObject;
                }
                else
                {
                    return null;
                }
            }

            public void SetCharacterSpriteVisible(CharacterInfo characterInfo, bool isVisible) 
            {
                Instance.DragCameraUI.DontDrag = !isVisible;
                if (_tempDic.ContainsKey(characterInfo))
                {
                    _tempDic[characterInfo].gameObject.SetActive(isVisible);
                }
            }

            public void RemoveCharacterSprite(CharacterInfo characterInfo)
            {
                Instance.DragCameraUI.DontDrag = false;
                GameObject.Destroy(_tempDic[characterInfo].gameObject);
                _tempDic.Remove(characterInfo);
            }
        }
    }
}
