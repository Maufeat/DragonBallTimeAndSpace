using System;
using LuaInterface;
using Models;
using quest;

public class QTEController : ControllerBase
{
    private UI_QTE ui_qte
    {
        get
        {
            this.ui_qte_ = UIManager.GetUIObject<UI_QTE>();
            return this.ui_qte_;
        }
    }

    public override void Awake()
    {
        this.netWork = new QTENetWork();
        this.netWork.Initialize();
    }

    public override void OnUpdate()
    {
    }

    public void OnGetMSG_PlayBellQTE_SC(MSG_PlayBellQTE_SC msg)
    {
        float angle = 80f;
        float validangle = 40f;
        float speedbottom = 140f;
        float speededge = 10f;
        float time = 10f;
        LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("massiveConfig").GetCacheField_Table("qte");
        LuaTable field_Table = cacheField_Table.GetField_Table(msg.qtelevel.ToString());
        if (field_Table != null)
        {
            angle = float.Parse(field_Table.GetField_String("angle"));
            validangle = float.Parse(field_Table.GetField_String("validangle"));
            speedbottom = float.Parse(field_Table.GetField_String("speedbottom"));
            speededge = float.Parse(field_Table.GetField_String("speededge"));
            time = float.Parse(field_Table.GetField_String("time"));
        }
        this.curLevel = msg.qtelevel;
        if (this.ui_qte == null)
        {
            UIManager.Instance.ShowUI<UI_QTE>("UI_QTE", delegate ()
            {
                this.ui_qte.SetCheckerParam(angle, validangle, speedbottom, speededge, time);
            }, UIManager.ParentType.CommonUI, false);
        }
        else
        {
            this.ui_qte.SetCheckerParam(angle, validangle, speedbottom, speededge, time);
        }
    }

    public void ReqQteResult(uint result)
    {
        this.netWork.ReqMSG_PlayBellQTEResult_CS(this.curLevel, result);
    }

    public override string ControllerName
    {
        get
        {
            return "qte_controller";
        }
    }

    private QTENetWork netWork;

    public uint curLevel = 1U;

    private UI_QTE ui_qte_;
}
