using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public partial class BattleController
    {
        private class PrepareState : BattleControllerState 
        {
            private Dictionary<CharacterInfo, GameObject> _tempList = new Dictionary<CharacterInfo, GameObject>();

            public PrepareState(StateContext context) : base(context)
            {
            }

            public override void Begin(object obj)
            {
                _info = Instance.Info;
                Camera.main.transform.position = new Vector3(_info.NoAttachList[0].x-10, 10, _info.NoAttachList[0].y-10);
            }

            public GameObject PlaceCharacter(Vector2Int position, CharacterInfo characterInfo)
            {
                if (!_tempList.ContainsKey(characterInfo))
                {
                    GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Character/" + characterInfo.Controller), Vector3.zero, Quaternion.identity);
                    obj.transform.position = new Vector3(position.x, _info.TileInfoDic[position].Height, position.y);
                    _tempList.Add(characterInfo, obj);
                }
                else
                {
                    _tempList[characterInfo].SetActive(true);
                    _tempList[characterInfo].transform.position = new Vector3(position.x, _info.TileInfoDic[position].Height, position.y);
                }
                Instance._dragCameraUI.DontDrag = false;

                return _tempList[characterInfo];
            }

            public void HideCharacter(CharacterInfo characterInfo) 
            {
                Instance._dragCameraUI.DontDrag = true;
                if (_tempList.ContainsKey(characterInfo))
                {
                    _tempList[characterInfo].gameObject.SetActive(false);
                }
            }
        }
    }
}
