using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using System.Runtime.CompilerServices;

public class UIManager : ManagerParent
{
	#region Variables
	public GameObject GameOver;
    public Image RedScreen;
    public static UIManager Singleton;

	bool CursorVisble = false;
	#endregion

	#region Mono
	#endregion

    

	#region Public Methods
	public void DisplayOver ( bool display )
	{

        GameOver.gameObject.SetActive ( display );
	}

    public void BloodHit()
    {
        Time.timeScale = 0;
        DOVirtual.DelayedCall(.065f, () => {
            Time.timeScale = 1;
        });
        Camera.main.DOFieldOfView(45, .12f);//.SetEase(Ease.InBounce);
        RedScreen.DOFade(.4f, .12f).OnComplete(() => {
            RedScreen.DOFade(0, .08f);
            Camera.main.DOFieldOfView(60, .08f);//.SetEase(Ease.InBounce);
        });
    }

    #endregion

    #region Private Methods

    void Awake()
    {
        if(Singleton == null)
        {
            Singleton = this;
        } else
        {
            Destroy(this);
        }
    }


    protected override void InitializeManager ( )
	{
		InitializeUI ( );
	}

	void InitializeUI ( )
	{
		InvokeRepeating ( "checkCurosr", 0, 0.5f );
	}

	void checkCurosr ( )
	{
		Cursor.visible = CursorVisble;
		if ( CursorVisble )
		{
			Cursor.lockState = CursorLockMode.None;
		}
		else
		{
			Cursor.lockState = CursorLockMode.Locked;
		}
	}




	void InitializeUI<T>(ref T manager) where T : UiParent
	{
		//Debug.Log("Initializing managers");
		T[] managers = GetComponentsInChildren<T>();

		if(managers.Length == 0)
		{
			//Debug.LogError("No manager of type: " + typeof(T) + " found.");
			return;
		}

		//Set to first manager
		manager = managers[0];
		manager.Initialize();

		if(managers.Length > 1) //Too many managers
		{
			//Debug.LogError("Found " + managers.Length + " UI of type " + typeof(T));
			for(int i = 1; i < managers.Length; i++)
			{
				Destroy(managers[i].gameObject);
			}
		} 
	}
	#endregion
}
