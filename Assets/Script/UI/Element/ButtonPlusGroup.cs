using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPlusGroup : MonoBehaviour
{
    [HideInInspector]
    public ButtonPlusSingle CurrentSelect;

    public List<ButtonPlusSingle> Buttons = new List<ButtonPlusSingle>();

    public void SetSelect(ButtonPlus button)
    {
        CurrentSelect = null;
        for (int i = 0; i < Buttons.Count; i++)
        {
            if (Buttons[i].gameObject.Equals(button.gameObject))
            {
                Buttons[i].SetSelect(true);
                CurrentSelect = Buttons[i];
            }
            else
            {
                Buttons[i].SetSelect(false);
            }
        }
    }

    public void SetSelect(ButtonPlusSingle button)
    {
        CurrentSelect = null;
        for (int i = 0; i < Buttons.Count; i++)
        {
            if (Buttons[i].Equals(button))
            {
                Buttons[i].SetSelect(true);
                CurrentSelect = Buttons[i];
            }
            else
            {
                Buttons[i].SetSelect(false);
            }
        }
    }

    public void OnMoveDown(Vector2 v, ButtonPlusSingle defaultSelect)
    {
        if (CurrentSelect == null)
        {
            CurrentSelect = defaultSelect;
        }

        CurrentSelect.SetSelect(false);
        if (v.y > 0.1f)
        {
            if (CurrentSelect.SelectOnUp != null)
            {
                CurrentSelect = CurrentSelect.SelectOnUp;
            }
        }
        else if (v.y < -0.1f)
        {
            if (CurrentSelect.SelectOnDown != null)
            {
                CurrentSelect = CurrentSelect.SelectOnDown;
            }
        }
        else if (v.x > 0.1f)
        {
            if (CurrentSelect.SelectOnRight != null)
            {
                CurrentSelect = CurrentSelect.SelectOnRight;
            }
        }
        else if (v.x < -0.1f)
        {
            if (CurrentSelect.SelectOnLeft != null)
            {
                CurrentSelect = CurrentSelect.SelectOnLeft;
            }
        }
        CurrentSelect.SetSelect(true);
    }

    public void OnZDown()
    {
        if (CurrentSelect != null)
        {
            CurrentSelect.Button.OnPointerClick(null);
        }
    }

    public void Add(ButtonPlusSingle button)
    {
        button.ClickHandler += ButtonOnClick;
        button.EnterHandler += OnEnter;
        button.ExitHandler += OnExit;
        Buttons.Add(button);
    }

    public void Clear() 
    {
        Buttons.Clear();
    }

    public void CancelAllSelect() 
    {
        CurrentSelect = null;
        for (int i=0; i<Buttons.Count; i++) 
        {
            Buttons[i].SetSelect(false);
        }
    }

    private void ButtonOnClick(ButtonPlus button)
    {
        SetSelect(button);
    }

    private void OnEnter(ButtonPlus button)
    {
        //if (CurrentSelect != null && !button.gameObject.Equals(CurrentSelect.gameObject))
        //{
        //    CurrentSelect.SetSelect(false);
        //}
    }

    private void OnExit(ButtonPlus button)
    {
        if (CurrentSelect != null && button.gameObject.Equals(CurrentSelect.gameObject))
        {
            CurrentSelect.SetSelect(true);
        }
    }

    private void Awake()
    {
        for (int i = 0; i < Buttons.Count; i++)
        {
            Buttons[i].ClickHandler = ButtonOnClick;
            Buttons[i].EnterHandler += OnEnter;
            Buttons[i].ExitHandler += OnExit;
        }
    }
}
