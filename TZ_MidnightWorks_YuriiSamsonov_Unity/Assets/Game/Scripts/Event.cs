using System;

namespace Game.Scripts
{
    public delegate void Event<T>(T eventArgs) where T : EventArgs;
}