using System;

namespace ZTPwords.Logic.State
{
    public enum State
    {
        Learning,
        Test
    }

    public class Context
    {
        private StateMode _stateMode;

        public Context(State state)
        {
            ChangeState(state);
        }

        public void ChangeState(State state)
        {
            switch (state)
            {
                case State.Learning:
                    _stateMode = new LearningState();
                    break;
                case State.Test:
                    _stateMode = new TestState();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }
}