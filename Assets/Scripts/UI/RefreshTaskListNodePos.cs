using System;
using UnityEngine;

public class RefreshTaskListNodePos : MonoBehaviour
{
    private void OnDisable()
    {
        if (this.taskListViewNode)
        {
            Vector3 localPosition = this.taskListViewNode.localPosition;
            this.closePosY = localPosition.y;
            localPosition.y = 0f;
            this.taskListViewNode.localPosition = localPosition;
        }
    }

    private void OnEnable()
    {
        if (this.taskListViewNode)
        {
            Vector3 localPosition = this.taskListViewNode.localPosition;
            localPosition.y = this.closePosY;
            this.taskListViewNode.localPosition = localPosition;
        }
    }

    public Transform taskListViewNode;

    private float closePosY;
}
