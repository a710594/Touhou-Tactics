using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Battle;

public class CharacterListGroup : MonoBehaviour
{
    private static readonly int _max = 8;

    public Image Image;

    private List<Image> _imageList = new List<Image>();
    private List<BattleCharacterController> _characterList = new List<BattleCharacterController>();

    public void Init(List<BattleCharacterController> characterList)
    {
        _characterList = characterList;
        Image image;
        for (int i=0; i<_characterList.Count; i++) 
        {
            image = Instantiate(Image);
            image.transform.SetParent(transform);
            if (i > _max)
            {
                image.gameObject.SetActive(false);
            }
            _imageList.Add(image);
        }
    }

    public void Add(BattleCharacterController controller)
    {
        Image image;
        Sprite sprite;
        image = Instantiate(Image);
        image.transform.SetParent(transform);
        image.gameObject.SetActive(false);
        _imageList.Add(image);
        sprite = Resources.Load<Sprite>("Image/" + controller.SpriteFront);
        
    }

    public void Refresh() 
    {
        for (int i=0; i<_imageList.Count; i++) 
        {
            if (i < _characterList.Count)
            {
                _imageList[i].gameObject.SetActive(true);
                _imageList[i].sprite = _characterList[i].SpriteFront;
            }
            else
            {
                _imageList[i].gameObject.SetActive(false);
            }
        }

        //for (int i=0; i<_characterList.Count; i++) 
        //{
        //    if (i < _max)
        //    {
        //        _imageList[i].gameObject.SetActive(true);
        //        _imageList[i].sprite = _spriteDic[_characterList[i]];
        //    }
        //    else
        //    {
        //        _imageList[i].gameObject.SetActive(false);
        //    }
        //}
    }
}
