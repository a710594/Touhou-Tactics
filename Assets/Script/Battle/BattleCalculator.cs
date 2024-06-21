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
        public List<Vector2Int> GetNormalAreaList(Vector2Int from, Vector2Int to, TargetEnum areaTarget, List<Vector2Int> areaList)
        {
            Vector2 v2 = to - from;
            int angle = (int)Vector2.Angle(v2, Vector2.up) / 90 * 90; //取最接近90的倍數的數
            Vector3 cross = Vector3.Cross(v2, Vector2.up);
            if (cross.z > 0)
            {
                angle = 360 - angle;
            }

            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Matrix4x4 m = Matrix4x4.Rotate(rotation);
            Vector3 v3;
            List<Vector2Int> mulList = new List<Vector2Int>();
            for (int i = 0; i < areaList.Count; i++)
            {
                v3 = m.MultiplyPoint3x4(new Vector3(areaList[i].x, areaList[i].y, 0));
                mulList.Add(new Vector2Int(Mathf.RoundToInt(v3.x), Mathf.RoundToInt(v3.y)));
            }

            Vector2Int position;
            List<Vector2Int> result = new List<Vector2Int>();
            for (int i = 0; i < mulList.Count; i++)
            {
                position = to + mulList[i];
                if (position.x <= Info.MaxX && position.x >= Info.MinY && position.y <= Info.MaxY && position.y >= Info.MinY)
                {
                    result.Add(position);
                }
            }

            RemoveByFaction(areaTarget, result);

            return result;
        }

        public List<Vector2Int> GetTroughAreaList(Vector3 from, Vector3 to)
        {
            List<Vector2Int> list = new List<Vector2Int>();
            Utility.CheckThrough(from, to, Info.TileAttachInfoDic, out bool isBlock, out Vector3 result);
            List<Vector3> line = Utility.DrawLine3D(from, result);
            for (int i = 0; i < line.Count; i++)
            {
                list.Add(Utility.ConvertToVector2Int(line[i]));
            }
            list.Remove(Utility.ConvertToVector2Int(from));

            return list;
        }

        public void RemoveByFaction(TargetEnum target, List<Vector2Int> list) 
        {
            Vector2Int v2;
            for (int i = 0; i < CharacterList.Count; i++)
            {
                v2 = Utility.ConvertToVector2Int(CharacterList[i].Position);
                if (list.Contains(v2))
                {
                    if (target == TargetEnum.None ||
                       (target == TargetEnum.Us && SelectedCharacter.Faction != CharacterList[i].Faction) ||
                       (target == TargetEnum.Them && SelectedCharacter.Faction == CharacterList[i].Faction))
                    {
                        list.Remove(v2);
                        i--;
                    }
                }
            }
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


        public HitType CheckHit(int hit, BattleCharacterInfo user, BattleCharacterInfo target, bool noCritical = false)
        {
            float hitRate = GetHitRate(hit, user, target);

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
                if (!noCritical && (hitRate - 1) * 1f > UnityEngine.Random.Range(0f, 1f))
                {
                    return HitType.Critical;
                }
                else
                {
                    return HitType.Hit;
                }
            }
        }

        public float GetHitRate(int hit, BattleCharacterInfo user, BattleCharacterInfo target) 
        {
            float hitRate = user.DEX * (hit / 100f) / target.AGI;

            //角色互相面對時角度為180,方向相同時角度為0
            //角度越大則命中越低
            //面對面的時候命中率只有一半
            //從背面攻擊時命中率則為1.5倍
            Vector3 v = target.Position - user.Position;
            float angle = Vector2.Angle(new Vector2(v.x, v.z), target.Direction);
            //戰士的被動技能
            if (Passive.Contains<SwordmanPassive>(user.PassiveList) && angle > 90)
            {
                angle = 90;

            }
            hitRate = (angle * (-1 / 180f) + 1.5f) * hitRate;

            return hitRate;
        }

        public int GetDamage(Effect effect, BattleCharacterInfo user, BattleCharacterInfo target)
        {
            int damage = 0;
            if (effect.Type == EffectModel.TypeEnum.PhysicalAttack)
            {
                float atk = (float)user.STR;
                float def = (float)target.CON;
                for (int i = 0; i < CharacterList.Count; i++)
                {
                    for (int j = 0; j < CharacterList[i].StatusList.Count; j++)
                    {
                        if (CharacterList[i].StatusList[j].Type == StatusModel.TypeEnum.STR && user.Faction == CharacterList[i].Faction)
                        {
                            for (int k = 0; k < CharacterList[i].StatusList[j].AreaList.Count; k++)
                            {
                                if (Utility.ConvertToVector2Int(CharacterList[i].Position) + CharacterList[i].StatusList[j].AreaList[k] == Utility.ConvertToVector2Int(user.Position))
                                {
                                    atk *= CharacterList[i].StatusList[j].Value / 100f;
                                }
                            }
                        }
                        else if (CharacterList[i].StatusList[j].Type == StatusModel.TypeEnum.CON && target.Faction == CharacterList[i].Faction)
                        {
                            for (int k = 0; k < CharacterList[i].StatusList[j].AreaList.Count; k++)
                            {
                                if (Utility.ConvertToVector2Int(CharacterList[i].Position) + CharacterList[i].StatusList[j].AreaList[k] == Utility.ConvertToVector2Int(target.Position))
                                {
                                    def *= CharacterList[i].StatusList[j].Value / 100f;
                                }
                            }
                        }
                    }
                }

                damage =  Mathf.RoundToInt(atk / def * effect.Value * (1 + (user.Lv - 1) * 0.1f) + user.Weapon.ATK - target.Armor.DEF);
            }
            else if (effect.Type == EffectModel.TypeEnum.MagicAttack)
            {
                float mtk = (float)user.INT;
                float mef = (float)target.MEN;
                for (int i = 0; i < CharacterList.Count; i++)
                {
                    for (int j = 0; j < CharacterList[i].StatusList.Count; j++)
                    {
                        if (CharacterList[i].StatusList[j].Type == StatusModel.TypeEnum.INT && user.Faction == CharacterList[i].Faction)
                        {
                            for (int k = 0; k < CharacterList[i].StatusList[j].AreaList.Count; k++)
                            {
                                if (Utility.ConvertToVector2Int(CharacterList[i].Position) + CharacterList[i].StatusList[j].AreaList[k] == Utility.ConvertToVector2Int(user.Position))
                                {
                                    mtk *= CharacterList[i].StatusList[j].Value / 100f;
                                }
                            }
                        }
                        else if (CharacterList[i].StatusList[j].Type == StatusModel.TypeEnum.MEN && target.Faction == CharacterList[i].Faction)
                        {
                            for (int k = 0; k < CharacterList[i].StatusList[j].AreaList.Count; k++)
                            {
                                if (Utility.ConvertToVector2Int(CharacterList[i].Position) + CharacterList[i].StatusList[j].AreaList[k] == Utility.ConvertToVector2Int(target.Position))
                                {
                                    mef *= CharacterList[i].StatusList[j].Value / 100f;
                                }
                            }
                        }
                    }
                }

                damage = Mathf.RoundToInt(mtk / mef * effect.Value *(1 + (user.Lv - 1) * 0.1f) + user.Weapon.MTK - target.Armor.MEF);
            }
            else
            {
                damage = 0;
            }

            //各種和被動技能相關的計算
            if(damage > 0) 
            {
                if (Passive.Contains<MagicianPassive>(user.PassiveList))
                {
                    damage = Mathf.RoundToInt(damage * MagicianPassive.GetValue(user));
                }
                if (Passive.Contains<PhoenixPassive>(user.PassiveList))
                {
                    damage = Mathf.RoundToInt(damage * PhoenixPassive.GetValue(user));
                }
                if (Passive.Contains<DreamEaterPassive>(user.PassiveList))
                {
                    if (target.IsSleep())
                    {
                        damage *= 2;
                    }
                }
            }
            else 
            {
                damage = 0;
            }

            return damage;
        }

        public int GetPredictionHp(BattleCharacterInfo user, BattleCharacterInfo target, int targetCurrentHp, Effect effect)
        {
            int prediction = targetCurrentHp;

            if (effect.Type == EffectModel.TypeEnum.PhysicalAttack || effect.Type == EffectModel.TypeEnum.MagicAttack)
            {
                prediction = targetCurrentHp - GetDamage(effect, user, target);
            }
            else if (effect.Type == EffectModel.TypeEnum.Recover) 
            {
                prediction = targetCurrentHp + Mathf.RoundToInt((float)effect.Value * (float)user.MEN / 100f);
            }
            else if(effect.Type == EffectModel.TypeEnum.Medicine) 
            {
                prediction = targetCurrentHp + effect.Value;
            }

            if (prediction < 0)
            {
                prediction = 0;
            }
            else if (prediction > target.MaxHP)
            {
                prediction = target.MaxHP;
            }

            if (effect.SubEffect != null)
            {
                return GetPredictionHp(user, target, prediction, effect.SubEffect);
            }
            else
            {
                return prediction;
            }
        }

        public Vector3 RandomCharacterPosition(BattleCharacterInfo.FactionEnum faction) 
        {
            bool isRepeat = false;
            bool hasPath = false;
            Vector2Int v2;
            Vector3 v3 = new Vector3();
            List<Vector2Int> path;

            while (true) 
            {
                v2 = new Vector2Int(Mathf.RoundToInt(Utility.RandomGaussian(Info.MinX, Info.MaxX)), Mathf.RoundToInt(Utility.RandomGaussian(Info.MinY, Info.MaxY)));
                if(Info.TileAttachInfoDic[v2].MoveCost > 0) 
                {
                    //檢查是否為保留區
                    if (Info.NoAttachList.Contains(v2)) 
                    {
                        continue;
                    }

                    //檢查位置是否有和其他角色重複
                    isRepeat = false;
                    for (int i = 0; i < CharacterList.Count; i++)
                    {
                        if (v2 == Utility.ConvertToVector2Int(CharacterList[i].Position))
                        {
                            isRepeat = true;
                            break;
                        }
                    }

                    if (!isRepeat)
                    {
                        //檢查是否與其他角色之間有路徑
                        for (int i = 0; i < Info.NoAttachList.Count; i++)
                        {
                            path = GetPath(v2, Utility.ConvertToVector2Int(CharacterList[i].Position), CharacterList[i].Faction);
                            if (path != null)
                            {
                                v3 = new Vector3(v2.x, Info.TileAttachInfoDic[v2].Height, v2.y);
                                hasPath = true;
                                break;
                            }
                        }
                        if (hasPath)
                        {
                            break;
                        }
                    }
                }
            }

            return v3;
        }

        public bool HasCharacter(Vector2Int start, Vector2Int target) 
        {
            if (start == target) //自己的位置不算
            {
                return false;
            }

            bool hasCharacter = false;

            for (int i=0; i<CharacterList.Count; i++) 
            {
            if(Utility.ConvertToVector2Int(CharacterList[i].Position) == target) 
                {
                    hasCharacter = true;
                    break;
                }
            }

            return hasCharacter;
        }
    }
}