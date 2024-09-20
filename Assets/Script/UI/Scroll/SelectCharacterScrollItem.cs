using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Battle
{

    public class SelectCharacterScrollItem : ScrollItem
    {
        private bool _drag = false;
        private Transform _anchor; //¦³¥Î¶Ü?

        public override void SetData(object data)
        {
            base.SetData(data);

            Sprite sprite = Resources.Load<Sprite>("Image/" + ((CharacterInfo)Data).Controller + "_F");
            Image.sprite = sprite;
            Image.transform.localPosition = Vector3.zero;
        }

        /*public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);

            _drag = true;
            Image.color = Color.white;
        }

        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);

            Image.rectTransform.anchoredPosition += eventData.delta;
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            //base.OnEndDrag(eventData);

            _drag = false;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Vector2Int position = new Vector2Int();
            if (Physics.Raycast(ray, out hit, 100))
            {
                position = Utility.ConvertToVector2Int(hit.point);
                GameObject obj = BattleController.Instance.PlaceCharacter(position, (CharacterInfo)Data);
                if (obj != null)
                {
                    _anchor = obj.transform;
                    Image.color = Color.clear;

                    if (DragEndHandler != null)
                    {
                        DragEndHandler(this);
                    }
                }
                else
                {
                    BattleController.Instance.SetCharacterSpriteVisible((CharacterInfo)Data, true);
                    transform.localPosition = Vector3.zero;
                    Image.color = Color.white;
                }
            }
            else
            {
                BattleController.Instance.SetCharacterSpriteVisible((CharacterInfo)Data, true);
                transform.localPosition = Vector3.zero;
                Image.color = Color.white;
            }
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            if (eventData.button == PointerEventData.InputButton.Right)
            {
                _drag = false;
                _anchor = null;
                BattleController.Instance.RemoveCharacterSprite((CharacterInfo)Data);
                transform.localPosition = Vector3.zero;
                Image.color = Color.white;
            }
        }*/

        void Update()
        {
            if (_anchor != null && !_drag)
            {
                this.transform.position = Camera.main.WorldToScreenPoint(_anchor.position);
            }
        }
    }
}