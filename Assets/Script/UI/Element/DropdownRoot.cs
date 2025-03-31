using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropdownRoot : DropdownNode
{
    public virtual void ButtonOnClick(object data) { }

    public virtual void ButtonOnEnter(object data, DropdownButton button, DropdownGroup group) { }

    public virtual void ButtonOnExit() { }
}
