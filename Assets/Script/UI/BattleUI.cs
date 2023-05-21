using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUI : MonoBehaviour
{
    public SkillButton[] SkillButtons;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                Debug.Log(hit.transform.position);
                BattleController.Instance.Click(new Vector2(hit.transform.position.x, hit.transform.position.z));
            }
        }
    }

    private void SkillOnCkick(Skill skill) 
    {
        BattleController.Instance.SelectSkill(new Skill(DataContext.Instance.SkillDic[1], BattleController.Instance._selectedCharacter));
    }

    private void Awake()
    {
        SkillButtons[0].ClickHandler += SkillOnCkick;
    }
}
