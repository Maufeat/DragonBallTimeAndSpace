using System;
using System.Collections.Generic;
using Models;

public class MatchPasswordController : ControllerBase
{
    public override void Awake()
    {
        this.Init();
    }

    public void Init()
    {
        LuaProcess.RegisertFunction(new Action<List<VarType>>(this.OpenMatchPasswordUI));
    }

    public void OpenMatchPasswordUI(List<VarType> vtl)
    {
        UIManager.Instance.ShowUI<UI_MatchPassword>("UI_MatchPassword", delegate ()
        {
        }, UIManager.ParentType.CommonUI, false);
        this.vtl = vtl;
    }

    public List<int> RandomInputPassWord(int psCount)
    {
        this.ps.Clear();
        int num = 1;
        for (int i = 0; i < psCount; i++)
        {
            num *= 10;
        }
        Random random = new Random();
        int num2 = random.Next(0, 10000);
        int num3 = 1;
        for (int j = 0; j < psCount; j++)
        {
            if (j > 0)
            {
                num3 *= 10;
            }
            int item = num2 / num3 % 10;
            this.ps.Insert(0, item);
        }
        return this.ps;
    }

    public bool OnEnsure(List<int> inputPs)
    {
        if (inputPs.Count != this.ps.Count)
        {
            return false;
        }
        for (int i = 0; i < inputPs.Count; i++)
        {
            if (inputPs[i] != this.ps[i])
            {
                return false;
            }
        }
        this.ExcutQuest();
        return true;
    }

    private void ExcutQuest()
    {
        if (this.vtl != null && this.vtl.Count == 3)
        {
            GlobalRegister.ReqExecuteQuest(uint.Parse(this.vtl[1].ToString()), this.vtl[0].ToString(), uint.Parse(this.vtl[2].ToString()), 0U);
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override string ControllerName
    {
        get
        {
            return "matchpassword_controller";
        }
    }

    private List<VarType> vtl;

    private List<int> ps = new List<int>();
}
