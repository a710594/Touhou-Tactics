using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class BattleCalculator
{
    public enum HitType
    {
        Miss,
        Hit,
        Critical,
        NoDamage,
    }

    public static  List<Vector2> GetNormalAreaList(Effect effect, Vector2 center) 
    {
        List<Vector2> list = new List<Vector2>();
        for (int i = 0; i < effect.Data.AreaList.Count; i++)
        {
            list.Add(center + effect.Data.AreaList[i]);
        }
        return list;
    }

    public static List<Vector2> GetTroughAreaList(Vector3 from, Vector3 to, Dictionary<Vector2, TileInfo> tileDic)
    {
        List<Vector2> list = new List<Vector2>();
        CheckLine(from, to, tileDic, out bool isBlock, out Vector3 result);
        List<Vector3> line = Utility.DrawLine3D(from, result);
        for (int i = 0; i < line.Count; i++)
        {
            list.Add(Utility.ConvertToVector2(line[i]));
        }
        list.Remove(Utility.ConvertToVector2(from));
        return list;
    }

    public static bool CheckEffectArea(List<Vector2> list, Vector2 target)
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

    //public static void SetEffect(Effect effect, BattleCharacterInfo user, BattleCharacterInfo target, List<FloatingNumberData> list) 
    //{
    //    FloatingNumberData floatingNumberData;
    //    HitType hitType = CheckHit(effect, user, target);
    //    if(effect.Data.Type == EffectModel.TypeEnum.PhysicalAttack || effect.Data.Type == EffectModel.TypeEnum.MagicAttack) 
    //    {
    //        if (hitType != HitType.Miss)
    //        {
    //            int damage = SetDamage(effect, user, target, hitType);
    //            if (hitType == HitType.Hit)
    //            {
    //                floatingNumberData = new FloatingNumberData(FloatingNumberData.TypeEnum.Damage, damage.ToString());
    //            }
    //            else
    //            {
    //                floatingNumberData = new FloatingNumberData(FloatingNumberData.TypeEnum.Critical, damage.ToString());
    //            }
    //        }
    //        else
    //        {
    //            floatingNumberData = new FloatingNumberData(FloatingNumberData.TypeEnum.Miss, "Miss");
    //        }
    //        list.Add(floatingNumberData);
    //    }

    //    if (effect.SubEffect != null && hitType != HitType.Miss)
    //    {
    //        SetEffect(effect.SubEffect, user, target, list);
    //    }
    //}


    public static HitType CheckHit(Effect effect, BattleCharacterInfo user, BattleCharacterInfo target)
    {
        float hitRate = user.SEN * (effect.Data.Hit / 100f) / target.AGI;
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
            for (int i=0; i<characterList.Count; i++) 
            {
                for (int j = 0; j < characterList[i].StatusList.Count; j++)
                {
                    if (characterList[i].StatusList[j].Data.Type == StatusModel.TypeEnum.ATK && user.Faction == characterList[i].Faction)
                    {
                        for (int k=0; k< characterList[i].StatusList[j].Data.AreaList.Count; k++) 
                        {
                            if(Utility.ConvertToVector2(characterList[i].Position) + characterList[i].StatusList[j].Data.AreaList[k] == Utility.ConvertToVector2(user.Position)) 
                            {
                                atk *= characterList[i].StatusList[j].Data.Value / 100f;
                            }
                        }
                    }
                    else if(characterList[i].StatusList[j].Data.Type == StatusModel.TypeEnum.DEF && target.Faction == characterList[i].Faction) 
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
            return GetPredictionHp(prediction, effect, user, target, characterList);
        }
        else
        {
            return prediction;
        }
    }

    public static void CheckLine(Vector3 from, Vector3 to, Dictionary<Vector2, TileInfo> tileDic, out bool isBlock, out Vector3 result) 
    {
        isBlock = false;
        int height;
        Vector2 position;
        List<Vector3> list = Utility.DrawLine3D(from, to);
        for(int i=0; i<list.Count; i++) 
        {
            position = new Vector2(list[i].x, list[i].z);
            if (tileDic.ContainsKey(position)) 
            {
                height = tileDic[position].Height;
                if(tileDic[position].AttachID != null) 
                {
                    height += DataContext.Instance.AttachScriptableObjectDic[tileDic[position].AttachID].Height;
                }

                if(height> list[i].y) 
                {
                    isBlock = true;
                    result = list[i];
                    return;
                }
            }
        }

        result = to;
    }

    public static void CheckParabola(Vector3 from, Vector3 to, int parabolaHeight, Dictionary<Vector2, TileInfo> tileDic, out bool isBlock, out List<Vector3> result)
    {
        isBlock = false;
        result = new List<Vector3>();
        int height;
        Vector2 position;
        List<Vector3> list = Utility.DrawParabola(from, to, parabolaHeight);
        for (int i = 0; i < list.Count; i++)
        {
            position = new Vector2(list[i].x, list[i].z);
            if (tileDic.ContainsKey(position))
            {
                result.Add(list[i]);

                height = tileDic[position].Height;
                if (tileDic[position].AttachID != null)
                {
                    height += DataContext.Instance.AttachScriptableObjectDic[tileDic[position].AttachID].Height;
                }

                if (height > list[i].y)
                {
                    isBlock = true;
                    return;
                }
            }
        }
    }
}
