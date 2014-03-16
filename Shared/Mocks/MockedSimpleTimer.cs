using NUnit.Framework;
using System;

namespace PerpetualEngine
{
    public partial class SimpleTimer
    {
        static SimpleTimer()
        {
            SimpleTimer.Create = () => {
                return new MockedSimpleTimer();
            };
        }
    }

    class MockedSimpleTimer : SimpleTimer
    {
        bool stop = false;

        public override void Repeat(TimeSpan timeSpan, Action action)
        {
            RepeatAsync(timeSpan, action);
        }

        async void RepeatAsync(TimeSpan timeSpan, Action action)
        {
            stop = false;
            while (!stop) {
                await timeSpan;
                action();
            }
        }

        public override void Clear()
        {
            stop = true;
        }
    }
}
