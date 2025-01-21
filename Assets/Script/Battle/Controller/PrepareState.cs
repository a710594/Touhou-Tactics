using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public partial class BattleController
    {
        public class PrepareState : BattleControllerState 
        {
            private Dictionary<int, BattleCharacterController> _tempDic = new Dictionary<int, BattleCharacterController>(); //<JobID, BattleCharacterController>

            public PrepareState(StateContext context) : base(context)
            {
                _characterList = Instance.CharacterList;
            }

            public override void Begin()
            {
                Camera.main.transform.localPosition = new Vector3(-10, 10, -10);

                Instance.SelectBattleCharacterUI.Init(Instance.MinPlayerCount, Instance.MaxPlayerCount, Instance._candidateList);

                for (int i=0; i< Instance.PlayerPositionList.Count; i++) 
                {
                    Instance.TileDic[Instance.PlayerPositionList[i]].TileObject.Quad.gameObject.SetActive(true);
                }

                if(Instance.PrepareStateBeginHandler!=null)
                {
                    Instance.PrepareStateBeginHandler();
                }
            }

            public override void End()
            {
                foreach (KeyValuePair<int, BattleCharacterController> pair in _tempDic)
                {
                    Instance._maxIndex++;
                    pair.Value.Info.Index = Instance._maxIndex;
                    _characterList.Add(pair.Value);

                    pair.Value.MoveEndHandler += Instance.OnMoveEnd;
                    Instance.BattleUI.SetLittleHpBarAnchor(pair.Value);
                    Instance.BattleUI.SetLittleHpBarValue(pair.Value);
                    Instance.BattleUI.SetFloatingNumberPoolAnchor(pair.Value);
                }
                Instance.BattleUI.gameObject.SetActive(true);
                Instance.SelectBattleCharacterUI.gameObject.SetActive(false);

                Instance.SortCharacterList(true);
                Instance.CharacterListGroupInit();
            }

            public BattleCharacterController PlaceCharacter(Vector2Int position, CharacterInfo info)
            {
                if (Instance.PlayerPositionList.Contains(position))
                {
                    //????M??L?????|
                    foreach (KeyValuePair<int, BattleCharacterController> pair in _tempDic)
                    {
                        if (pair.Key != info.JobId && Utility.ConvertToVector2Int(pair.Value.transform.position) == position)
                        {
                            return null;
                        }
                    }

                    if (!_tempDic.ContainsKey(info.JobId))
                    {
                        BattleCharacterController character = ((GameObject)GameObject.Instantiate(Resources.Load("Prefab/Character/Player/" + info.Controller), Vector3.zero, Quaternion.identity)).GetComponent<BattleCharacterController>();
                        character.transform.position = new Vector3(position.x, Instance.TileDic[position].TileData.Height, position.y);
                        character.transform.SetParent(Instance._root);
                        JobModel job = DataContext.Instance.JobDic[info.JobId];
                        character.Init(CharacterManager.Instance.Info.Lv, job);
                        character.SetAngle();
                        _tempDic.Add(info.JobId, character);
                    }
                    else
                    {
                        _tempDic[info.JobId].gameObject.SetActive(true);
                        _tempDic[info.JobId].transform.position = new Vector3(position.x, Instance.TileDic[position].TileData.Height, position.y);
                    }
                    Instance.DragCameraUI.DontDrag = false;

                    return _tempDic[info.JobId];
                }
                else
                {
                    return null;
                }
            }

            public void SetCharacterSpriteVisible(CharacterInfo characterInfo, bool isVisible) 
            {
                Instance.DragCameraUI.DontDrag = !isVisible;
                if (_tempDic.ContainsKey(characterInfo.JobId))
                {
                    _tempDic[characterInfo.JobId].gameObject.SetActive(isVisible);
                }
            }

            public void RemoveCharacterSprite(int jobId)
            {
                Instance.DragCameraUI.DontDrag = false;
                GameObject.Destroy(_tempDic[jobId].gameObject);
                _tempDic.Remove(jobId);
            }
        }
    }
}
