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
                Camera.main.transform.localPosition = new Vector3(-10, 10, -10);

                int needCount = _info.NeedCount;
                if (CharacterManager.Instance.SurvivalCount() < needCount) 
                {
                    needCount = CharacterManager.Instance.SurvivalCount();
                }

                Instance.SelectBattleCharacterUI.Init(needCount, _info.MustBeEqualToNeedCount, Instance._candidateList);

                for (int i=0; i< _info.PlayerPositionList.Count; i++) 
                {
                    _info.TileDic[_info.PlayerPositionList[i]].TileObject.Quad.gameObject.SetActive(true);
                }

                if(Instance.PrepareStateBeginHandler!=null)
                {
                    Instance.PrepareStateBeginHandler();
                }
            }

            public override void End()
            {
                BattleCharacterInfo info;
                foreach (KeyValuePair<CharacterInfo, BattleCharacterController> pair in _tempDic)
                {
                    info = new BattleCharacterInfo(pair.Key, CharacterManager.Instance.Info.Lv);
                    Instance._maxIndex++;
                    info.Index = Instance._maxIndex;
                    info.Position = pair.Value.transform.position;
                    info.Controller = pair.Value.GetComponent<BattleCharacterController>();
                    _characterList.Add(info);

                    info.Controller.MoveEndHandler += Instance.OnMoveEnd;
                    Instance.BattleUI.SetLittleHpBarAnchor(info);
                    Instance.BattleUI.SetLittleHpBarValue(info);
                    Instance.BattleUI.SetFloatingNumberPoolAnchor(info);
                }
                Instance.BattleUI.gameObject.SetActive(true);
                Instance.SelectBattleCharacterUI.gameObject.SetActive(false);

                Instance.SortCharacterList(true);
                Instance.CharacterListGroupInit();
            }

            public BattleCharacterController PlaceCharacter(Vector2Int position, CharacterInfo characterInfo)
            {
                if (_info.PlayerPositionList.Contains(position))
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
                        BattleCharacterController character = ((GameObject)GameObject.Instantiate(Resources.Load("Prefab/Character/Player/" + characterInfo.Controller), Vector3.zero, Quaternion.identity)).GetComponent<BattleCharacterController>();
                        character.transform.position = new Vector3(position.x, _info.TileDic[position].TileData.Height, position.y);
                        character.transform.SetParent(Instance._root);
                        character.Init(characterInfo.Controller);
                        character.SetAngle();
                        _tempDic.Add(characterInfo, character);
                    }
                    else
                    {
                        _tempDic[characterInfo].gameObject.SetActive(true);
                        _tempDic[characterInfo].transform.position = new Vector3(position.x, _info.TileDic[position].TileData.Height, position.y);
                    }
                    Instance.DragCameraUI.DontDrag = false;

                    return _tempDic[characterInfo];
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
