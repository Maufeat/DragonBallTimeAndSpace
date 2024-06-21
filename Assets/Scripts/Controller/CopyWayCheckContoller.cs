using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Managers;
using Models;
using msg;
using UnityEngine;

public class CopyWayCheckContoller : ControllerBase
{
    public CopyManager MCopyManager
    {
        get
        {
            return ManagerCenter.Instance.GetManager<CopyManager>();
        }
    }

    private GameObject wayRoot
    {
        get
        {
            if (this.wayRoot_ == null)
            {
                this.wayRoot_ = new GameObject("way_root");
            }
            return this.wayRoot_;
        }
    }

    public override string ControllerName
    {
        get
        {
            return base.GetType().ToString();
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (this.isStartCheck)
        {
            if (this.wayRoot.transform.childCount == 0)
            {
                this.isStartCheck = false;
                return;
            }
            this.CheckCurWalkIsValid();
        }
        if (this.showWayFlag)
        {
            this.showWpTipTimerTem += Time.deltaTime;
            if (this.showWpTipTimerTem > this.showWpTipTimer && this.validWpList.Count > 0)
            {
                this.showWpTipTimerTem = 0f;
                while (this.showTipIndex < this.validWpList.Count)
                {
                    if (this.wayPoints[this.validWpList[this.showTipIndex]].isValid)
                    {
                        Projector componentInChildren = this.wayPoints[this.validWpList[this.showTipIndex]].pointObj.GetComponentInChildren<Projector>();
                        if (componentInChildren != null)
                        {
                            componentInChildren.material.color = this.wayTipColor;
                        }
                        this.showTipIndex++;
                        break;
                    }
                    this.showTipIndex++;
                }
                if (this.showTipIndex >= this.validWpList.Count)
                {
                    this.showTipIndex = 0;
                    this.showWayFlag = false;
                    Scheduler.Instance.AddTimer(1f, false, delegate
                    {
                        this.ColorWayPoint(this.wayDefaultColor, true);
                    });
                    Scheduler.Instance.AddTimer(2f, false, delegate
                    {
                        this.ColorWayPoint(this.wayTipColor, true);
                    });
                    Scheduler.Instance.AddTimer(3f, false, delegate
                    {
                        this.ColorWayPoint(this.wayDefaultColor, true);
                    });
                    Scheduler.Instance.AddTimer(4f, false, delegate
                    {
                        this.ColorSingleWayPoint(this.wayFirstTipColor, this.validWpList[0]);
                        ManagerCenter.Instance.GetManager<CopyManager>().MCopyNetWork.ReqMSG_Show_Path_Way_End_CS();
                        this.isStartCheck = false;
                        if (this.isNeedCheckWPInLocal)
                        {
                            this.isStartCheck = true;
                        }
                    });
                }
            }
        }
    }

    internal void On_MSG_Ret_Find_Path_End_SC(MSG_Ret_Find_Path_End_SC msg)
    {
        Scheduler.Instance.AddTimer(2f, false, new Scheduler.OnScheduler(this.ClearWays));
    }

    internal void OnCheckWayPointValid(MSG_Ret_Find_Path_SC msg)
    {
        if (msg.moveres == 1U)
        {
            this.ColorSingleWayPoint(this.wayRightColor, msg.gridindex);
        }
        else if (msg.moveres == 0U)
        {
            this.ColorSingleWayPoint(this.wayWrongColor, msg.gridindex);
            Scheduler.Instance.AddTimer(2f, false, new Scheduler.OnScheduler(this.ClearWays));
        }
        if (this.validWpList.Count > 0 && this.validWpList[0] == msg.gridindex)
        {
            this.isWalkToFirst = true;
        }
        this.isWalkToFirst = true;
    }

    public void CreateCheckBoxsErea(float x, float y, float width, float height, float reCutCellNumX, float reCutCellNumY, uint[] ways = null, bool isShowIndex = false)
    {
        this.ClearWays();
        this.isWalkToFirst = false;
        this.validWpList.Clear();
        if (ways != null && ways.Length > 0)
        {
            this.validWpList = ways.ToList<uint>();
        }
        Vector2 serverStartPos = new Vector2(x - width / 2f, y - height / 2f);
        float deltaUnitX = width / reCutCellNumX;
        float deltaUnitY = height / reCutCellNumY;
        this.wpSizeX = width * 0.333333343f / reCutCellNumX;
        this.wpSizeY = height * 0.333333343f / reCutCellNumY;
        Vector2 offset = Vector2.right * deltaUnitX / 2f + Vector2.up * deltaUnitY / 2f;
        Vector3 wpScale = new Vector3(this.wpSizeX - 0.05f, 0.1f, this.wpSizeY - 0.05f);
        uint testIndex = 1U;
        FFEffectControl component = MainPlayer.Self.GetComponent<FFEffectControl>();
        FFeffect eeffectByName = component.GetEeffectByName("lz_yidong_blue");
        Action<GameObject> action = delegate (GameObject objPreab)
        {
            int num = 0;
            while ((float)num < reCutCellNumY)
            {
                int num2 = 0;
                while ((float)num2 < reCutCellNumX)
                {
                    Vector2 v = serverStartPos + new Vector2((float)num2 * deltaUnitX, (float)num * deltaUnitY) + offset;
                    Vector3 worldPosByServerPos = GraphUtils.GetWorldPosByServerPos(v);
                    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(objPreab);
                    gameObject.gameObject.SetActive(true);
                    gameObject.name = num + "_" + num2;
                    gameObject.transform.SetParent(this.wayRoot.transform);
                    gameObject.transform.localScale = wpScale;
                    gameObject.transform.position = worldPosByServerPos;
                    if (gameObject.GetComponent<BoxCollider>() == null)
                    {
                        gameObject.AddComponent<BoxCollider>();
                    }
                    worldPosByServerPos.y = MapHightDataHolder.GetMapHeight(worldPosByServerPos.x, worldPosByServerPos.z) + 0.1f;
                    gameObject.transform.position = worldPosByServerPos;
                    Vector3 up = Vector3.up;
                    this.GetRotationFor(gameObject, out up);
                    gameObject.transform.rotation = Quaternion.LookRotation(Vector3.forward, up);
                    Material material = UnityEngine.Object.Instantiate<Material>(objPreab.GetComponentInChildren<Projector>().material);
                    gameObject.GetComponentInChildren<Projector>().material = material;
                    Material material2 = material;
                    Projector componentInChildren = gameObject.GetComponentInChildren<Projector>();
                    componentInChildren.orthographicSize = wpScale.x * 0.5f;
                    componentInChildren.nearClipPlane = 0f;
                    componentInChildren.farClipPlane = 1f;
                    material2.color = Color.grey;
                    componentInChildren.ignoreLayers = (-1 & ~LayerMask.GetMask(new string[]
                    {
                        "Terrian"
                    }));
                    if (isShowIndex)
                    {
                        this.AddTestIndexText(gameObject, testIndex);
                    }
                    bool isValid = this.validWpList.Contains(testIndex);
                    float minCheckDist = Mathf.Min(offset.x, offset.y);
                    WayPointCheck value = new WayPointCheck(gameObject, isValid, testIndex, minCheckDist);
                    this.wayPoints.Add(testIndex, value);
                    testIndex += 1U;
                    num2++;
                }
                num++;
            }
            if (objPreab != null)
            {
                UnityEngine.Object.Destroy(objPreab);
            }
        };
        FFEffectControl ffeffectControl = component;
        Action<GameObject> onCreateObj = action;
        ffeffectControl.AddEffect("lz_yidong_blue", null, onCreateObj);
        Scheduler.Instance.AddTimer(3f, false, delegate
        {
            this.showWayFlag = true;
            this.showTipIndex = 0;
        });
    }

    private void AddTestIndexText(GameObject obj, uint index)
    {
        TextMesh textMesh = obj.GetComponentInChildren<TextMesh>();
        if (textMesh == null)
        {
            GameObject gameObject = new GameObject("index");
            textMesh = gameObject.AddComponent<TextMesh>();
        }
        textMesh.transform.SetParent(obj.transform);
        textMesh.transform.localPosition = Vector3.zero;
        textMesh.fontSize = 32;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.alignment = TextAlignment.Center;
        textMesh.transform.localScale = new Vector3(0.1f, 0.2f, 1f);
        textMesh.text = "N:" + index;
        textMesh.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
    }

    private bool CheckCurWalkIsValid()
    {
        if (MainPlayer.Self != null)
        {
            uint num = 1U;
            while ((ulong)num <= (ulong)((long)this.wayPoints.Count))
            {
                bool flag = this.wayPoints[num].IsInThisPointRange(MainPlayer.Self.NextPosition2D);
                if (flag)
                {
                    if (this.wayPoints[num].isValid)
                    {
                        this.ColorSingleWayPoint(this.wayRightColor, this.wayPoints[num].index);
                    }
                    else
                    {
                        this.ColorSingleWayPoint(this.wayWrongColor, this.wayPoints[num].index);
                    }
                    return this.wayPoints[num].isValid;
                }
                num += 1U;
            }
        }
        return false;
    }

    private bool GetGroudInfo(Vector3 pos, out RaycastHit hit)
    {
        Ray ray = new Ray(pos + Vector3.up * 100f, Vector3.down);
        return Physics.Raycast(ray, out hit, 150f, LayerMask.GetMask(new string[]
        {
            "Terrian"
        }));
    }

    private void GetRotationFor(GameObject obj, out Vector3 averageDir)
    {
        averageDir = Vector3.up;
        BoxCollider component = obj.GetComponent<BoxCollider>();
        int num = 0;
        if (component)
        {
            Vector3 point = (Vector3.up + Vector3.left) * 100f;
            Vector3 point2 = (Vector3.up + Vector3.right) * 100f;
            Vector3 point3 = (Vector3.down + Vector3.left) * 100f;
            Vector3 point4 = (Vector3.down + Vector3.right) * 100f;
            Vector3 vector = component.bounds.ClosestPoint(point);
            Vector3 vector2 = component.bounds.ClosestPoint(point2);
            Vector3 vector3 = component.bounds.ClosestPoint(point3);
            Vector3 vector4 = component.bounds.ClosestPoint(point4);
            Vector3[] array = new Vector3[]
            {
                vector,
                vector2,
                vector3,
                vector4
            };
            for (int i = 0; i < array.Length; i++)
            {
                Ray ray = new Ray(array[i] + Vector3.up * 100f, Vector3.down);
                RaycastHit raycastHit;
                bool flag = Physics.Raycast(ray, out raycastHit, 150f, LayerMask.GetMask(new string[]
                {
                    "Terrian"
                }));
                if (flag)
                {
                    averageDir += raycastHit.normal;
                    num++;
                }
            }
            averageDir /= (float)num;
        }
    }

    private void ClearWays()
    {
        List<uint> list = new List<uint>(this.wayPoints.Keys);
        while (list.Count > 0)
        {
            if (this.wayPoints[list[0]].pointObj)
            {
                UnityEngine.Object.DestroyImmediate(this.wayPoints[list[0]].pointObj);
            }
            list.RemoveAt(0);
        }
        this.wayPoints.Clear();
        this.isStartCheck = false;
    }

    private void ColorSingleWayPoint(Color c, uint index)
    {
        if (this.wayPoints.ContainsKey(index))
        {
            Material material = this.wayPoints[index].pointObj.GetComponentInChildren<Projector>().material;
            material.color = c;
        }
    }

    private void ColorWayPoint(Color c, bool isChangeValidWp = true)
    {
        uint num = 1U;
        while ((ulong)num <= (ulong)((long)this.wayPoints.Count))
        {
            Material material = this.wayPoints[num].pointObj.GetComponentInChildren<Projector>().material;
            if (isChangeValidWp)
            {
                if (this.wayPoints[num].isValid)
                {
                    material.color = c;
                }
            }
            else
            {
                material.color = c;
            }
            num += 1U;
        }
    }

    public void GmTest(string gmOrder)
    {
        string[] array = gmOrder.Replace("//wc ", string.Empty).Split(new char[]
        {
            ' '
        });
        int num = int.Parse(array[0].Split(new char[]
        {
            '='
        })[1]) * 2;
        int num2 = int.Parse(array[1].Split(new char[]
        {
            '='
        })[1]) * 2;
        int num3 = num / int.Parse(array[2].Split(new char[]
        {
            '='
        })[1]);
        int num4 = num2 / int.Parse(array[3].Split(new char[]
        {
            '='
        })[1]);
        uint[] ways = null;
        if (array.Length >= 5)
        {
            string[] array2 = array[4].Replace("[", string.Empty).Replace("]", string.Empty).Split(new char[]
            {
                '-'
            });
            List<uint> list = new List<uint>();
            for (int i = 0; i < array2.Length; i++)
            {
                list.Add(uint.Parse(array2[i]));
            }
            ways = list.ToArray();
        }
        this.isNeedCheckWPInLocal = true;
        this.CreateCheckBoxsErea(MainPlayer.Self.NextPosition2D.x, MainPlayer.Self.NextPosition2D.y, (float)num, (float)num2, (float)num3, (float)num4, ways, true);
    }

    public void OnGetWayGrid(MSG_Ret_Rondom_Way_SC msg)
    {
        this.isNeedCheckWPInLocal = true;
        uint num = msg.zonewidth * 2U;
        uint num2 = msg.zoneheight * 2U;
        uint num3 = num / msg.gridwidth;
        uint num4 = num2 / msg.gridheight;
        string[] array = msg.pathinfo.Split(new char[]
        {
            '-'
        });
        List<uint> list = new List<uint>();
        for (int i = 0; i < array.Length; i++)
        {
            list.Add(uint.Parse(array[i].Trim()));
        }
        uint[] ways = list.ToArray();
        this.CreateCheckBoxsErea(msg.zonecenterx, msg.zonecentery, num, num2, num3, num4, ways, false);
    }

    private Color wayDefaultColor = Color.grey;

    private Color wayTipColor = Color.green;

    private Color wayRightColor = Color.green;

    private Color wayWrongColor = Color.red;

    private Color wayFirstTipColor = Color.green;

    private bool isStartCheck;

    private bool isNeedCheckWPInLocal;

    private bool showWayFlag;

    private bool isWalkToFirst;

    private GameObject wayRoot_;

    private float wpSizeX;

    private float wpSizeY;

    private Dictionary<uint, WayPointCheck> wayPoints = new Dictionary<uint, WayPointCheck>();

    private float showWpTipTimer = 0.5f;

    private float showWpTipTimerTem;

    private int showTipIndex;

    private List<uint> validWpList = new List<uint>();
}
