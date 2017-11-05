using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using System.Runtime.CompilerServices;
using System.Collections;
using UnityEngine.EventSystems;

public class UiManager : ManagerParent
{
	#region Variables
	#if UNITY_EDITOR
	public bool lauchGame = false;
	#endif
	public Slider MotionSlider;
    public Slider Madness;
	public Image RedScreen;
	public GameObject speedEffect;
	public Transform MenuParent;
	public GameObject PatternBackground;
	public GameObject GlobalBack;

	public Text ScorePoints;
	public Text MoneyPoints;

    [Header("SHOP STUFF")]
    public Image SlowMotion;
    public Image BonusLife;

    [Header("MISC GAMEFEEL")]
    public Image CircleFeel;

    Dictionary <MenuType, UiParent> AllMenu;
	MenuType menuOpen;

	GameObject InGame;
	bool onMainScene = true;
	#endregion

	#region Mono
	#endregion

	#region Public Methods
	public void OpenThisMenu ( MenuType thisType, MenuTokenAbstract GetTok = null )
	{
		UiParent thisUi;

		if ( AllMenu.TryGetValue ( thisType, out thisUi ) )
		{
			InGame.SetActive ( false );
			if ( menuOpen != MenuType.Nothing )
			{
				CloseThisMenu ( true );
			}

			menuOpen = thisType;
			GlobalBack.SetActive ( true );
			thisUi.OpenThis ( GetTok );
		}
	}

	public void CloseThisMenu ( bool openNew = false )
	{
		UiParent thisUi;

		if ( menuOpen != MenuType.Nothing && AllMenu.TryGetValue ( menuOpen, out thisUi ) )
		{
			InGame.SetActive ( true );
			GlobalBack.SetActive ( false );
			thisUi.CloseThis ( );
			menuOpen = MenuType.Nothing;

			if ( onMainScene && !openNew )
			{
				OpenThisMenu ( MenuType.MenuHome );
			}
		}
	}

	public void BloodHit()
	{
		Time.timeScale = 0f;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
        DOVirtual.DelayedCall(.11f, () => {
			Time.timeScale = 1;
            Time.fixedDeltaTime = .02F;
        });
		Camera.main.DOFieldOfView(45, .12f);//.SetEase(Ease.InBounce);
		RedScreen.DOFade(.4f, .12f).OnComplete(() => {
			RedScreen.DOFade(0, .08f);
			Camera.main.DOFieldOfView(60, .08f);//.SetEase(Ease.InBounce);
		});
	}

    public void OpenMadness()
    {
        Camera.main.GetComponent<CameraFilterPack_Distortion_Dream2>().enabled = true;
        Camera.main.GetComponent<CameraFilterPack_Color_YUV>().enabled = true;
        //Camera.main.transform.GetComponent<RainbowMove>().enabled = false;

        //Camera.main.transform.DOKill(false);

        /*
        Camera.main.transform.DOLocalMoveY(0, .3f).OnComplete(() => {
            DOVirtual.DelayedCall(.65f,()=>{
                Camera.main.transform.DOLocalMoveY(.9f, .1f);
            });
        }).SetLoops(-1,LoopType.Yoyo);*/
        
        Camera.main.DOFieldOfView(40, .35f).OnComplete(() => {
            Camera.main.DOFieldOfView(60, .35f);
        }).SetLoops(-1,LoopType.Yoyo);
        
    }

    public void CloseMadness()
    {
        Camera.main.GetComponent<CameraFilterPack_Distortion_Dream2>().enabled = false;
        Camera.main.GetComponent<CameraFilterPack_Color_YUV>().enabled = false;
		Camera.main.DOKill ( true );

        Camera.main.transform.DOKill(true);

        //Camera.main.transform.GetComponent<RainbowMove>().enabled = true;
    }

	public void DashSpeedEffect ( bool enable )
	{
		if ( speedEffect == null )
		{
			return;
		}

		if ( enable )
		{
			speedEffect.GetComponent<CanvasGroup>().DOFade(1, .25f); 
		}
		else
		{
			speedEffect.GetComponent<CanvasGroup>().DOFade(0, .25f); 
		}
	}

    public void TakeCoin()
    {
        MoneyPoints.transform.DOScale(1.5f, .1f).SetEase(Ease.InBounce).OnComplete(() => {
            MoneyPoints.transform.DOScale(1f, .05f).SetEase(Ease.InBounce);
        });
    }

	public void StartSlowMo()
    {
        SlowMotion.transform.DOLocalMove(new Vector2(930, -510), .05f);
        CircleFeel.transform.DOScale(1, 0);
        CircleFeel.DOColor(Color.white, 0);
        SlowMotion.DOFade(0, .05f);
        DOVirtual.DelayedCall(.1f, () => {
            SlowMotion.DOFade(.75f, .1f);
            SlowMotion.transform.DOScale(4, 0f);
            CircleFeel.transform.DOScale(25, .25f);
            CircleFeel.DOFade(.75f, .15f).OnComplete(() => {
                CircleFeel.DOFade(0, .1f);
            });
            SlowMotion.transform.DOPunchPosition(Vector3.one * 30f, .15f, 18, 1).OnComplete(()=> {
                SlowMotion.transform.DOLocalMove(new Vector2(0, 0), .05f);
                SlowMotion.DOFade(0, .05f);
                DOVirtual.DelayedCall(.2f, () =>
                {
                    SlowMotion.DOFade(1, .15f);
                    SlowMotion.transform.DOScale(1, 0f);
                });
            });
        });
    }

    public void StartBonusLife()
    {
        CircleFeel.transform.DOScale(1, 0);
        CircleFeel.DOColor(new Color32(0xf4,0x6c,0x6e,0xff),0);
        BonusLife.transform.DOLocalMove(new Vector2(960, -480), .1f);
        BonusLife.GetComponent<RainbowScale>().enabled = false;
        BonusLife.DOFade(0, .05f);
        DOVirtual.DelayedCall(.1f, () => {
            BonusLife.DOFade(.75f, .1f);
            BonusLife.transform.DOScale(10, 0f);
            BonusLife.transform.DOPunchPosition(Vector3.one * 20f, .7f, 18, 1).OnComplete(() => {
                CircleFeel.transform.DOScale(28, .8f);
                CircleFeel.DOFade(1, .2f).OnComplete(() => {
                    CircleFeel.DOFade(0, .4f);
                });
                BonusLife.transform.DOScale(40f, .5f);
                BonusLife.DOFade(0, .5f);
            });
        });
    }
	#endregion

	#region Private Methods
	protected override void InitializeManager ( )
	{
		AllPlayerPrefs.SetIntValue ( Constants.Coin, 1000000 );
		InieUI ( );

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
		#else
		OpenThisMenu ( MenuType.MenuHome );
		#endif
	}

	void InieUI ( )
	{
        //	InvokeRepeating ( "checkCurosr", 0, 0.5f );

		System.Action <HomeEvent> checkLevel = delegate ( HomeEvent thisEvnt )
		{
			onMainScene = thisEvnt.onMenuHome;
		};

		GlobalManager.Event.Register ( checkLevel );

        MoneyPoints.text = "" + AllPlayerPrefs.GetIntValue(Constants.Coin);

        if ( PatternBackground != null )
		{
			PatternBackground.transform.DOLocalMoveY(-60, 5f).SetEase(Ease.Linear).OnComplete(() => {
				PatternBackground.transform.DOLocalMoveY(1092, 0);
			}).SetLoops(-1, LoopType.Restart);
		}
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
