namespace Wpf3DAnimations.Common.Utilities
{
    using System;

    public class EventArgs<T> : EventArgs
    {
        public EventArgs()
        {
        }

        public EventArgs(T value)
        {
            Value = value;
        }

        public T Value { get; set; }
    }
}
