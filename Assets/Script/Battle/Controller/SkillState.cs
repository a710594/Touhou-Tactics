using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public partial class BattleController
    {
        public class SkillState : BattleControllerState
        {
            private int _originalHP;
            private Timer _timer = new Timer();
            private BattleCharacterController _target = null;

            public SkillState(StateContext context) : base(context)
            {
            }

            public override void Begin() 
            {
                _selectedCharacter = Instance.SelectedCharacter;
                _selectedCharacter.Info.HasMain = true;

                //_target = null;
                //if (Instance._targetList.Count > 0 && Instance._targetList[0] != _selectedCharacter)
                //{
                //    _target = Instance._targetList[0];
                //    _originalHP = _target.Info.CurrentHP;
                //}

                //int maxCount = -1;
                //for (int i = 0; i < Instance._targetList.Count; i++)
                //{
                //    Instance.UseEffect(_selectedCharacter.Info.SelectedCommand, _selectedCharacter, Instance._targetList[i], out int count);
                //    if (count > maxCount) 
                //    {
                //        maxCount = count;
                //    }
                //}

                //if (_target != null) 
                //{
                //    Instance.CharacterInfoUIGroup.SetCharacterInfoUIWithTween_2(_target, _originalHP, Utility.ConvertToVector2Int(_target.transform.position));
                //}

                Skill skill = (Skill)_selectedCharacter.Info.SelectedCommand;
                if (skill.CD > 0)
                {
                    skill.CurrentCD = skill.CD + 1;
                }

                Instance.UseEffect();
                //_timer.Start(maxCount * 0.5f, Instance.CheckResult);
            }
        }
    }
}