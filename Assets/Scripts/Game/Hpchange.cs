using System;
using Framework.Managers;
using magic;
using UnityEngine;
using UnityEngine.UI;

public class Hpchange : MonoBehaviour
{
    public void Init(NormalObjectInPool obj)
    {
        this.Hittype = base.transform.Find("hit/img_state").GetComponent<Image>();
        this.objHit = base.transform.Find("hit").gameObject;
        this.obRevert = base.transform.Find("revert").gameObject;
        this.objRevert = base.transform.Find("revert/number/txt_num").GetComponent<Text>();
        this.objExp = base.transform.Find("exp/number/txt_num").GetComponent<Text>();
        this.objHitNormal[0] = base.gameObject.FindChild("hit/hitNumParent");
        this.objHitNormal[1] = base.gameObject.FindChild("hit/mon_hitNumParent");
        this.objHitNormal[2] = base.gameObject.FindChild("hit/pet_hitNumParent");
        this.objHitBurst[0] = base.gameObject.FindChild("hit/crinumber");
        this.objHitBurst[1] = base.gameObject.FindChild("hit/mon_crinumber");
        this.objHitBurst[2] = base.gameObject.FindChild("hit/pet_crinumber");
        TweenUtil.SetFinishedDisActive(this.Hittype.gameObject, null);
        TweenUtil.SetFinishedDisActive(this.objHit, null);
        TweenUtil.SetFinishedDisActive(this.obRevert.gameObject, null);
        TweenUtil.SetFinishedDisActive(this.objExp.gameObject, null);
        for (int i = 0; i < this.objHitNormal.Length; i++)
        {
            GameObject gameObject = this.objHitNormal[0];
            if (!(gameObject == null))
            {
                for (int j = 0; j < gameObject.transform.childCount; j++)
                {
                    TweenUtil.SetFinishedDisActive(gameObject.transform.GetChild(j).gameObject, gameObject);
                }
            }
        }
        TweenUtil.SetFinishedDisActive(this.objHitBurst[0], null);
        TweenUtil.SetFinishedDisActive(this.objHitBurst[1], null);
        TweenUtil.SetFinishedDisActive(this.objHitBurst[2], null);
        this.DisableObj();
        this.poolObj = obj;
    }

    private void DisableObj()
    {
        this.Hittype.gameObject.SetActive(false);
        this.objHit.SetActive(false);
        this.obRevert.gameObject.SetActive(false);
        this.objExp.gameObject.SetActive(false);
        for (int i = 0; i < this.objHitNormal.Length; i++)
        {
            if (this.objHitNormal[i])
            {
                this.objHitNormal[i].SetActive(false);
                for (int j = 0; j < this.objHitNormal[i].transform.childCount; j++)
                {
                    this.objHitNormal[i].transform.GetChild(j).gameObject.SetActive(false);
                }
            }
        }
        for (int k = 0; k < this.objHitBurst.Length; k++)
        {
            if (this.objHitBurst[k])
            {
                this.objHitBurst[k].SetActive(false);
            }
        }
    }

    public void Dispose()
    {
        this.RunningTime = 0f;
        TweenUtil.Reset(base.gameObject);
        this.DisableObj();
        this.bBang = false;
        this.m_Type = ATTACKRESULT.ATTACKRESULT_NONE;
    }

    private void Start()
    {
        if (Camera.main != null)
        {
            this.maincaCamera = Camera.main;
        }
        this.textSize = LuaConfigManager.GetXmlConfigTable("massiveConfig").GetCacheField_Table("TextSize").GetCacheField_Float("value");
        if (this.maincaCamera != null)
        {
            base.transform.rotation = this.maincaCamera.transform.rotation;
            float num = Vector3.Distance(base.transform.position, this.maincaCamera.transform.position);
            float num2 = num * this.textSize;
            base.transform.localScale = new Vector3(num2, num2, num2);
        }
    }

    private void Update()
    {
        if (this.maincaCamera == null || !this.maincaCamera.enabled || !this.maincaCamera.gameObject.activeSelf)
        {
            this.maincaCamera = Camera.main;
            return;
        }
        this.RunningTime += Time.deltaTime;
        if (this.maincaCamera != null)
        {
            base.transform.rotation = this.maincaCamera.transform.rotation;
            float num = Vector3.Distance(base.transform.position, this.maincaCamera.transform.position);
            float num2 = num * this.textSize;
            base.transform.localScale = new Vector3(num2, num2, num2);
        }
        if (this.RunningTime > this.PlayTime)
        {
            if (this.poolObj != null)
            {
                this.poolObj.DisableAndBackToPool(false);
                this.poolObj = null;
            }
            this.Dispose();
        }
    }

    private void Awake()
    {
    }

