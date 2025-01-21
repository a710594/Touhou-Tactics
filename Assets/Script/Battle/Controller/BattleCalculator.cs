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
        /*
         取得一個範圍內的座標,例如如果range=2,就會像這樣
              x
             xxx
            xxxxx
             xxx
              x
        */
        public List<Vector2Int> GetRange(int range, Vector2Int start)
        {
            Vector2Int position;
            List<Vector2Int> list = new List<Vector2Int>();

            //range == -1 代表射程無限
            if (range == -1)
            {
                foreach(KeyValuePair<Vector2Int, BattleInfoTile> pair in TileDic) 
                {
                    list.Add(pair.Key);
                }
            }
            else
            {
                //BFS
                Queue<Vector2Int> queue = new Queue<Vector2Int>();
                queue.Enqueue(start);
                while (queue.Count != 0)
                {
                    position = queue.Dequeue();
                    if (!list.Contains(position))
                    {
                        list.Add(position);
                    }

                    if (!list.Contains(position + Vector2Int.up) && TileDic.ContainsKey(position + Vector2Int.up) && Utility.ManhattanDistance(position + Vector2Int.up, start) <= range)
                    {
                        queue.Enqueue(position + Vector2Int.up);
                    }
                    if (!list.Contains(position + Vector2Int.down) && TileDic.ContainsKey(position + Vector2Int.down) && Utility.ManhattanDistance(position + Vector2Int.down, start) <= range)
                    {
                        queue.Enqueue(position + Vector2Int.down);
                    }
                    if (!list.Contains(position + Vector2Int.left) && TileDic.ContainsKey(position + Vector2Int.left) && Utility.ManhattanDistance(position + Vector2Int.left, start) <= range)
                    {
                        queue.Enqueue(position + Vector2Int.left);
                    }
                    if (!list.Contains(position + Vector2Int.right) && TileDic.ContainsKey(position + Vector2Int.right) && Utility.ManhattanDistance(position + Vector2Int.right, start) <= range)
                    {
                        queue.Enqueue(position + Vector2Int.right);
                    }
                }
            }

            return list;
        }

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
                if (TileDic.ContainsKey(position))
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
            Utility.CheckThrough(from, to, TileDic, out bool isBlock, out Vector3 result);
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
                v2 = Utility.ConvertToVector2Int(CharacterList[i].transform.position);
                if (list.Contains(v2))
                {
                    if (target == TargetEnum.None ||
                       (target == TargetEnum.Us && SelectedCharacter.Info.Faction != CharacterList[i].Info.Faction) ||
                       (target == TargetEnum.Them && SelectedCharacter.Info.Faction == CharacterList[i].Info.Faction))
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


        public HitType CheckHit(int hit, BattleCharacterController user, BattleCharacterController target, bool noCritical = false)
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

        public float GetHitRate(int hit, BattleCharacterController user, BattleCharacterController target) 
        {
            int decorationDex = 0;
            for (int i = 0; i < user.Info.Decoration.Count; i++)
            {
                decorationDex += user.Info.Decoration[i].DEX;
            }
            int userDEX = user.Info.DEX + decorationDex;

            int decorationAGI = 0;
            for (int i = 0; i < target.Info.Decoration.Count; i++)
            {
                decorationAGI += target.Info.Decoration[i].AGI;
            }
            int targetAGI = target.Info.AGI + decorationAGI;

            float hitRate = userDEX * (hit / 100f) / targetAGI;

            //角色互相面對時角度為180,方向相同時角度為0
            //角度越大則命中越低
            //面對面的時候命中率只有一半
            //從背面攻擊時命中率則為1.5倍
            Vector3 v = target.transform.position - user.transform.position;
            float angle = Vector2.Angle(new Vector2(v.x, v.z), target.Direction);
            //戰士的被動技能
            if (Passive.Contains<SwordmanPassive>(user.Info.PassiveList) && angle > 90)
            {
                angle = 90;

            }
            hitRate = (angle * (-1 / 180f) + 1.5f) * hitRate;

            return hitRate;
        }

        public int GetDamage(Effect effect, BattleCharacterController user, BattleCharacterController target)
        {
            int damage = 0;
            if (effect.Type == EffectModel.TypeEnum.PhysicalAttack)
            {
                float atk = (float)user.Info.STR;
                float def = (float)target.Info.CON;
                for (int i = 0; i < CharacterList.Count; i++)
                {
                    for (int j = 0; j < CharacterList[i].Info.StatusList.Count; j++)
                    {
                        if (CharacterList[i].Info.StatusList[j].Type == StatusModel.TypeEnum.STR && user.Info.Faction == CharacterList[i].Info.Faction)
                        {
                            for (int k = 0; k < CharacterList[i].Info.StatusList[j].AreaList.Count; k++)
                            {
                                if (Utility.ConvertToVector2Int(CharacterList[i].transform.position) + CharacterList[i].Info.StatusList[j].AreaList[k] == Utility.ConvertToVector2Int(user.transform.position))
                                {
                                    atk *= CharacterList[i].Info.StatusList[j].Value / 100f;
                                }
                            }
                        }
                        else if (CharacterList[i].Info.StatusList[j].Type == StatusModel.TypeEnum.CON && target.Info.Faction == CharacterList[i].Info.Faction)
                        {
                            for (int k = 0; k < CharacterList[i].Info.StatusList[j].AreaList.Count; k++)
                            {
                                if (Utility.ConvertToVector2Int(CharacterList[i].transform.position) + CharacterList[i].Info.StatusList[j].AreaList[k] == Utility.ConvertToVector2Int(target.transform.position))
                                {
                                    def *= CharacterList[i].Info.StatusList[j].Value / 100f;
                                }
                            }
                        }
                    }
                }

                int armorDef = 0;
                for (int i=0; i<target.Info.Armor.Count; i++) 
                {
                    armorDef += target.Info.Armor[i].DEF;
                }

                damage =  Mathf.RoundToInt(atk / def * effect.Value * (1 + (user.Info.Lv - 1) * 0.1f) + user.Info.Weapon.ATK - armorDef);
            }
            else if (effect.Type == EffectModel.TypeEnum.MagicAttack)
            {
                float mtk = (float)user.Info.INT;
                float mef = (float)target.Info.MEN;
                for (int i = 0; i < CharacterList.Count; i++)
                {
                    for (int j = 0; j < CharacterList[i].Info.StatusList.Count; j++)
                    {
                        if (CharacterList[i].Info.StatusList[j].Type == StatusModel.TypeEnum.INT && user.Info.Faction == CharacterList[i].Info.Faction)
                        {
                            for (int k = 0; k < CharacterList[i].Info.StatusList[j].AreaList.Count; k++)
                            {
                                if (Utility.ConvertToVector2Int(CharacterList[i].transform.position) + CharacterList[i].Info.StatusList[j].AreaList[k] == Utility.ConvertToVector2Int(user.transform.position))
                                {
                                    mtk *= CharacterList[i].Info.StatusList[j].Value / 100f;
                                }
                            }
                        }
                        else if (CharacterList[i].Info.StatusList[j].Type == StatusModel.TypeEnum.MEN && target.Info.Faction == CharacterList[i].Info.Faction)
                        {
                            for (int k = 0; k < CharacterList[i].Info.StatusList[j].AreaList.Count; k++)
                            {
                                if (Utility.ConvertToVector2Int(CharacterList[i].transform.position) + CharacterList[i].Info.StatusList[j].AreaList[k] == Utility.ConvertToVector2Int(target.transform.position))
                                {
                                    mef *= CharacterList[i].Info.StatusList[j].Value / 100f;
                                }
                            }
                        }
                    }
                }

                int armorMef = 0;
                for (int i = 0; i < target.Info.Armor.Count; i++)
                {
                    armorMef += target.Info.Armor[i].MEF;
                }

                damage = Mathf.RoundToInt(mtk / mef * effect.Value *(1 + (user.Info.Lv - 1) * 0.1f) + user.Info.Weapon.MTK - armorMef);
            }
            else
            {
                damage = 0;
            }

            //各種和被動技能相關的計算
            if(damage > 0) 
            {
                if (Passive.Contains<MagicianPassive>(user.Info.PassiveList))
                {
                    damage = Mathf.RoundToInt(damage * MagicianPassive.GetValue(user.Info));
                }
                if (Passive.Contains<PhoenixPassive>(user.Info.PassiveList))
                {
                    damage = Mathf.RoundToInt(damage * PhoenixPassive.GetValue(user.Info));
                }
                if (Passive.Contains<DreamEaterPassive>(user.Info.PassiveList))
                {
                    if (target.Info.IsSleep())
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

        public int GetPredictionHp(BattleCharacterController user, BattleCharacterController target, int targetCurrentHp, Effect effect)
        {
            int prediction = targetCurrentHp;

            if (effect.Type == EffectModel.TypeEnum.PhysicalAttack || effect.Type == EffectModel.TypeEnum.MagicAttack)
            {
                prediction = targetCurrentHp - GetDamage(effect, user, target);
            }
            else if (effect.Type == EffectModel.TypeEnum.Recover) 
            {
                prediction = targetCurrentHp + Mathf.RoundToInt((float)effect.Value * (float)user.Info.MEN / 100f);
            }
            else if(effect.Type == EffectModel.TypeEnum.Medicine) 
            {
                prediction = targetCurrentHp + effect.Value;
            }

            if (prediction < 0)
            {
                prediction = 0;
            }
            else if (prediction > target.Info.MaxHP)
            {
                prediction = target.Info.MaxHP;
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

        public Vector3 RandomCharacterPosition(BattleCharacterControllerData.FactionEnum faction) 
        {
            bool isRepeat = false;
            bool hasPath = false;
            Vector2Int v2;
            Vector3 v3 = new Vector3();
            List<Vector2Int> path;

            while (true) 
            {
                //常態分佈
                //v2 = new Vector2Int(Mathf.RoundToInt(Utility.RandomGaussian(Info.MinX, Info.MaxX)), Mathf.RoundToInt(Utility.RandomGaussian(Info.MinY, Info.MaxY)));
                List<Vector2Int> list = new List<Vector2Int>(TileDic.Keys);
                v2 = list[UnityEngine.Random.Range(0, list.Count)];
                if(TileDic[v2].MoveCost > 0) 
                {
                    //檢查是否為保留區
                    if (PlayerPositionList.Contains(v2)) 
                    {
                        continue;
                    }

                    //檢查位置是否有和其他角色重複
                    isRepeat = false;
                    for (int i = 0; i < CharacterList.Count; i++)
                    {
                        if (v2 == Utility.ConvertToVector2Int(CharacterList[i].transform.position))
                        {
                            isRepeat = true;
                            break;
                        }
                    }

                    if (!isRepeat)
                    {
                        //檢查是否與其他角色之間有路徑
                        for (int i = 0; i < PlayerPositionList.Count; i++)
                        {
                            path = GetPath(v2, Utility.ConvertToVector2Int(CharacterList[i].transform.position), CharacterList[i].Info.Faction);
                            if (path != null)
                            {
                                v3 = new Vector3(v2.x, TileDic[v2].TileData.Height, v2.y);
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
            if(Utility.ConvertToVector2Int(CharacterList[i].transform.position) == target) 
                {
                    hasCharacter = true;
                    break;
                }
            }

            return hasCharacter;
        }
    }
}