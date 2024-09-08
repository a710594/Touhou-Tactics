using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusIconGroup : MonoBehaviour
{
    public StatusIcon StatusIcon;
    public Transform StatusGrid;

    private List<StatusIcon> _statusIconList = new List<StatusIcon>();

    public void SetData(BattleCharacterInfo character, bool raycastTarget) 
    {
        StatusIcon icon;

        for (int i = 0; i < _statusIconList.Count; i++)
        {
            _statusIconList[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < character.StatusList.Count; i++)
        {
            if (i >= _statusIconList.Count)
            {
                icon = Instantiate(StatusIcon);
                icon.transform.SetParent(StatusGrid);
                _statusIconList.Add(icon);
            }
            _statusIconList[i].gameObject.SetActive(true);
            _statusIconList[i].SetData(character.StatusList[i], raycastTarget);
        }
    }
}
