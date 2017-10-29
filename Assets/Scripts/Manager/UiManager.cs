﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using System.Runtime.CompilerServices;
using System.Collections;
using UnityEngine.EventSystems;

public class UIManager : ManagerParent
{
	#region Variables
	#if UNITY_EDITOR
	public bool lauchGame = false;
	#endif
	public Slider MotionSlider;
	public GameObject GameOver;
    public Image RedScreen;
    public GameObject speedEffect;
	public Transform MenuParent;

	Dictionary <MenuType, UiParent> AllMenu;
	MenuType menuOpen;

	GameObject InGame;
	#endregion

	#region Mono
	#endregion

	#region Public Methods
	public void OpenThisMenu ( MenuType thisType, MenuTokenAbstract GetTok = null )
	{
		UiParent thisUi;
		InGame.SetActive ( false );
		if ( menuOpen != MenuType.Nothing )
		{
			CloseThisMenu ( );
		}

		menuOpen = thisType;

		if ( AllMenu.TryGetValue ( thisType, out thisUi ) )
		{
			thisUi.OpenThis ( GetTok );
		}
	}

	public void CloseThisMenu ( )
	{
		UiParent thisUi;
		InGame.SetActive ( true );

		if ( menuOpen != MenuType.Nothing && AllMenu.TryGetValue ( menuOpen, out thisUi ) )
		{
			thisUi.CloseThis (  );
		}

		menuOpen = MenuType.Nothing;
	}

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

    public void OpenDashSpeed()
    {
        speedEffect.GetComponent<CanvasGroup>().DOFade(1, .25f);
    }

    public void CloseDashSpeed()
    {
        speedEffect.GetComponent<CanvasGroup>().DOFade(0, .25f);
    }


    #endregion

    #region Private Methods



    protected override void InitializeManager ( )
	{
		InitializeUI ( );

		Object[] getAllMenu =Resources.LoadAll ( "Menu" );
		Dictionary<MenuType, UiParent> setAllMenu = new Dictionary<MenuType, UiParent> ( getAllMenu.Length );

		GameObject thisMenu;
		UiParent thisUi;

		for ( int a = 0; a < getAllMenu.Length; a++ )
		{
			thisMenu = (GameObject) Instantiate ( getAllMenu [ a ], MenuParent );
			thisUi = thisMenu.GetComponent<UiParent> ( );
			setAllMenu.Add ( thisUi.ThisMenu, thisUi );
			InitializeUI ( ref thisUi );
		}

		AllMenu = setAllMenu;

		InGame = transform.Find ( "Canvas/InGame" ).gameObject;

		#if UNITY_EDITOR
		if ( !lauchGame )
		{
			OpenThisMenu ( MenuType.MenuHome );
		}
		else
		{
			GlobalManager.GameCont.StartGame ( );
		}
		#else
		OpenThisMenu ( MenuType.MenuHome );
		#endif

	}

	void InitializeUI ( )
	{
		InvokeRepeating ( "checkCurosr", 0, 0.5f );
	}

	void checkCurosr ( )
	{
		if ( menuOpen != MenuType.Nothing )
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}
		else
		{
			Cursor.visible = false;
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

    /*
	{
		GameOver.gameObject.SetActive ( display );
	protected override void InitializeManager ( )
    */