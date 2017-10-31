using UnityEngine;
using System.Collections.Generic;
using System;

public class EventManager : ManagerParent
{
	#region Variables
	Dictionary<Type, Delegate> eventCallbacks;
	#endregion
	#region Public Methods
	/// <summary>
	/// Register a method with no return value and a single parament of the same type as the used event. Callback is called every time the event is raised.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="callback"></param>
	public void Register<T>(Action<T> callback) where T : GameEvent
	{
		Type eventType = typeof(T);

		Delegate eventDelegate;

		if(eventCallbacks.TryGetValue(eventType, out eventDelegate))
		{
		    //Try to remove the delegate first, to remove duplicates
		    eventDelegate = Delegate.Remove(eventDelegate, callback);
		    eventCallbacks[eventType] = Delegate.Combine(eventDelegate, callback);
		}
		else
		{
		    eventCallbacks.Add(eventType, callback);
		}
	}

	/// <summary>
	/// Remove callback from getting event messages
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="callback"></param>
	public void UnRegister<T>(Action<T> callback) where T : GameEvent
	{
	Type eventType = typeof(T);

	Delegate eventDelegate;

	if(eventCallbacks.TryGetValue(eventType, out eventDelegate))
	{
	    eventDelegate = Delegate.Remove(eventDelegate, callback);

	    if(eventDelegate != null)
	    {
	        eventCallbacks[eventType] = callback;
	    }
	    else
	    {
	        eventCallbacks.Remove(eventType);
	    }
	}
	}

	/// <summary>
	/// Raise an event, sending the event object to all clients that have registered for this specific event
	/// </summary>
	/// <param name="evnt"></param>
	public void Raise(GameEvent evnt)
	{
		Delegate eventDelegate;

		Type eventType = evnt.GetType();

		if(eventCallbacks.TryGetValue(eventType, out eventDelegate))
		{
		    Delegate[] callbacks = eventDelegate.GetInvocationList();

		    if(callbacks != null)
		    {
		        //Loop through callbacks and wrap each in a try/catch to avoid a single error breaking the entire chain
		        for(int i = 0; i < callbacks.Length; i++)
		        {
		            try
		            {
		                callbacks[i].DynamicInvoke(evnt);
		            }
		            catch (System.Reflection.TargetInvocationException e)
		            {
		                Debug.LogError(eventType + " Exception: " + e.InnerException.Message + "\n" + e.InnerException.StackTrace);
		            }
		            catch (Exception e)
		            {
		                Debug.LogError("Event with type: " + eventType + " raised with invalid argument. Message: " + e.Message);
		            }
		        }
		    }
		}
	}
	#endregion

	#region Private Methods
	protected override void InitializeManager()
	{
		eventCallbacks = new Dictionary<Type, Delegate>();
	}
	#endregion
}
