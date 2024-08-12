using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial
{
    public bool IsActive = true;
    protected StateContext _context = new StateContext();

    public virtual void Start()
    {
    }

    public virtual void Deregister()
    {
    }
}
