using System;
using System.Collections.Generic;
using Golf.States.Interfaces;

namespace Golf.States
{
	public class StateMachine : IUpdatable
	{
		private Dictionary<Type, IGameState> m_states = new();
		private IGameState m_currentState;
		private IUpdatable m_currentUpdatable;

		public void AddState<TState>(TState state) where TState : IGameState
		{
			m_states[typeof(TState)] = state;
		}

		public void ChangeState<TState>() where TState : class, IGameState
		{
			if (m_states.TryGetValue(typeof(TState), out IGameState state))
			{
				m_currentState?.Exit();
				m_currentState = state;
				m_currentUpdatable = m_currentState as IUpdatable;
				m_currentState.Enter();
			}
		}

		public void Update()
		{
			m_currentUpdatable?.Update();
		}
	}
}