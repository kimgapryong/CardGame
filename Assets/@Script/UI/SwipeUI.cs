using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeUI : MonoBehaviour
{
	[SerializeField]
	private	Scrollbar	scrollBar;					// Scrollbar의 위치를 바탕으로 현재 페이지 검사
	[SerializeField]
	private	Transform[]	circleContents;				// 현재 페이지를 나타내는 원 Image UI들의 Transform
	[SerializeField]
	private	float		swipeTime = 0.2f;			// 페이지가 Swipe 되는 시간
	[SerializeField]
	private	float		swipeDistance = 50.0f;		// 페이지가 Swipe되기 위해 움직여야 하는 최소 거리

	private	float[]		scrollPageValues;			// 각 페이지의 위치 값 [0.0 - 1.0]
	private	float		valueDistance = 0;			// 각 페이지 사이의 거리
	private	int			currentPage = 0;			// 현재 페이지
	private	int			maxPage = 0;				// 최대 페이지
	private	float		startTouchX;				// 터치 시작 위치
	private	float		endTouchX;					// 터치 종료 위치
	private	bool		isSwipeMode = false;		// 현재 Swipe가 되고 있는지 체크
	private	float		circleContentScale = 1.6f;  // 현재 페이지의 원 크기(배율)

	private int myNum = 0;
	public static bool isWipe = false;
	private void Awake()
	{
		if(circleContents.Length < 0)
			return;
		// 스크롤 되는 페이지의 각 value 값을 저장하는 배열 메모리 할당
		scrollPageValues = new float[transform.childCount];

		// 스크롤 되는 페이지 사이의 거리
		valueDistance = 1f / (scrollPageValues.Length - 1f);

		// 스크롤 되는 페이지의 각 value 위치 설정 [0 <= value <= 1]
		for (int i = 0; i < scrollPageValues.Length; ++ i )
		{
			scrollPageValues[i] = valueDistance * i;
		}

		// 최대 페이지의 수
		maxPage = transform.childCount;
	}

	private void Start()
	{
        if (circleContents.Length <= 0)
            return;
        // 최초 시작할 때 0번 페이지를 볼 수 있도록 설정
        SetScrollBarValue(0);
	}

	public void SetScrollBarValue(int index)
	{
		currentPage		= index;
		scrollBar.value	= scrollPageValues[index];
	}
    public void SetCircelContents(List<Transform> circleTrans, int myNunm)
    {
        this.myNum = myNunm;
        circleContents = new Transform[circleTrans.Count];

        for (int i = 0; i < circleTrans.Count; i++)
            circleContents[i] = circleTrans[i];

        // 여기서 transform.childCount가 아니라 전달받은 circleTrans.Count 기준으로 처리
        scrollPageValues = new float[circleTrans.Count];
        valueDistance = 1f / (scrollPageValues.Length - 1f);

        for (int i = 0; i < scrollPageValues.Length; ++i)
            scrollPageValues[i] = valueDistance * i;

        maxPage = circleTrans.Count;

        SetScrollBarValue(0);
    }

    private void Update()
	{
		UpdateInput();

		// 아래에 배치된 페이지 버튼 제어
		UpdateCircleContent();
	}

	private void UpdateInput()
	{
		// 현재 Swipe를 진행중이면 터치 불가
		if ( isSwipeMode == true ) return;
		if (isWipe == true && myNum == 0) return;	

		#if UNITY_EDITOR
		// 마우스 왼쪽 버튼을 눌렀을 때 1회
		if ( Input.GetMouseButtonDown(0) )
		{
			// 터치 시작 지점 (Swipe 방향 구분)
			startTouchX = Input.mousePosition.x;
		}
		else if ( Input.GetMouseButtonUp(0) )
		{
			// 터치 종료 지점 (Swipe 방향 구분)
			endTouchX = Input.mousePosition.x;

			UpdateSwipe();
		}
		#endif

		#if UNITY_ANDROID
		if ( Input.touchCount == 1 )
		{
			Touch touch = Input.GetTouch(0);

			if ( touch.phase == TouchPhase.Began )
			{
				// 터치 시작 지점 (Swipe 방향 구분)
				startTouchX = touch.position.x;
			}
			else if ( touch.phase == TouchPhase.Ended )
			{
				// 터치 종료 지점 (Swipe 방향 구분)
				endTouchX = touch.position.x;

				UpdateSwipe();
			}
		}
		#endif
	}

	private void UpdateSwipe()
	{
		// 너무 작은 거리를 움직였을 때는 Swipe X
		if ( Mathf.Abs(startTouchX-endTouchX) < swipeDistance )
		{
			// 원래 페이지로 Swipe해서 돌아간다
			StartCoroutine(OnSwipeOneStep(currentPage));
			return;
		}

		// Swipe 방향
		bool isLeft = startTouchX < endTouchX ? true : false;

		// 이동 방향이 왼쪽일 때
		if ( isLeft == true )
		{
			// 현재 페이지가 왼쪽 끝이면 종료
			if ( currentPage == 0 ) return;

			// 왼쪽으로 이동을 위해 현재 페이지를 1 감소
			currentPage --;
		}
		// 이동 방향이 오른쪽일 떄
		else
		{
			// 현재 페이지가 오른쪽 끝이면 종료
			if ( currentPage == maxPage - 1 ) return;

			// 오른쪽으로 이동을 위해 현재 페이지를 1 증가
			currentPage ++;
		}

		// currentIndex번째 페이지로 Swipe해서 이동
		StartCoroutine(OnSwipeOneStep(currentPage));
	}

	/// <summary>
	/// 페이지를 한 장 옆으로 넘기는 Swipe 효과 재생
	/// </summary>
	private IEnumerator OnSwipeOneStep(int index)
	{
		float start		= scrollBar.value;
		float current	= 0;
		float percent	= 0;

		isSwipeMode = true;

		while ( percent < 1 )
		{
			current += Time.deltaTime;
			percent = current / swipeTime;

			scrollBar.value = Mathf.Lerp(start, scrollPageValues[index], percent);

			yield return null;
		}

		isSwipeMode = false;
	}

	private void UpdateCircleContent()
	{
		// 아래에 배치된 페이지 버튼 크기, 색상 제어 (현재 머물고 있는 페이지의 버튼만 수정)
		for ( int i = 0; i < scrollPageValues.Length; ++ i )
		{
			circleContents[i].localScale					= Vector2.one;
			circleContents[i].GetComponent<Image>().color	= Color.white;

			// 페이지의 절반을 넘어가면 현재 페이지 원을 바꾸도록
			if ( scrollBar.value < scrollPageValues[i] + (valueDistance / 2) && scrollBar.value > scrollPageValues[i] - (valueDistance / 2) )
			{
				circleContents[i].localScale					= Vector2.one * circleContentScale;
				circleContents[i].GetComponent<Image>().color	= Color.white;
			}
		}
	}
}



