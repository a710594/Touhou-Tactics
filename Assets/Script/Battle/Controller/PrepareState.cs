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

            public override void Begin(object obj)
            {
                _info = Instance.Info;
                Camera.main.transform.position = new Vector3(_info.NoAttachList[0].x-10, 10, _info.NoAttachList[0].y-10);

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
                    battleCharacter = new BattleCharacterInfo(pair.Key);
                    battleCharacter.ID = _characterList.Count + 1;
                    battleCharacter.Position = pair.Value.transform.position;
                    _characterList.Add(battleCharacter);
                    _info.TileInfoDic[Utility.ConvertToVector2Int(battleCharacter.Position)].HasCharacter = true;
                    Instance._controllerDic.Add(battleCharacter.ID, pair.Value.GetComponent<BattleCharacterController>());
                    Instance._controllerDic[battleCharacter.ID].MoveEndHandler += Instance.OnMoveEnd;
                    Instance._controllerDic[battleCharacter.ID].SetDirectionHandler += Instance.SetDirection;
                    Instance._battleUI.SetLittleHpBarAnchor(battleCharacter.ID, Instance._controllerDic[battleCharacter.ID]);
                    Instance._battleUI.SetLittleHpBarValue(battleCharacter.ID, battleCharacter);
                    Instance._battleUI.SetFloatingNumberPoolAnchor(battleCharacter.ID, Instance._controllerDic[battleCharacter.ID]);
                }
                Instance._battleUI.gameObject.SetActive(true);

                Instance.SortCharacterList(true);

                Instance._battleUI.CharacterListGroupInit(_characterList);

                Instance._arrow = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Other/Arrow"), Vector3.zero, Quaternion.identity);
            }

            public GameObject PlaceCharacter(Vector2Int position, CharacterInfo characterInfo)
            {
                if (_info.NoAttachList.Contains(position))
                {
                    if (!_tempDic.ContainsKey(characterInfo))
                    {
                        GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Character/" + characterInfo.Controller), Vector3.zero, Quaternion.identity);
                        obj.transform.position = new Vector3(position.x, _info.TileInfoDic[position].Height, position.y);
                        _tempDic.Add(characterInfo, obj);
                    }
                    else
                    {
                        //不能和其他角色重疊
                        foreach(KeyValuePair<CharacterInfo, GameObject> pair in _tempDic)
                        {
                            if(pair.Key != characterInfo && Utility.ConvertToVector2Int(pair.Value.transform.position) == position) 
                            {
                                return null;
                            }
                        }

                        _tempDic[characterInfo].SetActive(true);
                        _tempDic[characterInfo].transform.position = new Vector3(position.x, _info.TileInfoDic[position].Height, position.y);
                    }
                    Instance._dragCameraUI.DontDrag = false;

                    return _tempDic[characterInfo];
                }
                else
                {
                    return null;
                }
            }

            public void HideCharacter(CharacterInfo characterInfo) 
            {
                Instance._dragCameraUI.DontDrag = true;
                if (_tempDic.ContainsKey(characterInfo))
                {
                    _tempDic[characterInfo].gameObject.SetActive(false);
                }
            }
        }
    }
}
