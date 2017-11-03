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

    public GameObject ScorePoints;
    public GameObject MoneyPoints;

    [HideInInspector]
    public float totalDistance;

    [Header("SHOP STUFF")]
    public Image SlowMotion;
    public Image BonusLife;

    [Header("MISC GAMEFEEL")]
    public Image CircleFeel;

    [Header("GAME OVER")]
    public GameObject GameOverObject;
    public GameObject PatternGameOver, BarGameOver;
    public Text YouGameOver, MadeGameOver, PointsGameOver, PressGameOver;


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

		if ( AllMenu.TryGetValue ( thisType, out thisUi ) )
		{
			InGame.SetActive ( false );
			if ( menuOpen != MenuType.Nothing )
			{
				CloseThisMenu ( );
			}

			menuOpen = thisType;
			GlobalBack.SetActive ( true );
			thisUi.OpenThis ( GetTok );
		}
	}

	public void CloseThisMenu ( )
	{
		UiParent thisUi;

		if ( menuOpen != MenuType.Nothing && AllMenu.TryGetValue ( menuOpen, out thisUi ) )
		{
			InGame.SetActive ( true );
			GlobalBack.SetActive ( false );
			thisUi.CloseThis (  );
			menuOpen = MenuType.Nothing;
		}

	}

	public void DisplayOver ( bool display )
	{
		//GameOver.gameObject.SetActive ( display );
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

	public void OpenDashSpeed()
	{
		if ( speedEffect != null )
		{
			speedEffect.GetComponent<CanvasGroup>().DOFade(1, .25f); 
		}
	}

    public void GameOver()
    {
        Debug.Log("GameOver");
        PointsGameOver.text = "" + Mathf.RoundToInt(totalDistance);
        YouGameOver.DOFade(0, 0);
        MadeGameOver.DOFade(0, 0);
        PointsGameOver.DOFade(0, 0);
        YouGameOver.transform.DOScale(5, 0);
        MadeGameOver.transform.DOScale(5, 0);
        PointsGameOver.transform.DOScale(5, 0);
        BarGameOver.transform.DOScaleY(0, 0);

        PatternGameOver.transform.DOLocalMoveY(-60, 5f).SetEase(Ease.Linear).OnComplete(() => {
            PatternGameOver.transform.DOLocalMoveY(1092, 0);
        }).SetLoops(-1, LoopType.Restart);


        GameOverObject.GetComponent<CanvasGroup>().DOFade(1f, 1f).OnComplete(() =>
        {
            YouGameOver.DOFade(1, .25f);
            YouGameOver.transform.DOScale(1, .25f).OnComplete(()=> {
                MadeGameOver.DOFade(1, .25f);
                MadeGameOver.transform.DOScale(1, .25f).OnComplete(() =>
                {
                    PointsGameOver.DOFade(1, .25f);
                    BarGameOver.transform.DOScaleY(1.25f, .2f).OnComplete(() =>
                    {
                        BarGameOver.transform.DOScaleY(1, .05f);
                    });
                    PointsGameOver.transform.DOScale(1, .25f);

                    DOVirtual.DelayedCall(1.5f, () => {
                        PressGameOver.GetComponent<CanvasGroup>().DOFade(1, .5f);
                    });
                });
            });
        });
    }

    public void StartSlowMo()
    {
        SlowMotion.transform.DOLocalMove(new Vector2(960, -540), .05f);
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

	public void CloseDashSpeed()
	{
		if ( speedEffect != null )
		{
			speedEffect.GetComponent<CanvasGroup>().DOFade(0, .25f); 
		}
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
		#else
		OpenThisMenu ( MenuType.MenuHome );
		#endif
	}

    void Update()
    {

        ScorePoints.transform.GetChild(0).GetComponent<Text>().text = "" + Mathf.RoundToInt(totalDistance);
        MoneyPoints.transform.GetChild(0).GetComponent<Text>().text = "" + AllPlayerPrefs.GetIntValue(Constants.Coin);
    }

    void InitializeUI ( )
	{
		//	InvokeRepeating ( "checkCurosr", 0, 0.5f );

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
