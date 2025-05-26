using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeUI : MonoBehaviour
{
    [SerializeField]
    private Scrollbar scrollBar;
    [SerializeField]
    private Transform[] circleContents;
    [SerializeField]
    private float swipeTime = 0.2f;

    private float swipeDistance = 20f;

    private float[] scrollPageValues;
    private float valueDistance = 0;
    private int currentPage = 0;
    private int maxPage = 0;
    private float startTouchX;
    private float endTouchX;
    private bool isSwipeMode = false;
    private float circleContentScale = 1.6f;

    private int myNum = 0;
    public static bool isWipe = false;

    private void Awake()
    {
        if (circleContents.Length < 0)
            return;

        scrollPageValues = new float[transform.childCount];
        valueDistance = 1f / (scrollPageValues.Length - 1f);

        for (int i = 0; i < scrollPageValues.Length; ++i)
            scrollPageValues[i] = valueDistance * i;

        maxPage = transform.childCount;
    }

    private void Start()
    {
        if (circleContents.Length <= 0)
            return;

        SetScrollBarValue(1);
        RegisterCircleClickEvents();
    }

    public void SetScrollBarValue(int index)
    {
        currentPage = index;
        scrollBar.value = scrollPageValues[index];
    }

    public void SetCircelContents(List<Transform> circleTrans, int myNunm)
    {
        this.myNum = myNunm;
        circleContents = new Transform[circleTrans.Count];

        for (int i = 0; i < circleTrans.Count; i++)
            circleContents[i] = circleTrans[i];

        scrollPageValues = new float[circleTrans.Count];
        valueDistance = 1f / (scrollPageValues.Length - 1f);

        for (int i = 0; i < scrollPageValues.Length; ++i)
            scrollPageValues[i] = valueDistance * i;

        maxPage = circleTrans.Count;

        SetScrollBarValue(0);
        RegisterCircleClickEvents();
    }

    private void RegisterCircleClickEvents()
    {
        for (int i = 0; i < circleContents.Length; i++)
        {
            int index = i;
            Button btn = circleContents[i].GetComponent<Button>();
            if (btn == null) btn = circleContents[i].gameObject.AddComponent<Button>();

            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                if (!isSwipeMode)
                    StartCoroutine(OnSwipeOneStep(index));
            });
        }
    }

    private void Update()
    {
        UpdateInput();
        UpdateCircleContent();
    }

    private void UpdateInput()
    {
        if (isSwipeMode) return;
        if (isWipe && myNum == 0) return;

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            startTouchX = Input.mousePosition.x;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            endTouchX = Input.mousePosition.x;
            UpdateSwipe();
        }
#endif

#if UNITY_ANDROID
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
                startTouchX = touch.position.x;
            else if (touch.phase == TouchPhase.Ended)
            {
                endTouchX = touch.position.x;
                UpdateSwipe();
            }
        }
#endif
    }

    private void UpdateSwipe()
    {
        float deltaX = endTouchX - startTouchX;

        if (Mathf.Abs(deltaX) < swipeDistance)
        {
            StartCoroutine(OnSwipeOneStep(currentPage));
            return;
        }

        bool isLeft = deltaX > 0;

        if (isLeft)
        {
            if (currentPage == 0) return;
            currentPage--;
        }
        else
        {
            if (currentPage == maxPage - 1) return;
            currentPage++;
        }

        StartCoroutine(OnSwipeOneStep(currentPage));
    }

    private IEnumerator OnSwipeOneStep(int index)
    {
        float start = scrollBar.value;
        float current = 0;
        float percent = 0;

        isSwipeMode = true;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / swipeTime;

            scrollBar.value = Mathf.Lerp(start, scrollPageValues[index], percent);

            yield return null;
        }

        currentPage = index;
        isSwipeMode = false;
    }

    private void UpdateCircleContent()
    {
        for (int i = 0; i < scrollPageValues.Length; ++i)
        {
            circleContents[i].localScale = Vector2.one;
            circleContents[i].GetComponent<Image>().color = Color.white;

            if (scrollBar.value < scrollPageValues[i] + (valueDistance / 2) &&
                scrollBar.value > scrollPageValues[i] - (valueDistance / 2))
            {
                circleContents[i].localScale = Vector2.one * circleContentScale;
                circleContents[i].GetComponent<Image>().color = Color.white;
            }
        }
    }
}
