using System;
using System.Collections.Generic;
using Framework.Managers;
using UnityEngine;

public class FogAndLightSplitBox : MonoBehaviour
{
    public event FogAndLightSplitBox.TriggerEnter m_TriggerEnter;

    public event FogAndLightSplitBox.TriggerStay m_TriggerStay;

    public event FogAndLightSplitBox.TriggerExit m_TriggerExit;

    public void Awake()
    {
        if (this.m_FogController == null)
        {
            UnityEngine.Object @object = UnityEngine.Object.FindObjectOfType(typeof(FogAndLightCenter));
            if (@object != null)
            {
                this.m_FogController = (@object as FogAndLightCenter);
                this.m_FogController.AddGroup(this);
            }
            else
            {
                Debug.LogError("can not find FogAndLightController");
            }
        }
        this.m_State = new BoxBottomState();
        string[] array = this.m_AreaName.Split(new char[]
        {
            '|'
        });
        if (array.Length < 2)
        {
            return;
        }
        foreach (object obj in base.gameObject.transform)
        {
            Transform transform = (Transform)obj;
            if (transform.name == array[0])
            {
                this.m_LeftAnchor.Add(transform);
            }
            if (transform.name == array[1])
            {
                this.m_RightAnchor.Add(transform);
            }
        }
        if (this.m_LeftAnchor.Count < 2)
        {
            return;
        }
        if (this.m_RightAnchor.Count < 2)
        {
            return;
        }
        if (Mathf.Abs(Vector3.Distance(this.m_LeftAnchor[0].position, this.m_RightAnchor[0].position) - base.GetComponent<BoxCollider>().bounds.size.x) < 0.1f || Mathf.Abs(Vector3.Distance(this.m_LeftAnchor[0].position, this.m_RightAnchor[1].position) - base.GetComponent<BoxCollider>().bounds.size.x) < 0.1f)
        {
            this.m_State.m_Dis = base.GetComponent<BoxCollider>().bounds.size.x;
        }
        else
        {
            this.m_State.m_Dis = base.GetComponent<BoxCollider>().bounds.size.z;
        }
        this.m_PlaneLeft = new Plane(this.m_LeftAnchor[0].position, this.m_LeftAnchor[1].position, new Vector3(this.m_LeftAnchor[0].position.x, this.m_LeftAnchor[0].position.y + 10f, this.m_LeftAnchor[0].position.z));
        this.m_PlaneRight = new Plane(this.m_RightAnchor[0].position, this.m_RightAnchor[1].position, new Vector3(this.m_RightAnchor[0].position.x, this.m_RightAnchor[0].position.y + 10f, this.m_RightAnchor[0].position.z));
        this.m_Valide = true;
    }

    public string GetAreaName()
    {
        return this.m_AreaName.Trim();
    }

    public void GetMainPlayer()
    {
        MainPlayer mainPlayer = ManagerCenter.Instance.GetManager<EntitiesManager>().MainPlayer;
        if (mainPlayer != null && mainPlayer.ModelObj != null)
        {
            this.m_FogController.m_MainPlayer = mainPlayer.ModelObj.transform;
        }
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (this.m_FogController.m_MainPlayer == null)
        {
            this.GetMainPlayer();
        }
        if (!this.m_Valide || this.m_FogController.m_MainPlayer == null || collider.gameObject != this.m_FogController.m_MainPlayer.gameObject)
        {
            return;
        }
        this.m_State.state = BoxBottomState.State.stay;
        if (this.m_TriggerEnter != null)
        {
            this.m_TriggerEnter(this);
        }
    }

    public void OnTriggerStay(Collider collider)
    {
        if (this.m_FogController.m_MainPlayer == null)
        {
            this.GetMainPlayer();
        }
        if (!this.m_Valide || this.m_FogController.m_MainPlayer == null || collider.gameObject != this.m_FogController.m_MainPlayer.gameObject)
        {
            return;
        }
        this.m_State.state = BoxBottomState.State.stay;
        if (this.m_TriggerStay != null)
        {
            this.m_TriggerStay(this);
        }
    }

    public void OnTriggerExit(Collider collider)
    {
        if (this.m_FogController.m_MainPlayer == null)
        {
            this.GetMainPlayer();
        }
        if (!this.m_Valide || this.m_FogController.m_MainPlayer == null || collider.gameObject != this.m_FogController.m_MainPlayer.gameObject)
        {
            return;
        }
        this.m_State.state = BoxBottomState.State.none;
        if (this.m_TriggerExit != null)
        {
            this.m_TriggerExit(this);
        }
    }

    public void CaculateBoxBottomState(Transform m_MainPlayer)
    {
        this.m_State.m_ToLeft = FogAndLightSplitBox.DisPoint2Plane(m_MainPlayer.transform.position, this.m_PlaneLeft);
        this.m_State.m_ToRight = FogAndLightSplitBox.DisPoint2Plane(m_MainPlayer.transform.position, this.m_PlaneRight);
        if (this.m_State.state != BoxBottomState.State.stay)
        {
            if (this.m_State.m_ToLeft < this.m_State.m_ToRight)
            {
                this.m_State.state = BoxBottomState.State.left;
            }
            else
            {
                this.m_State.state = BoxBottomState.State.right;
            }
        }
    }

    public BoxBottomState GetState(Transform mianPlayer)
    {
        if (this.m_State == null || !this.m_Valide)
        {
            return null;
        }
        this.CaculateBoxBottomState(mianPlayer);
        return this.m_State;
    }

    public bool GetValide()
    {
        return this.m_Valide;
    }

    public static float DisPoint2Line(Vector3 point, Vector3 linePoint1, Vector3 linePoint2)
    {
        Vector3 vector = point - linePoint1;
        Vector3 onNormal = linePoint2 - linePoint1;
        Vector3 a = Vector3.Project(vector, onNormal);
        return Mathf.Sqrt(Mathf.Pow(Vector3.Magnitude(vector), 2f) - Mathf.Pow(Vector3.Magnitude(a), 2f));
    }

    public static float DisPoint2Plane(Vector3 point, Plane p)
    {
        return Mathf.Abs(p.GetDistanceToPoint(point));
    }

    public string m_AreaName = "A|B";

    private FogAndLightCenter m_FogController;

    private List<Transform> m_LeftAnchor = new List<Transform>();

    private List<Transform> m_RightAnchor = new List<Transform>();

    private BoxBottomState m_State;

    private Plane m_PlaneLeft;

    private Plane m_PlaneRight;

    private bool m_Valide;

    public delegate void TriggerEnter(FogAndLightSplitBox bbvo);

    public delegate void TriggerStay(FogAndLightSplitBox bbvo);

    public delegate void TriggerExit(FogAndLightSplitBox bbvo);
}
