using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quiz
{
    /// <summary>
    /// A generic empty state. Pass onExecute action into Constructor to run once when entering the state
    /// (or null to do nothing)
    /// </summary>
    public class State : AbstractState
    {
        readonly Action m_OnExecute;

        /// <param name="onExecute">An event that is invoked when the state is executed</param>
        ///
        // Constructor takes delegate to execute and optional name (for debugging)
        public State(Action onExecute, string stateName = nameof(State))
        {
            m_OnExecute = onExecute;
            Name = stateName;
        }

        public override IEnumerator Execute()
        {
            yield return null;

            if (m_Debug)
                base.DebugState();

            // Invokes the m_OnExecute Action if it exists
            m_OnExecute?.Invoke();
        }
    }
}