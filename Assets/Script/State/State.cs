using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class State
{
    protected StateContext _context = null;

    public State(StateContext context)
    {
        _context = context;
    }

    public virtual void Begin(object obj)
    {
    }

    public virtual void End()
    {
    }
}
