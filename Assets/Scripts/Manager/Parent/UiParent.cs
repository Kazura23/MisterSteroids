using UnityEngine;

public abstract class UiParent : MonoBehaviour
{
	#region Variables
	protected bool pause;

	public abstract MenuType ThisMenu
	{
		get;
	}
	#endregion

	#region Mono
	#endregion

	#region Public Methods
	public void Initialize()
	{
		InitializeUi();
		CloseThis ( );
	}

	public virtual void OpenThis ( MenuTokenAbstract GetTok = null )
	{
		gameObject.SetActive ( true );
	}

	public virtual void CloseThis ( )
	{
		gameObject.SetActive ( false );
	}

	public virtual void Pause ( bool setPause )
	{
		pause = setPause;
	}
	#endregion

	#region Private Methods
	protected abstract void InitializeUi();
	#endregion
}
