using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using UnityEngine;
using UnityEngine.UI;

public class UI_QTE : UIPanelBase
{
    private bool isInCheckState
    {
        get
        {
            return this.isInCheckState_;
        }
        set
        {
            this.isInCheckState_ = value;
            if (!this.isInCheckState_)
            {
                this.SetCurLeftTimer(-1f);
            }
        }
    }

    private QTEController qteController
    {
        get
        {
            if (this.qteController_ == null)
            {
                this.qteController_ = ControllerManager.Instance.GetController<QTEController>();
            }
            return this.qteController_;
        }
    }

    public override void OnInit(Transform root)
    {
        this.ui_root = root;
        Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.Update));
        this.InitObject();
        this.InitEvent();
    }

    private void InitObject()
    {
        this.qteRoot = this.ui_root.Find("Offset_QTE/Panel_pendulum");
        this.tipsRoot = this.ui_root.Find("Offset_QTE/Panel_tips");
        this.bgRadio = this.ui_root.Find("Offset_QTE/Panel_pendulum/bg_radio");
        this.bgValid = this.ui_root.Find("Offset_QTE/Panel_pendulum/bg_valid");
        this.bgPointer = this.ui_root.Find("Offset_QTE/Panel_pendulum/bg_pointer");
        this.timerTr = this.ui_root.Find("Offset_QTE/Panel_tips/Text_time_count");
    }

    private void InitEvent()
    {
    }

    private void SetPanelAlpha(Transform node, float a = 0.5f)
    {
        node.gameObject.SetActive(true);
        Graphic component = node.GetComponent<Graphic>();
        if (component != null)
        {
            component.color = new Color(component.color.r, component.color.g, component.color.b, a);
            if (node.transform.childCount > 0)
            {
                for (int i = 0; i < node.childCount; i++)
                {
                    this.SetPanelAlpha(node.GetChild(i), a);
                }
            }
        }
    }

    private void PlayTipAnimation(Transform node, bool isPlayOrStop = true)
    {
        if (node.transform.childCount > 0)
        {
            for (int i = 0; i < node.childCount; i++)
            {
                UITweener component = node.GetChild(i).GetComponent<UITweener>();
                if (component != null)
                {
                    component.Reset();
                    Graphic component2 = component.gameObject.GetComponent<Graphic>();
                    if (component2 != null)
                    {
                        Color color = component2.color;
                        color.a = 1f;
                        component2.color = color;
                    }
                    if (!isPlayOrStop)
                    {
                        component.enabled = false;
                    }
                    else
                    {
                        component.Play(isPlayOrStop);
                    }
                }
                this.PlayTipAnimation(node.GetChild(i), isPlayOrStop);
            }
        }
    }

    private void SetTips(bool isNeedCheckAutoClose, params int[] showIndex)
    {
        List<int> list = new List<int>();
        if (showIndex != null && showIndex.Length > 0)
        {
            for (int i = 0; i < showIndex.Length; i++)
            {
                list.Add(showIndex[i]);
            }
            this.tipsRoot.gameObject.SetActive(true);
            if (isNeedCheckAutoClose)
            {
                Scheduler.Instance.AddTimer(5f, false, delegate
                {
                    this.tipsRoot.gameObject.SetActive(false);
                });
            }
        }
        else
        {
            this.tipsRoot.gameObject.SetActive(false);
        }
        for (int j = 0; j < this.tipsRoot.childCount; j++)
        {
            Transform child = this.tipsRoot.GetChild(j);
            child.gameObject.SetActive(list.Contains(j));
        }
    }

    public void SetCheckerParam(float angleRange, float validRange, float bottomSpeed, float edgeSpeed, float length = 5f)
    {
        this.checkDuration = length;
        this.rotateBottomSpeed = bottomSpeed;
        this.rotateEdgeSpeed = edgeSpeed;
        this.angleRange = angleRange;
        this.validRange = validRange;
        float num = angleRange / 360f;
        float fillAmount = validRange / 360f;
        this.bgValid.GetComponent<Image>().fillAmount = fillAmount;
        Quaternion quaternion = Quaternion.Euler(0f, 0f, angleRange / 2f);
        Quaternion rotation = Quaternion.Euler(0f, 0f, validRange / 2f);
        this.bgValid.transform.rotation = rotation;
        this.rotateDir = ((UnityEngine.Random.value <= 0.5f) ? 1 : -1);
        this.bgPointer.transform.rotation = Quaternion.Euler(0f, 0f, (float)(-(float)this.rotateDir) * angleRange / 2f);
        this.SetPanelAlpha(this.qteRoot, 1f);
        this.SetTips(false, new int[]
        {
            1
        });
        this.PlayTipAnimation(this.tipsRoot, true);
        Scheduler.Instance.AddTimer(4f, false, delegate
        {
            this.timer = 0f;
            this.isInCheckState = true;
            this.isTriggerQte = false;
            this.SetTips(false, new int[1]);
            this.SetPanelAlpha(this.qteRoot, 1f);
            this.PlayTipAnimation(this.tipsRoot, false);
        });
    }

    private void Update()
    {
        if (this.isInCheckState)
        {
            if (this.timer >= this.checkDuration)
            {
                this.timer = 0f;
                if (!this.isTriggerQte)
                {
                    this.ReqQteResult(2U);
                }
                this.isInCheckState = false;
                return;
            }
            this.RotatePendulum();
            this.SetCurLeftTimer(this.checkDuration - this.timer);
            this.timer += Time.deltaTime;
        }
    }

    private void ReqQteResult(uint result)
    {
        this.isTriggerQte = true;
        this.qteController.ReqQteResult(result);
        this.qteRoot.gameObject.SetActive(false);
        MainPlayerTargetSelectMgr component = MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>();
        MainPlayerSkillHolder component2 = MainPlayer.Self.GetComponent<MainPlayerSkillHolder>();
        component2.skillAttackAutoAttack.ActiveSelf = false;
        component.SetTargetNull();
        if (result > 0U)
        {
            this.SetTips(true, new int[]
            {
                2
            });
        }
        else
        {
            this.SetTips(false, new int[0]);
        }
    }

    private void RotatePendulum()
    {
        this.angleToMiddle = Vector3.Angle(Vector3.down, this.bgPointer.up * -1f);
        float num = Mathf.Lerp(this.rotateBottomSpeed, this.rotateEdgeSpeed, Mathf.Abs(this.angleToMiddle) / (this.angleRange / 2f));
        Quaternion rotation = this.bgPointer.transform.rotation;
        Quaternion rhs = Quaternion.Euler(0f, 0f, Time.deltaTime * num * (float)this.rotateDir);
        Quaternion rotation2 = rotation * rhs;
        this.bgPointer.transform.rotation = rotation2;
        float num2 = Vector3.Angle(Vector3.down, this.bgPointer.up * -1f);
        if (num2 > this.angleRange / 2f)
        {
            this.rotateDir *= -1;
            this.bgPointer.transform.rotation = rotation;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            uint result = 1U;
            if (this.IsHitValidArea() == UI_QTE.HitResult.HitValidArea)
            {
                result = 0U;
            }
            this.ReqQteResult(result);
            this.isInCheckState = false;
        }
    }

    internal static void SetTest(string gmtext)
    {
        string[] array = gmtext.Split(new char[]
        {
            ' '
        });
        float angle = 80f;
        float validangle = 40f;
        float speedbottom = 140f;
        float speededge = 10f;
        float time = 10f;
        if (array.Length <= 2)
        {
            LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("massiveConfig").GetCacheField_Table("qte");
            LuaTable field_Table = cacheField_Table.GetField_Table(array[1]);
            if (field_Table != null)
            {
                angle = float.Parse(field_Table.GetField_String("angle"));
                validangle = float.Parse(field_Table.GetField_String("validangle"));
                speedbottom = float.Parse(field_Table.GetField_String("speedbottom"));
                speededge = float.Parse(field_Table.GetField_String("speededge"));
                time = float.Parse(field_Table.GetField_String("time"));
            }
        }
        else
        {
            angle = float.Parse(array[1].Split(new char[]
            {
                '='
            })[1]);
            validangle = float.Parse(array[2].Split(new char[]
            {
                '='
            })[1]);
            speedbottom = float.Parse(array[3].Split(new char[]
            {
                '='
            })[1]);
            speededge = float.Parse(array[4].Split(new char[]
            {
                '='
            })[1]);
            time = float.Parse(array[5].Split(new char[]
            {
                '='
            })[1]);
        }
        UIManager.Instance.ShowUI<UI_QTE>("UI_QTE", delegate ()
        {
            UI_QTE uiobject = UIManager.GetUIObject<UI_QTE>();
            uiobject.SetCheckerParam(angle, validangle, speedbottom, speededge, time);
        }, UIManager.ParentType.CommonUI, false);
    }

    private UI_QTE.HitResult IsHitValidArea()
    {
        if (this.isInCheckState)
        {
            return (this.angleToMiddle >= this.validRange / 2f) ? UI_QTE.HitResult.HitInValidArea : UI_QTE.HitResult.HitValidArea;
        }
        return UI_QTE.HitResult.NotInHitTime;
    }

    private void SetCurLeftTimer(float leftTime)
    {
        this.timerTr.GetComponent<Text>().text = "Left Time:" + leftTime.ToString("f1");
        if (leftTime < 0f)
        {
            this.timerTr.GetComponent<Text>().text = "Left Time:0.0";
        }
    }

    public override void OnDispose()
    {
        base.OnDispose();
        Scheduler.Instance.RemoveUpdator(new Scheduler.OnScheduler(this.Update));
    }

    private float checkDuration;

    private float timer;

    private float rotateBottomSpeed;

    private float rotateEdgeSpeed;

    private float angleRange;

    private float validRange;

    private int rotateDir = 1;

    private float angleToMiddle;

    private bool isTriggerQte;

    private bool isInCheckState_;

    private Transform ui_root;

    private Transform qteRoot;

    private Transform tipsRoot;

    private Transform bgRadio;

    private Transform bgValid;

    private Transform bgPointer;

    private Transform timerTr;

    private QTEController qteController_;

    private enum HitResult
    {
        NotInHitTime,
        HitInValidArea,
        HitValidArea
    }
}
