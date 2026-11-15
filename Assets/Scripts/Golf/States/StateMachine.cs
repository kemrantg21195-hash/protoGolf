using System;
using System.Collections.Generic;
using Golf.States.Interfaces;

namespace Golf.States
{
	public class StateMachine : IUpdatable
	{
		private readonly Dictionary<Type, IGameState> m_states = new();

		private IGameState m_currentState;
		private IUpdatable m_currentUpdatable;

		public void AddState<TState>(TState state) where TState : IGameState
		{
			Type type = typeof(TState);
			m_states[type] = state;
		}

		public void ChangeState<TState>() where TState : class, IGameState
		{
			Type type = typeof(TState);

			if (!m_states.TryGetValue(type, out IGameState nextState))
			{
				return;
			}

			m_currentState?.Exit();
			m_currentState = nextState;
			m_currentUpdatable = m_currentState as IUpdatable;
			m_currentState.Enter();
		}

		public void Update()
		{
			m_currentUpdatable?.Update();
		}
	}
}