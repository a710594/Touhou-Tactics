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

    public void Add(BattleCharacterController controller)
    {
        Image image;
        Sprite sprite;
        image = Instantiate(Image);
        image.transform.SetParent(transform);
        image.gameObject.SetActive(false);
        _imageList.Add(image);
        sprite = Resources.Load<Sprite>("Image/" + controller.Info.FileName + "_F");
        controller.Sprite = sprite;
    }

    public void Refresh() 
    {
        for (int i=0; i<_imageList.Count; i++) 
        {
            if (i < BattleController.Instance.CharacterList.Count)
            {
                _imageList[i].gameObject.SetActive(true);
                _imageList[i].sprite = BattleController.Instance.CharacterList[i].Sprite;
            }
            else
            {
                _imageList[i].gameObject.SetActive(false);
            }
        }
    }
}
