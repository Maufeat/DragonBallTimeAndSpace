using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICenterOnChild : MonoBehaviour, IDragHandler, IEventSystemHandler, IEndDragHandler
{
    private void Awake()
    {
        this._scrollView = base.GetComponent<ScrollRect>();
        if (this._scrollView == null)
        {
            Debug.LogError("CenterOnChild: No ScrollRect");
            return;
        }
        this._container = this._scrollView.content;
        this._grid = this._container.GetComponent<GridLayoutGroup>();
        if (this._grid == null)
        {
            Debug.LogError("CenterOnChild: No GridLayoutGroup on the ScrollRect's content");
            return;
        }
        this._scrollView.movementType = ScrollRect.MovementType.Unrestricted;
        this.ResetAll();
    }

    private void Update()
    {
        if (this._centering)
        {
            Vector3 localPosition = this._container.localPosition;
            if (this.moveType == MoveType.Horizontal)
            {
                localPosition.x = Mathf.Lerp(this._container.localPosition.x, this._targetPos, this.centerSpeed * Time.deltaTime);
                this._container.localPosition = localPosition;
                if (Mathf.Abs(this._container.localPosition.x - this._targetPos) < 0.002f)
                {
                    this._centering = false;
                }
            }
            else if (this.moveType == MoveType.Vertical)
            {
                localPosition.y = Mathf.Lerp(this._container.localPosition.y, this._targetPos, this.centerSpeed * Time.deltaTime);
                this._container.localPosition = localPosition;
                if (Mathf.Abs(this._container.localPosition.y - this._targetPos) < 0.002f)
                {
                    this._centering = false;
                }
            }
        }
    }

    public void Reset()
    {
        this._scrollView.enabled = false;
        bool centering = this._centering;
        if (this._centering)
        {
            this._centering = false;
        }
        this._childrenPos.Clear();
        float num;
        if (this.moveType == MoveType.Horizontal)
        {
            num = this._scrollView.GetComponent<RectTransform>().rect.width * 0.5f - this._grid.cellSize.x * 0.5f;
        }
        else
        {
            num = -this._scrollView.GetComponent<RectTransform>().rect.height * 0.5f + this._grid.cellSize.y * 0.5f;
        }
        this._childrenPos.Add(num);
        int activeChildCount = this.GetActiveChildCount(this._container);
        for (int i = 0; i < activeChildCount - 1; i++)
        {
            if (this.moveType == MoveType.Horizontal)
            {
                num -= this._grid.cellSize.x + this._grid.spacing.x;
            }
            else
            {
                num += this._grid.cellSize.y + this._grid.spacing.y;
            }
            this._childrenPos.Add(num);
        }
        if (centering)
        {
            this._centering = true;
        }
        this._scrollView.enabled = true;
    }

    public void ResetAll()
    {
        this.Reset();
        this._centering = false;
        if (this.moveType == MoveType.Horizontal)
        {
            Vector3 localPosition = this._container.transform.localPosition;
            localPosition.x = this._childrenPos[0];
            this._container.transform.localPosition = localPosition;
        }
        else
        {
            Vector3 localPosition2 = this._container.transform.localPosition;
            localPosition2.y = this._childrenPos[0];
            this._container.transform.localPosition = localPosition2;
        }
    }

    public void CenterOnTarget(GameObject target)
    {
        if (!target.activeSelf || target == null)
        {
            return;
        }
        int childCount = this._container.childCount;
        int num = 0;
        bool flag = false;
        for (int i = 0; i < childCount; i++)
        {
            GameObject gameObject = this._container.GetChild(i).gameObject;
            if (gameObject.activeSelf)
            {
                if (gameObject == target)
                {
                    flag = true;
                    break;
                }
                num++;
            }
        }
        if (!flag)
        {
            return;
        }
        this._centering = false;
        this._scrollView.enabled = false;
        float num2 = this._childrenPos[num];
        Vector3 localPosition = this._container.transform.localPosition;
        if (this.moveType == MoveType.Horizontal)
        {
            localPosition.x = num2;
        }
        else
        {
            localPosition.y = num2;
        }
        this._container.transform.localPosition = localPosition;
        if (this.onCenter != null)
        {
            this.onCenter(target);
        }
        this._scrollView.enabled = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this._centering = true;
        if (this.moveType == MoveType.Horizontal)
        {
            this._targetPos = this.FindClosestPos(this._container.localPosition.x);
        }
        else
        {
            this._targetPos = this.FindClosestPos(this._container.localPosition.y);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        this._centering = false;
    }

    private float FindClosestPos(float currentPos)
    {
        int index = 0;
        float result = 0f;
        float num = float.PositiveInfinity;
        for (int i = 0; i < this._childrenPos.Count; i++)
        {
            float num2 = this._childrenPos[i];
            float num3 = Mathf.Abs(num2 - currentPos);
            if (num3 < num)
            {
                num = num3;
                result = num2;
                index = i;
            }
        }
        this.UpdareCenterGameObj(index);
        return result;
    }

    private int GetActiveChildCount(Transform parentTrans)
    {
        int childCount = parentTrans.childCount;
        int num = 0;
        for (int i = 0; i < childCount; i++)
        {
            GameObject gameObject = parentTrans.GetChild(i).gameObject;
            if (gameObject.activeSelf)
            {
                num++;
            }
        }
        return num;
    }

    private void UpdareCenterGameObj(int index)
    {
        int childCount = this._container.childCount;
        int num = 0;
        for (int i = 0; i < childCount; i++)
        {
            GameObject gameObject = this._container.GetChild(i).gameObject;
            if (gameObject.activeSelf)
            {
                if (num == index)
                {
                    this.centerObj = gameObject;
                    break;
                }
                num++;
            }
        }
        if (this.onCenter != null)
        {
            this.onCenter(this.centerObj);
        }
    }

    [Range(5f, 20f)]
    public float centerSpeed = 10f;

    public MoveType moveType = MoveType.Horizontal;

    public Action<GameObject> onCenter;

    public GameObject centerObj;

    private ScrollRect _scrollView;

    private Transform _container;

    private List<float> _childrenPos = new List<float>();

    private float _targetPos;

    private bool _centering;

    private GridLayoutGroup _grid;
}
