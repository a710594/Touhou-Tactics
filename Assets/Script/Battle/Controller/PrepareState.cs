using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public partial class BattleController
    {
        private class PrepareState : BattleControllerState 
        {
            private Dictionary<CharacterInfo, GameObject> _tempDic = new Dictionary<CharacterInfo, GameObject>();

            public PrepareState(StateContext context) : base(context)
            {
                _info = Instance.Info;
                _characterList = Instance.CharacterList;
            }

            public override void Begin()
            {
                _info = Instance.Info;
                Camera.main.transform.position = new Vector3(_info.NoAttachList[0].x-10, 10, _info.NoAttachList[0].y-10);

                Instance.Arrow = GameObject.Find("Arrow");
                Instance.DirectionGroup = GameObject.Find("DirectionGroup");
                Instance.DirectionGroup.SetActive(false);

                for (int i=0; i< _info.NoAttachList.Count; i++) 
                {
                    _info.TileComponentDic[_info.NoAttachList[i]].Quad.gameObject.SetActive(true);
                }
            }

            public override void End()
            {
                BattleCharacterInfo battleCharacter;
                foreach (KeyValuePair<CharacterInfo, GameObject> pair in _tempDic)
                {
                    battleCharacter = new BattleCharacterInfo(pair.Key, CharacterManager.Instance.Info.Lv);
                    Instance._maxIndex++;
                    battleCharacter.Index = Instance._maxIndex;
                    battleCharacter.Position = pair.Value.transform.position;
                    _characterList.Add(battleCharacter);
                    Instance._controllerDic.Add(battleCharacter.Index, pair.Value.GetComponent<BattleCharacterController>());
                    Instance._controllerDic[battleCharacter.Index].SetSprite(battleCharacter.Sprite);
                    Instance._controllerDic[battleCharacter.Index].MoveEndHandler += Instance.OnMoveEnd;
                    Instance._controllerDic[battleCharacter.Index].SetDirectionHandler += Instance.SetDirection;
                    Instance._battleUI.SetLittleHpBarAnchor(battleCharacter.Index, Instance._controllerDic[battleCharacter.Index]);
                    Instance._battleUI.SetLittleHpBarValue(battleCharacter.Index, battleCharacter);
                    Instance._battleUI.SetFloatingNumberPoolAnchor(battleCharacter.Index, Instance._controllerDic[battleCharacter.Index]);
                }
                Instance._battleUI.gameObject.SetActive(true);
                Instance._selectBattleCharacterUI.gameObject.SetActive(false);

                Instance.SortCharacterList(true);

                Instance._battleUI.CharacterListGroupInit(_characterList);

                //Instance.Arrow = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Other/Arrow"), Vector3.zero, Quaternion.identity);
            }

            public GameObject PlaceCharacter(Vector2Int position, CharacterInfo characterInfo)
            {
                if (_info.NoAttachList.Contains(position))
                {
                    //不能和其他角色重疊
                    foreach (KeyValuePair<CharacterInfo, GameObject> pair in _tempDic)
                    {
                        if (pair.Key != characterInfo && Utility.ConvertToVector2Int(pair.Value.transform.position) == position)
                        {
                            return null;
                        }
                    }

                    if (!_tempDic.ContainsKey(characterInfo))
                    {
                        GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Character/" + characterInfo.Controller), Vector3.zero, Quaternion.identity);
                        obj.transform.position = new Vector3(position.x, _info.TileAttachInfoDic[position].Height, position.y);
                        obj.transform.SetParent(Instance._root);
                        _tempDic.Add(characterInfo, obj);
                    }
                    else
                    {
                        _tempDic[characterInfo].SetActive(true);
                        _tempDic[characterInfo].transform.position = new Vector3(position.x, _info.TileAttachInfoDic[position].Height, position.y);
                    }
                    Instance._dragCameraUI.DontDrag = false;

                    return _tempDic[characterInfo];
                }
                else
                {
                    return null;
                }
            }

            public void SetCharacterSpriteVisible(CharacterInfo characterInfo, bool isVisible) 
            {
                Instance._dragCameraUI.DontDrag = !isVisible;
                if (_tempDic.ContainsKey(characterInfo))
                {
                    _tempDic[characterInfo].SetActive(isVisible);
                }
            }

            public void RemoveCharacterSprite(CharacterInfo characterInfo)
            {
                Instance._dragCameraUI.DontDrag = false;
                GameObject obj = _tempDic[characterInfo];
                _tempDic.Remove(characterInfo);
                GameObject.Destroy(obj);
            }
        }
    }
}