/*using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SwipeUI : MonoBehaviour
{
	[SerializeField]
	private	Scrollbar	scrollBar;				// Scrollbar의 위치를 바탕으로 현재 페이지 검사
	
	[SerializeField]
	private	float		swipeTime = 0.2f;		// 페이지가 Swipe 되는 시간
	[SerializeField]
	private	float		swipeDistance = 50.0f;	// 페이지가 Swipe되기 위해 움직여야 하는 최소 거리

	private	float[]		arrayScrollValue;		// 각 페이지의 위치 값 [0.0 - 1.0]
	private	float		valueDistance = 0;		// 각 페이지 사이의 거리
	private	int			currentPage = 0;		// 현재 페이지
	private	int			maxPage = 0;			// 최대 페이지
	private	float		startTouchX;			// 터치 시작 위치
	private	float		endTouchX;				// 터치 종료 위치
	private	bool		isSwipeMode = false;	// 현재 Swipe가 되고 있는지 체크

	private	float		circleContentScale = 1.6f;	// 현재 페이지의 원 크기(배율)

	private void Awake()
	{
		// 스크롤 되는 페이지의 각 value 값을 저장하는 배열 메모리 할당
		arrayScrollValue = new float[transform.childCount];

		// 스크롤 되는 페이지 사이의 거리
		valueDistance = 1f / (arrayScrollValue.Length - 1f);

		// 스크롤 되는 페이지의 각 value 위치 설정 [0 <= value <= 1]
		for (int i = 0; i < arrayScrollValue.Length; ++ i )
		{
			arrayScrollValue[i] = valueDistance * i;
		}

		// 최대 페이지의 수
		maxPage = transform.childCount;
	}

	private void Start()
	{
		// 최초 시작할 때 0번 페이지를 볼 수 있도록 설정
		SetScrollBarValue(0);
	}

	public void SetScrollBarValue(int index)
	{
		currentPage		= index;
		scrollBar.value	= arrayScrollValue[index];
	}

	private void Update()
	{
		// 아래에 배치된 페이지 버튼 제어
		UpdateCircleContent();

		// 현재 Swipe를 진행중이면 터치 불가
		if ( isSwipeMode == true ) return;

		#if UNITY_EDITOR
		// 마우스 왼쪽 버튼을 눌렀을 때 1회
		if ( Input.GetMouseButtonDown(0) )
		{
			// 터치 시작 지점 (Swipe 방향 구분)
			startTouchX = Input.mousePosition.x;
		}
		else if ( Input.GetMouseButtonUp(0) )
		{
			// 터치 종료 지점 (Swipe 방향 구분)
			endTouchX = Input.mousePosition.x;

			UpdateSwipe();
		}
		#endif

		#if UNITY_ANDROID
		if ( Input.touchCount == 1 )
		{
			Touch touch = Input.GetTouch(0);

			if ( touch.phase == TouchPhase.Began )
			{
				// 터치 시작 지점 (Swipe 방향 구분)
				startTouchX = touch.position.x;
			}
			else if ( touch.phase == TouchPhase.Ended )
			{
				// 터치 종료 지점 (Swipe 방향 구분)
				endTouchX = touch.position.x;

				UpdateSwipe();
			}
		}
		#endif
	}

	private void UpdateSwipe()
	{
		// 너무 작은 거리를 움직였을 때는 Swipe X
		if ( Mathf.Abs(startTouchX-endTouchX) < swipeDistance )
		{
			// 원래 페이지로 Swipe해서 돌아간다
			StartCoroutine(OnSwipeOneStep(currentPage));
			return;
		}

		// Swipe 방향
		bool isLeft = startTouchX < endTouchX ? true : false;

		// 이동 방향이 왼쪽일 때
		if ( isLeft == true )
		{
			// 현재 페이지가 왼쪽 끝이면 종료
			if ( currentPage == 0 )
			{
				return;
			}

			currentPage --;
		}
		// 이동 방향이 오른쪽일 떄
		else
		{
			// 현재 페이지가 오른쪽 끝이면 종료
			if ( currentPage == maxPage - 1 )
			{
				return;
			}

			currentPage ++;
		}

		// currentIndex번째 페이지로 Swipe해서 이동
		StartCoroutine(OnSwipeOneStep(currentPage));
	}

	/// <summary>
	/// 페이지를 한 장 옆으로 넘기는 Swipe 효과 재생
	/// </summary>
	private IEnumerator OnSwipeOneStep(int index)
	{
		float start		= scrollBar.value;
		float current	= 0;
		float percent	= 0;

		isSwipeMode = true;

		while ( percent < 1 )
		{
			current += Time.deltaTime;
			percent = current / swipeTime;

			scrollBar.value = Mathf.Lerp(start, arrayScrollValue[index], percent);

			yield return null;
		}

		isSwipeMode = false;
	}

	private void UpdateCircleContent()
	{
		// 아래에 배치된 페이지 버튼 크기, 색상 제어 (현재 머물고 있는 페이지의 버튼만 수정)
		for ( int i = 0; i < arrayScrollValue.Length; ++ i )
		{
			circleContents[i].localScale					= Vector2.one;
			circleContents[i].GetComponent<Image>().color	= Color.white;

			// 페이지의 절반을 넘어가면 현재 페이지 원을 바꾸도록
			if ( scrollBar.value < arrayScrollValue[i] + (valueDistance / 2) && scrollBar.value > arrayScrollValue[i] - (valueDistance / 2) )
			{
				circleContents[i].localScale					= Vector2.one * circleContentScale;
				circleContents[i].GetComponent<Image>().color	= Color.black;
			}
		}
	}
}

*/