    public void SetViewType(ATTACKRESULT type)
    {
        this.m_Type = type;
        switch (this.m_Type)
        {
            case ATTACKRESULT.ATTACKRESULT_MISS:
                this.Hittype.sprite = ControllerManager.Instance.GetController<UIHpSystem>().GetHitSprite("fn0071");
                this.Hittype.gameObject.SetActive(true);
                return;
            case ATTACKRESULT.ATTACKRESULT_BANG:
                this.Hittype.gameObject.SetActive(false);
                return;
            case ATTACKRESULT.ATTACKRESULT_HOLD:
                this.Hittype.sprite = ControllerManager.Instance.GetController<UIHpSystem>().GetHitSprite("fn0014");
                this.Hittype.gameObject.SetActive(true);
                return;
            case ATTACKRESULT.ATTACKRESULT_BLOCK:
                this.Hittype.sprite = ControllerManager.Instance.GetController<UIHpSystem>().GetHitSprite("fn0014");
                this.Hittype.gameObject.SetActive(true);
                return;
            case ATTACKRESULT.ATTACKRESULT_DEFLECT:
                this.Hittype.sprite = ControllerManager.Instance.GetController<UIHpSystem>().GetHitSprite("fn0013");
                this.Hittype.gameObject.SetActive(true);
                return;
            case ATTACKRESULT.ATTACKRESULT_HIT:
                this.Hittype.gameObject.SetActive(false);
                return;
        }
        this.Hittype.gameObject.SetActive(false);
    }

    public void SetExp(int change)
    {
        if (change <= 0)
        {
            return;
        }
        this.objExp.gameObject.SetActive(true);
        this.objExp.text = "+" + change;
        TweenPosition component = base.gameObject.gameObject.GetComponent<TweenPosition>();
        if (component)
        {
            component.duration = 5f;
        }
        TweenUtil.Play(base.gameObject);
    }

    public void HitValue(Hpchange.EHarmType harmtype, ATTACKRESULT type, int change)
    {
        this.objHit.SetActive(true);
        if (type == ATTACKRESULT.ATTACKRESULT_BANG)
        {
            GameObject gameObject = this.objHitBurst[(int)harmtype];
            TweenUtil.SetFinishedDisActive(gameObject, null);
            gameObject.SetActive(true);
            if (change >= 0)
            {
                gameObject.transform.Find("txt_num").gameObject.SetActive(false);
            }
            else
            {
                Text text = gameObject.transform.Find("txt_num").GetComponent<Text>();
                text.gameObject.SetActive(true);
                text.text = change.ToString();
            }
        }
        else
        {
            GameObject gameObject2 = this.objHitNormal[(int)harmtype];
            gameObject2.SetActive(true);
            GameObject gameObject3 = gameObject2.transform.GetChild(UnityEngine.Random.Range(0, gameObject2.transform.childCount)).gameObject;
            TweenUtil.SetFinishedDisActive(gameObject3, null);
            gameObject3.SetActive(true);
            if (change > 0)
            {
                gameObject3.transform.Find("txt_num").gameObject.SetActive(false);
            }
            else if (change == 0)
            {
                if (type == ATTACKRESULT.ATTACKRESULT_NONE || type == ATTACKRESULT.ATTACKRESULT_NORMAL)
                {
                    Text text2 = gameObject3.transform.Find("txt_num").GetComponent<Text>();
                    text2.gameObject.SetActive(true);
                    text2.text = "-0";
                }
                else
                {
                    gameObject3.transform.Find("txt_num").gameObject.SetActive(false);
                }
            }
            else
            {
                Text text3 = gameObject3.transform.Find("txt_num").GetComponent<Text>();
                text3.gameObject.SetActive(true);
                text3.text = change.ToString();
            }
            TweenUtil.SetEndPosition(gameObject3);
        }
        TweenUtil.Play(base.gameObject);
    }

    public void SetRevertValue(int change)
    {
        if (change <= 0)
        {
            return;
        }
        base.transform.name = change.ToString();
        this.obRevert.gameObject.SetActive(true);
        this.objRevert.text = "+" + change.ToString();
        TweenUtil.Play(base.gameObject);
    }

    public bool bBang;

    private bool m_bRevert;

    private float textSize = 0.1f;

    private NormalObjectInPool poolObj;

    public Camera maincaCamera;

    public float PlayTime = 1.8f;

    public Image Hittype;

    private float RunningTime;

    public ATTACKRESULT m_Type;

    private GameObject objHit;

    private GameObject obRevert;

    private Text objRevert;

    private Text objExp;

    private GameObject[] objHitNormal = new GameObject[3];

    private GameObject[] objHitBurst = new GameObject[3];

    public enum EHarmType
    {
        PlayerAtt,
        PlayerBeAtt,
        SummonAtt
    }
}
