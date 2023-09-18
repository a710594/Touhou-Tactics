using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public partial class BattleController
    {
        private class PrepareState : BattleControllerState 
        {
            public PrepareState(StateContext context) : base(context)
            {
            }

            public override void Begin(object obj)
            {
                _info = Instance.BattleInfo;
                Camera.main.transform.position = new Vector3(_info.NoAttachList[0].x-10, 10, _info.NoAttachList[0].y-10);
            }

            public void PlaceCharacter(Vector2Int position, CharacterInfo characterInfo)
            {

                GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Character/" + characterInfo.Controller), Vector3.zero, Quaternion.identity);
                obj.transform.position = new Vector3(position.x, _info.TileInfoDic[position].Height, position.y);
                
            }
        }
    }
}
