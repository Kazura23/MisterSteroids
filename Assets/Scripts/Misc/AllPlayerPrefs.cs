using UnityEngine;

public static class AllPlayerPrefs
{
   // public static int piece;

    #region Get Methods
	public static int GetIntValue ( string thisString )
	{
		return PlayerPrefs.GetInt ( thisString, 0 );
	}
    #endregion

	#region Set Methods
	public static void SetIntValue ( string thisString, int thisValue, bool addition = true )
	{
		if ( addition )
		{
			PlayerPrefs.SetInt ( thisString, GetIntValue ( thisString ) + thisValue );
		}
		else
		{
			PlayerPrefs.SetInt ( thisString, thisValue );
		}
	}
	#endregion
}
