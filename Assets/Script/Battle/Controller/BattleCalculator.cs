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
        public Action AfterCheckResultHandler;

        /*
         取得一個範圍內的座標,例如如果range=2,就會像這樣
              x
             xxx
            xxxxx
             xxx
              x
        */
        public List<Vector2Int> GetPositionList(int range, Vector2Int start)
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

                    if (!list.Contains(position + Vector2Int.up) && TileDic.ContainsKey(position + Vector2Int.up) && CheckRange(start, position + Vector2Int.up, range))
                    {
                        queue.Enqueue(position + Vector2Int.up);
                    }
                    if (!list.Contains(position + Vector2Int.down) && TileDic.ContainsKey(position + Vector2Int.down) && CheckRange(start, position + Vector2Int.down, range))
                    {
                        queue.Enqueue(position + Vector2Int.down);
                    }
                    if (!list.Contains(position + Vector2Int.left) && TileDic.ContainsKey(position + Vector2Int.left) && CheckRange(start, position + Vector2Int.left, range))
                    {
                        queue.Enqueue(position + Vector2Int.left);
                    }
                    if (!list.Contains(position + Vector2Int.right) && TileDic.ContainsKey(position + Vector2Int.right) && CheckRange(start, position + Vector2Int.right, range))
                    {
                        queue.Enqueue(position + Vector2Int.right);
                    }
                }
            }

            return list;
        }

        private bool CheckRange(Vector2Int start, Vector2Int position, int range) 
        {
            if (Utility.ManhattanDistance(start, position) <= range && Mathf.Abs(TileDic[start].TileData.Height - TileDic[position].TileData.Height) <= range) 
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Vector2Int> GetNormalAreaList(Vector2Int from, Vector2Int to, TargetEnum areaTarget, List<Vector2Int> areaList)
        {
            Vector2 v2 = to - from;
            int angle = Mathf.RoundToInt(Vector2.Angle(v2, Vector2.up) / 90f) * 90; //取最接近90的倍數的數
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

            RemoveRange(areaTarget, result);

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

        public void RemoveRange(TargetEnum target, List<Vector2Int> list) 
        {
            Vector2Int v2;
            for (int i = 0; i < CharacterAliveList.Count; i++)
            {
                v2 = Utility.ConvertToVector2Int(CharacterAliveList[i].transform.position);
                if (list.Contains(v2))
                {
                    if (target == TargetEnum.None ||
                       (target == TargetEnum.Us && SelectedCharacter.Info.Faction != CharacterAliveList[i].Info.Faction) ||
                       (target == TargetEnum.Them && SelectedCharacter.Info.Faction == CharacterAliveList[i].Info.Faction))
                    {
                        list.Remove(v2);
                        i--;
                    }
                }
            }

            if(target == TargetEnum.All) 
            {
                list.Remove(Utility.ConvertToVector2Int(SelectedCharacter.transform.position));
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


        public HitType CheckHit(int hit, BattleCharacterController user, BattleCharacterController target, BattleTutorial tutorial)
        {
            if (tutorial != null)
            {
                if (Instance.Tutorial.Hit)
                {
                    return HitType.Hit;
                }
                else if (Instance.Tutorial.Critical)
                {
                    return HitType.Critical;
                }
            }

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
                if ((hitRate - 1) * 1f > UnityEngine.Random.Range(0f, 1f))
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
            if (hit == -1)
            {
                return 1;
            }
            else
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
                Vector3 v3 = target.transform.position - user.transform.position;
                Vector2Int v2 = Utility.ConvertToVector2Int(v3);

                float angle1 = Vector2.Angle(v2, Utility.ConvertToVector2Int(target.transform.forward));
                float angle2 = Vector2.Angle(v2, Utility.ConvertToVector2Int(user.transform.forward));

                //戰士的被動技能
                if (Passive.Contains<SwordmanPassive>(user.Info.PassiveList) && angle1 > 90)
                {
                    angle1 = 90;

                }
                hitRate = (angle1 * (-1 / 180f) + 1.5f) * hitRate;

                return hitRate;
            }
        }

        public int GetDamage(Effect effect, BattleCharacterController user, BattleCharacterController target)
        {
            int damage = 0;
            if (effect.Type == EffectModel.TypeEnum.PhysicalAttack)
            {
                float atk = (float)user.Info.STR;
                List<Status> strBuffList = GetStatueList(user.Info, StatusModel.TypeEnum.STR, Utility.ConvertToVector2Int(user.transform.position));
                for (int i=0; i<strBuffList.Count;i++) 
                {
                    atk *= strBuffList[i].Value / 100f;
                }

                float def = (float)target.Info.CON;
                List<Status> conBuffList = GetStatueList(target.Info, StatusModel.TypeEnum.CON, Utility.ConvertToVector2Int(target.transform.position));
                for (int i = 0; i < conBuffList.Count; i++)
                {
                    def *= conBuffList[i].Value / 100f;
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
                List<Status> intBuffList = GetStatueList(user.Info, StatusModel.TypeEnum.INT, Utility.ConvertToVector2Int(user.transform.position));
                for (int i = 0; i < intBuffList.Count; i++)
                {
                    mtk *= intBuffList[i].Value / 100f;
                }

                float mef = (float)target.Info.MEN;
                List<Status> menBuffList = GetStatueList(target.Info, StatusModel.TypeEnum.MEN, Utility.ConvertToVector2Int(target.transform.position));
                for (int i = 0; i < menBuffList.Count; i++)
                {
                    mef *= menBuffList[i].Value / 100f;
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

        public int GetRecover(Effect effect, BattleCharacterController user)
        {
            float men = (float)user.Info.MEN;
            List<Status> menBuffList = GetStatueList(user.Info, StatusModel.TypeEnum.MEN, Utility.ConvertToVector2Int(user.transform.position));
            for (int i = 0; i < menBuffList.Count; i++)
            {
                men *= menBuffList[i].Value / 100f;
            }

            int recover = Mathf.RoundToInt(men * effect.Value / 100f);

            return recover;
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
                prediction = targetCurrentHp + GetRecover(effect, user);
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

        public BattleCharacterController GetCharacterByPosition(Vector2Int position) 
        {
            BattleCharacterController character = null;
            for (int i=0; i<CharacterAliveList.Count; i++) 
            {
                if(Utility.ConvertToVector2Int(CharacterAliveList[i].transform.position) == position) 
                {
                    character = CharacterAliveList[i];
                    break;
                }
            }

            for (int i = 0; i < CharacterDyingList.Count; i++)
            {
                if (Utility.ConvertToVector2Int(CharacterDyingList[i].transform.position) == position)
                {
                    character = CharacterDyingList[i];
                    break;
                }
            }

            for (int i = 0; i < TempList.Count; i++)
            {
                if (Utility.ConvertToVector2Int(TempList[i].transform.position) == position)
                {
                    character = TempList[i];
                    break;
                }
            }

            return character;
        }

        public ResultType GetResult()
        {
            List<BattleCharacterController> list = new List<BattleCharacterController>(Instance.CharacterAliveList);
            list.AddRange(Instance.CharacterDyingList);
            BattleCharacterController target;

            for (int i = 0; i < list.Count; i++)
            {
                target = list[i];
                if (target.Info.CurrentHP <= 0)
                {
                    if (target.Info.Faction == BattleCharacterInfo.FactionEnum.Player)
                    {
                        if (CharacterDyingList.Contains(target))
                        {
                            target.gameObject.SetActive(false);
                            CharacterDyingList.Remove(target);
                            CharacterDeadList.Add(target);
                        }
                        else
                        {
                            CharacterAliveList.Remove(target);
                            CharacterDyingList.Add(target);
                            target.StartFlash();
                        }
                    }
                    else
                    {
                        CharacterAliveList.Remove(target);
                        target.gameObject.SetActive(false);
                    }
                }
                else if (CharacterDyingList.Contains(target))
                {
                    CharacterAliveList.Add(target);
                    CharacterDyingList.Remove(target);
                    target.StopFlash();
                }
            }          

            int playerCount = 0;
            int enemyCount = 0;
            for (int i = 0; i < CharacterAliveList.Count; i++)
            {
                if (CharacterAliveList[i].Info.Faction == BattleCharacterInfo.FactionEnum.Player)
                {
                    playerCount++;
                }
                else
                {
                    enemyCount++;
                }
            }

            if (playerCount == 0)
            {
                return ResultType.Lose;
            }
            else if (enemyCount == 0)
            {
                return ResultType.Win;
            }
            else
            {
                return ResultType.None;
            }
        }

        public void CheckDeath(BattleCharacterController character) 
        {
            if (character.Info.CurrentHP <= 0)
            {
                if (character.Info.Faction == BattleCharacterInfo.FactionEnum.Player)
                {
                    if (CharacterDyingList.Contains(character))
                    {
                        character.gameObject.SetActive(false);
                        CharacterDyingList.Remove(character);
                        CharacterDeadList.Add(character);
                    }
                    else
                    {
                        CharacterAliveList.Remove(character);
                        CharacterDyingList.Add(character);
                        character.StartFlash();
                    }
                }
                else
                {
                    CharacterAliveList.Remove(character);
                    character.gameObject.SetActive(false);
                }
            }
            else if (CharacterDyingList.Contains(character))
            {
                CharacterAliveList.Add(character);
                CharacterDyingList.Remove(character);
                character.StopFlash();
            }
        }

        public void CheckResult() 
        {
            int playerCount = 0;
            int enemyCount = 0;
            for (int i = 0; i < CharacterAliveList.Count; i++)
            {
                if (CharacterAliveList[i].Info.Faction == BattleCharacterInfo.FactionEnum.Player)
                {
                    playerCount++;
                }
                else
                {
                    enemyCount++;
                }
            }

            if (playerCount == 0)
            {
                _context.SetState<LoseState>();
            }
            else if (enemyCount == 0)
            {
                _context.SetState<WinState>();
            }
            else
            {
                _context.SetState<MoveState>();
            }

            if (AfterCheckResultHandler != null) 
            {
                AfterCheckResultHandler();
            }
        }

        public List<Status> GetStatueList(BattleCharacterInfo info, StatusModel.TypeEnum type, Vector2Int position) 
        {
            List<Status> list = new List<Status>();
            for (int i = 0; i < CharacterAliveList.Count; i++)
            {
                for (int j = 0; j < CharacterAliveList[i].Info.StatusList.Count; j++)
                {
                    if ((type == StatusModel.TypeEnum.None || CharacterAliveList[i].Info.StatusList[j].Type == type) && info.Faction == CharacterAliveList[i].Info.Faction)
                    {
                        for (int k = 0; k < CharacterAliveList[i].Info.StatusList[j].AreaList.Count; k++)
                        {
                            if (Utility.ConvertToVector2Int(CharacterAliveList[i].transform.position) + CharacterAliveList[i].Info.StatusList[j].AreaList[k] == position)
                            {
                                list.Add(CharacterAliveList[i].Info.StatusList[j]);
                            }
                        }
                    }
                }
            }

            return list;
        }

        public void CheckLine(Vector3 from, Vector3 to, out bool isBlock, out Vector3 result)
        {
            isBlock = false;
            float height;
            Vector2Int position;
            List<Vector3> list = Utility.DrawLine3D(from, to);
            for (int i = 0; i < list.Count; i++)
            {
                position = Utility.ConvertToVector2Int(list[i]);
                {
                    if (TileDic.ContainsKey(position))
                    {
                        height = TileDic[position].TileData.Height * 0.5f;
                        if (TileDic[position].AttachData != null)
                        {
                            height += TileDic[position].AttachData.Height * 0.5f;
                        }

                        if(position != Utility.ConvertToVector2Int(from) && position != Utility.ConvertToVector2Int(to) && GetCharacterByPosition(position) != null) 
                        {
                            height += 0.5f;
                        }

                        if (height >= list[i].y)
                        {
                            isBlock = true;
                            result = list[i];
                            return;
                        }
                    }

                }
            }
            result = to;
        }

        public void CheckParabola(Vector3 from, Vector3 to, int parabolaHeight, out bool isBlock, out List<Vector3> result)
        {
            isBlock = false;
            result = null;
            float height;
            Vector2Int position;
            List<Vector3> line = Utility.DrawLine3D(from, to);
            List<Vector3> list_1 = Utility.DrawParabola(from, to, parabolaHeight, line.Count - 1);
            List<Vector3> list_2 = Utility.DrawParabola(from, to, parabolaHeight, (line.Count - 1) * 10);
            for (int i = 0; i < list_1.Count; i++)
            {
                position = Utility.ConvertToVector2Int(list_1[i]);
                if (TileDic.ContainsKey(position))
                {
                    height = TileDic[position].TileData.Height * 0.5f + 0.5f;
                    if (TileDic[position].AttachData != null)
                    {
                        height += TileDic[position].AttachData.Height * 0.5f;
                    }

                    if (position != Utility.ConvertToVector2Int(from) && position != Utility.ConvertToVector2Int(to) && GetCharacterByPosition(position) != null)
                    {
                        height += 0.5f;
                    }

                    if (height - list_1[i].y > 1)
                    {
                        isBlock = true;
                        result = new List<Vector3>();
                        for (int j = 0; j < i; j++)
                        {
                            for (int k = 0; k < 10; k++)
                            {
                                result.Add(list_2[j * 10 + k]);
                            }
                        }
                        break;
                    }
                }
            }

            if (result == null)
            {
                result = list_2;
            }
        }
    }
}