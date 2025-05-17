using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class ExploreEnemyController : MonoBehaviour
    {
        public SpriteRenderer Arrow;
        public ExploreFileEnemy File;
        public CharacterController CharacterController;

        public virtual void Init(ExploreFileEnemy file)
        {
            File = file;
            Arrow.color = Color.yellow;
        }

        public class ExploreEnemyContext : StateContext
        {
            public ExploreEnemyController Controller;

            public void Init(ExploreEnemyController characterController)
            {
                Controller = characterController;
            }
        }

        protected class ExploreEnemyState : State
        {
            public ExploreEnemyController Enemy;

            public ExploreEnemyState(ExploreEnemyContext context) : base(context)
            {
                Enemy = context.Controller;
            }

            public virtual void Update() { }

            public virtual void OnControllerColliderHit(ControllerColliderHit hit) { }
        }
    }
}