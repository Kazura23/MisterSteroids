using UnityEngine;

public abstract class ManagerParent : MonoBehaviour
{
    #region Variables
    #endregion

    #region Mono
    #endregion

    #region Public Methods
    public void Initialize()
    {
        InitializeManager();
    }
    #endregion

    #region Private Methods
    protected abstract void InitializeManager();
    #endregion
}
