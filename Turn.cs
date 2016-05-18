using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
	public enum Turn
    {
        None = 0,
        Left = -1,
        Right = 1
    }

    public enum BallState
    {
        Free,
        Caught,
        Flaming
    }
}
