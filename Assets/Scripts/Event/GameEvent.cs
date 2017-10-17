using UnityEngine;
public abstract class GameEvent
{
    #region Variables
    #endregion
	
    #region Mono
    #endregion
	
    #region Public Methods
	public void Raise()
	{
		GlobalManager.Event.Raise ( this );
	}
    #endregion
	
    #region Private Methods
    #endregion
}
