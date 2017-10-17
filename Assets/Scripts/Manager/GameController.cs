using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : ManagerParent
{
	#region Variables
	public Transform GarbageTransform;
	public MeshDesctruc MeshDest;

    #endregion

    #region Mono

    #endregion

    #region Public Methods

    public void Restart() {
        SceneManager.LoadScene("ProtoAlex", LoadSceneMode.Single);
        GlobalManager.Ui.DisplayOver(false);
    }

    #endregion

    #region Private Methods
    protected override void InitializeManager ( )
	{
	}
	#endregion
}
