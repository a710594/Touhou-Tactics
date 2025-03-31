using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropdownNode : MonoBehaviour
{
    [NonSerialized]
    public DropdownNode Parent = null;
    [NonSerialized]
    public List<DropdownNode> ChildList = new List<DropdownNode>();
}
