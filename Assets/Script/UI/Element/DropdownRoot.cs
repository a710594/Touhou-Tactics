using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropdownRoot : DropdownNode
{
    public virtual void ButtonOnClick(object data) { }

    public virtual void ButtonOnEnter(DropdownButton button) { }

    public virtual void ButtonOnExit() { }
}
