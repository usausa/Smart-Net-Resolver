namespace Example.FormsApp.Services
{
    using System;

    public class CounterEventArgs : EventArgs
    {
        public int Value { get; }

        public CounterEventArgs(int value)
        {
            Value = value;
        }
    }

    public class CounterService
    {
        // Legacy style event notification
        public event EventHandler<CounterEventArgs> Changed;

        private int counter;

        public void Increment()
        {
            counter++;
            Changed?.Invoke(this, new CounterEventArgs(counter));
        }
    }
}
