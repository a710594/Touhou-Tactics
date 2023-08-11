using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Battle
{
    public partial class BattleController
    {
        public enum HitType
        {
            Miss,
            Hit,
            Critical,
            NoDamage,
        }

        public static List<Vector2Int> GetNormalAreaList(int width, int height, Effect effect, Vector2Int center)
        {
            Vector2Int position;
            List<Vector2Int> list = new List<Vector2Int>();
            for (int i = 0; i < effect.Data.AreaList.Count; i++)
            {
                position = center + effect.Data.AreaList[i];
                if (position.x < width && position.x >= 0 && position.y < height && position.y >= 0)
                {
                    list.Add(position);
                }
            }
            return list;
        }

        public List<Vector2Int> GetTroughAreaList(Vector3 from, Vector3 to)
        {
            List<Vector2Int> list = new List<Vector2Int>();
            CheckThrough(from, to, out bool isBlock, out Vector3 result);
            List<Vector3> line = Utility.DrawLine3D(from, result);
            for (int i = 0; i < line.Count; i++)
            {
                list.Add(Utility.ConvertToVector2Int(line[i]));
            }
            list.Remove(Utility.ConvertToVector2Int(from));

            return list;
        }

        public static bool CheckEffectArea(List<Vector2Int> list, Vector2Int target)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (target == list[i])
                {
                    return true;
                }
            }
            return false;
        }


        public static HitType CheckHit(Effect effect, BattleCharacterInfo user, BattleCharacterInfo target)
        {
            float hitRate = user.SEN * (effect.Data.Hit / 100f) / target.AGI;

            //角色互相面對時較度為180,方向相同時角度為0
            //角度越大則命中越低
            //面對面的時候命中率只有一半
            //從背面攻擊時命中率則為1.5倍
            Vector3 v = target.Position - user.Position;
            float angle = Vector2.Angle(new Vector2(v.x, v.z), target.Direction);
            if (Passive.Contains<FaceToFacePassive>(user.PassiveList) && angle > 90)
            {
                angle = 90;
            
            }
            hitRate = (angle * (-1 / 180f) + 1.5f) * hitRate;

            if (hitRate <= 1)
            {
                if (hitRate > UnityEngine.Random.Range(0f, 1f))
                {
                    return HitType.Hit;
                }
                else
                {
                    return HitType.Miss;
                }
            }
            else
            {
                if ((hitRate - 1) * 0.2f > UnityEngine.Random.Range(0f, 1f))
                {
                    return HitType.Critical;
                }
                else
                {
                    return HitType.Hit;
                }
            }
        }

        public static int GetDamage(Effect effect, BattleCharacterInfo user, BattleCharacterInfo target, List<BattleCharacterInfo> characterList)
        {
            if (effect.Data.Type == EffectModel.TypeEnum.PhysicalAttack)
            {
                float atk = (float)user.ATK;
                float def = (float)target.DEF;
                for (int i = 0; i < characterList.Count; i++)
                {
                    for (int j = 0; j < characterList[i].StatusList.Count; j++)
                    {
                        if (characterList[i].StatusList[j].Data.Type == StatusModel.TypeEnum.ATK && user.Faction == characterList[i].Faction)
                        {
                            for (int k = 0; k < characterList[i].StatusList[j].Data.AreaList.Count; k++)
                            {
                                if (Utility.ConvertToVector2(characterList[i].Position) + characterList[i].StatusList[j].Data.AreaList[k] == Utility.ConvertToVector2(user.Position))
                                {
                                    atk *= characterList[i].StatusList[j].Data.Value / 100f;
                                }
                            }
                        }
                        else if (characterList[i].StatusList[j].Data.Type == StatusModel.TypeEnum.DEF && target.Faction == characterList[i].Faction)
                        {
                            for (int k = 0; k < characterList[i].StatusList[j].Data.AreaList.Count; k++)
                            {
                                if (Utility.ConvertToVector2(characterList[i].Position) + characterList[i].StatusList[j].Data.AreaList[k] == Utility.ConvertToVector2(target.Position))
                                {
                                    def *= characterList[i].StatusList[j].Data.Value / 100f;
                                }
                            }
                        }
                    }
                }

                return Mathf.RoundToInt(atk / def * effect.Data.Value);
            }
            else if (effect.Data.Type == EffectModel.TypeEnum.MagicAttack)
            {
                float mtk = (float)user.MTK;
                float mef = (float)target.MEF;
                for (int i = 0; i < characterList.Count; i++)
                {
                    for (int j = 0; j < characterList[i].StatusList.Count; j++)
                    {
                        if (characterList[i].StatusList[j].Data.Type == StatusModel.TypeEnum.MTK && user.Faction == characterList[i].Faction)
                        {
                            for (int k = 0; k < characterList[i].StatusList[j].Data.AreaList.Count; k++)
                            {
                                if (Utility.ConvertToVector2(characterList[i].Position) + characterList[i].StatusList[j].Data.AreaList[k] == Utility.ConvertToVector2(user.Position))
                                {
                                    mtk *= characterList[i].StatusList[j].Data.Value / 100f;
                                }
                            }
                        }
                        else if (characterList[i].StatusList[j].Data.Type == StatusModel.TypeEnum.MEF && target.Faction == characterList[i].Faction)
                        {
                            for (int k = 0; k < characterList[i].StatusList[j].Data.AreaList.Count; k++)
                            {
                                if (Utility.ConvertToVector2(characterList[i].Position) + characterList[i].StatusList[j].Data.AreaList[k] == Utility.ConvertToVector2(target.Position))
                                {
                                    mef *= characterList[i].StatusList[j].Data.Value / 100f;
                                }
                            }
                        }
                    }
                }

                return Mathf.RoundToInt(mtk / mef * effect.Data.Value);
            }
            else
            {
                return -1;
            }
        }

        public static int GetPredictionHp(int targetCurrentHp, Effect effect, BattleCharacterInfo user, BattleCharacterInfo target, List<BattleCharacterInfo> characterList)
        {
            int prediction = targetCurrentHp;

            if (effect.Data.Type == EffectModel.TypeEnum.PhysicalAttack || effect.Data.Type == EffectModel.TypeEnum.MagicAttack)
            {
                prediction = targetCurrentHp - GetDamage(effect, user, target, characterList);
                if (prediction < 0)
                {
                    prediction = 0;
                }
                else if (prediction > target.MaxHP)
                {
                    prediction = target.MaxHP;
                }
            }

            if (effect.SubEffect != null)
            {
                return GetPredictionHp(prediction, effect.SubEffect, user, target, characterList);
            }
            else
            {
                return prediction;
            }
        }

        public static void CheckLine(Vector3 from, Vector3 to, List<BattleCharacterInfo> characterList, Dictionary<Vector2Int, TileInfo> tileDic, out bool isBlock, out Vector3 result)
        {
            isBlock = false;
            int height;
            Vector2Int position;
            List<Vector3> list = Utility.DrawLine3D(from, to);
            for (int i = 0; i < list.Count; i++)
            {
                position = Utility.ConvertToVector2Int(list[i]);
                if (tileDic.ContainsKey(position))
                {
                    height = tileDic[position].Height;
                    if (tileDic[position].AttachID != null)
                    {
                        height += DataContext.Instance.AttachScriptableObjectDic[tileDic[position].AttachID].Height;
                    }

                    for (int j = 0; j < characterList.Count; j++)
                    {
                        if (Utility.ConvertToVector2Int(from) != Utility.ConvertToVector2Int(characterList[j].Position) && Utility.ConvertToVector2Int(to) != Utility.ConvertToVector2Int(characterList[j].Position) && position == Utility.ConvertToVector2Int(characterList[j].Position))
                        {
                            height++;
                        }
                    }

                    if (height > list[i].y)
                    {
                        isBlock = true;
                        result = list[i];
                        return;
                    }
                }
            }

            result = to;
        }

        //和 CheckLine 相似,但是無視 attach 和 character
        public void CheckThrough(Vector3 from, Vector3 to, out bool isBlock, out Vector3 result)
        {
            isBlock = false;
            int height;
            Vector2Int position;
            List<Vector3> list = Utility.DrawLine3D(from, to);
            Dictionary<Vector2Int, TileInfo> tileDic = BattleInfo.TileInfoDic;
            for (int i = 0; i < list.Count; i++)
            {
                position = Utility.ConvertToVector2Int(list[i]);
                if (tileDic.ContainsKey(position))
                {
                    height = tileDic[position].Height;

                    if (height > list[i].y)
                    {
                        isBlock = true;
                        result = list[i];
                        return;
                    }
                }
            }

            result = to;
        }

        public static void CheckParabola(Vector3 from, Vector3 to, int parabolaHeight, Dictionary<Vector2Int, TileInfo> tileDic, out bool isBlock, out List<Vector3> result)
        {
            isBlock = false;
            result = new List<Vector3>();
            int height;
            Vector2Int position;
            List<Vector3> list = Utility.DrawParabola(from, to, parabolaHeight, true);
            for (int i = 0; i < list.Count; i++)
            {
                position = Utility.ConvertToVector2Int(list[i]);
                if (tileDic.ContainsKey(position))
                {
                    height = tileDic[position].Height;
                    if (tileDic[position].AttachID != null)
                    {
                        height += DataContext.Instance.AttachScriptableObjectDic[tileDic[position].AttachID].Height;
                    }

                    if (height > list[i].y)
                    {
                        isBlock = true;
                        to = list[i];
                        break;
                    }
                }
            }

            result = Utility.DrawParabola(from, to, parabolaHeight, false);
        }

        public Vector3 RandomCharacterPosition(BattleCharacterInfo.FactionEnum faction) 
        {
            bool isBreak = false;
            Vector2Int v2;
            Vector3 v3 = new Vector3();
            List<Vector2Int> path;

            while (true) 
            {
                v2 = new Vector2Int(UnityEngine.Random.Range(0, BattleInfo.Width), UnityEngine.Random.Range(0, BattleInfo.Height));
                if(BattleInfo.TileInfoDic[v2].MoveCost > 0) 
                {
                    for (int i = 0; i < CharacterList.Count; i++)
                    {
                        if (faction != CharacterList[i].Faction) //與自己的陣營不同的角色
                        {
                            path = AStarAlgor.Instance.GetPath(v2, Utility.ConvertToVector2Int(CharacterList[i].Position), CharacterList[i], CharacterList, BattleInfo.TileInfoDic, false);
                            if(path != null) 
                            {
                                v3 = new Vector3(v2.x, BattleInfo.TileInfoDic[v2].Height, v2.y);
                                isBreak = true;
                                break;
                            }
                        }
                    }
                    if (isBreak) 
                    {
                        break;
                    }
                }
            }

            return v3;
        }
    }
}