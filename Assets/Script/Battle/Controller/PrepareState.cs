using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public partial class BattleController
    {
        public class PrepareState : BattleControllerState 
        {
            public PrepareState(StateContext context) : base(context)
            {
                Instance.TempList.Clear();
            }

            public override void Begin()
            {
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
                for(int i=0; i<Instance.TempList.Count; i++)
                {
                    Instance._maxIndex++;
                    Instance.TempList[i].Index = Instance._maxIndex;
                    Instance.TempList[i].Outline.OutlineColor = Color.blue;
                    Instance.CharacterList.Add(Instance.TempList[i]);

                    Instance.TempList[i].MoveEndHandler += Instance.OnMoveEnd;
                    Instance.BattleUI.CharacterListGroup.Add(Instance.TempList[i]);
                    Instance.BattleUI.SetLittleHpBarAnchor(Instance.TempList[i]);
                    Instance.BattleUI.SetLittleHpBarValue(Instance.TempList[i]);
                    Instance.BattleUI.SetFloatingNumberPoolAnchor(Instance.TempList[i]);
                }
                Instance.BattleUI.gameObject.SetActive(true);
                Instance.SelectBattleCharacterUI.gameObject.SetActive(false);

                Instance.SortCharacterList(true);
            }

            public bool PlaceCharacter(BattleCharacterController character)
            {
                Vector2Int position = Utility.ConvertToVector2Int(character.transform.position);
                if (Instance.PlayerPositionList.Contains(position))
                {
                    for (int i = 0; i < Instance.TempList.Count; i++)
                    {
                        if (Utility.ConvertToVector2Int(Instance.TempList[i].transform.position) == position)
                        {
                            GameObject.Destroy(character.gameObject);
                            return false;
                        }
                    }
                    Instance.TempList.Add(character);
                    return true;
                }
                else
                {
                    GameObject.Destroy(character.gameObject);
                    return false;
                }
            }
        }
    }
}
