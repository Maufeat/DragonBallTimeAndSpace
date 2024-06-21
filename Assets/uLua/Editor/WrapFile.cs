using UnityEngine;
using System.Collections;
using System;
using Obj;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public static class WrapFile {

    public static BindType[] binds = new BindType[]
    {
        _GT(typeof(object)),
        _GT(typeof(System.String)),
        _GT(typeof(System.Enum)),   
        _GT(typeof(IEnumerator)),
        _GT(typeof(System.Delegate)),        
        _GT(typeof(Type)).SetBaseName("System.Object"),                                                     
        _GT(typeof(UnityEngine.Object)),
        
        //测试模板
        ////_GT(typeof(Dictionary<int,string>)).SetWrapName("DictInt2Str").SetLibName("DictInt2Str"),
        
        //custom    
		_GT(typeof(Util)),
        _GT(typeof(Debugger)),
        _GT(typeof(DelegateFactory)),
        
        //unity                        
        _GT(typeof(Component)),
        _GT(typeof(Behaviour)),
        _GT(typeof(MonoBehaviour)),        
        _GT(typeof(GameObject)),
        _GT(typeof(Transform)),
        _GT(typeof(Space)),

        _GT(typeof(Camera)),   
        _GT(typeof(CameraClearFlags)),           
        _GT(typeof(Material)),
        _GT(typeof(Renderer)),        
        _GT(typeof(MeshRenderer)),
        _GT(typeof(SkinnedMeshRenderer)),
        _GT(typeof(Light)),
        _GT(typeof(LightType)),     
        _GT(typeof(ParticleEmitter)),
        _GT(typeof(ParticleRenderer)),
        _GT(typeof(ParticleAnimator)),        
                
        _GT(typeof(Physics)),
        _GT(typeof(Collider)),
        _GT(typeof(BoxCollider)),
        _GT(typeof(MeshCollider)),
        _GT(typeof(SphereCollider)),
        
        _GT(typeof(CharacterController)),

        _GT(typeof(Animation)),             
        _GT(typeof(AnimationClip)).SetBaseName("UnityEngine.Object"),
        _GT(typeof(TrackedReference)),
        _GT(typeof(AnimationState)),  
        _GT(typeof(QueueMode)),  
        _GT(typeof(PlayMode)),                  
        
        _GT(typeof(AudioClip)),
        _GT(typeof(AudioSource)),                
        
        _GT(typeof(Application)),
        _GT(typeof(Input)),    
        _GT(typeof(TouchPhase)),            
        _GT(typeof(KeyCode)),             
        _GT(typeof(Screen)),
        _GT(typeof(Time)),
        _GT(typeof(RenderSettings)),
        _GT(typeof(SleepTimeout)),        

        _GT(typeof(AsyncOperation)).SetBaseName("System.Object"),
        _GT(typeof(AssetBundle)),   
        _GT(typeof(BlendWeights)),   
        _GT(typeof(QualitySettings)),          
        _GT(typeof(AnimationBlendMode)),    
        _GT(typeof(Texture)),
        _GT(typeof(RenderTexture)),
        _GT(typeof(ParticleSystem)),

        // Unity UI
        _GT(typeof(Button)).SetWrapName("UnityEngine_UI_Button"),
        _GT(typeof(Button.ButtonClickedEvent)).SetWrapName("UnityEngine_UI_Button_ButtonClickedEvent"),
        _GT(typeof(Dropdown)).SetWrapName("UnityEngine_UI_Dropdown"),
        _GT(typeof(Graphic)).SetWrapName("UnityEngine_UI_Graphic"),
        _GT(typeof(GridLayoutGroup)).SetWrapName("UnityEngine_UI_GridLayoutGroup"),
        _GT(typeof(HorizontalLayoutGroup)).SetWrapName("UnityEngine_UI_HorizontalLayoutGroup"),
        _GT(typeof(HorizontalOrVerticalLayoutGroup)).SetWrapName("UnityEngine_UI_HorizontalOrVerticalLayoutGroup"),
        _GT(typeof(Image)).SetWrapName("UnityEngine_UI_Image"),
        _GT(typeof(InputField)).SetWrapName("UnityEngine_UI_InputField"),
        _GT(typeof(InputField.OnChangeEvent)).SetWrapName("UnityEngine_UI_InputField_OnChangeEvent"),
        _GT(typeof(InputField.SubmitEvent)).SetWrapName("UnityEngine_UI_InputField_SubmitEvent"),
        _GT(typeof(LayoutElement)).SetWrapName("UnityEngine_UI_LayoutElement"),
        _GT(typeof(LayoutGroup)).SetWrapName("UnityEngine_UI_LayoutGroup"),
        _GT(typeof(LayoutRebuilder)).SetWrapName("UnityEngine_UI_LayoutRebuilder"),
        _GT(typeof(MaskableGraphic)).SetWrapName("UnityEngine_UI_MaskableGraphic"),
        _GT(typeof(RawImage)).SetWrapName("UnityEngine_UI_RawImage"),
        _GT(typeof(Scrollbar)).SetWrapName("UnityEngine_UI_Scrollbar"),
        _GT(typeof(Scrollbar.ScrollEvent)).SetWrapName("UnityEngine_UI_Scrollbar_ScrollEvent"),
        _GT(typeof(ScrollRect)).SetWrapName("UnityEngine_UI_ScrollRect"),
        _GT(typeof(Selectable)).SetWrapName("UnityEngine_UI_Selectable"),
        _GT(typeof(Slider)).SetWrapName("UnityEngine_UI_Slider"),
        _GT(typeof(Slider.SliderEvent)).SetWrapName("UnityEngine_UI_Slider_SliderEvent"),
        _GT(typeof(Text)).SetWrapName("UnityEngine_UI_Text"),
        _GT(typeof(ToggleGroup)).SetWrapName("UnityEngine_UI_ToggleGroup"),
        _GT(typeof(Toggle)).SetWrapName("UnityEngine_UI_Toggle"),
        _GT(typeof(Toggle.ToggleEvent)).SetWrapName("UnityEngine_UI_Toggle_ToggleEvent"),
        _GT(typeof(VerticalLayoutGroup)).SetWrapName("UnityEngine_UI_VerticalLayoutGroup"),

        // Unity Additions
        _GT(typeof(Sprite)),
        _GT(typeof(RichText)),
        _GT(typeof(RectTransform)),
        _GT(typeof(Texture2D)),
        _GT(typeof(EventSystem)).SetWrapName("UnityEngine_EventSystems_EventSystem"),
        _GT(typeof(UIBehaviour)).SetWrapName("UnityEngine_EventSystems_UIBehaviour"),
        _GT(typeof(UnityEventBase)),
        _GT(typeof(UnityEvent)),
        _GT(typeof(UnityEventHelper)),
        _GT(typeof(Mesh)),

        // DOTween
        _GT(typeof(TweenAlpha)),
        _GT(typeof(TweenColor)),
        _GT(typeof(TweenPositionDirection)),
        _GT(typeof(TweenPosition)),
        _GT(typeof(TweenScale)),

        // Dragon Ball Specific Wrappers
        _GT(typeof(CanvasGroup)),
        _GT(typeof(CharactorBase)),
        _GT(typeof(CommonItem)),
        _GT(typeof(ConstClient)),
        _GT(typeof(Const)),
        _GT(typeof(CountDownItem)),
        _GT(typeof(cs_CharacterMapData)),
        _GT(typeof(cs_CharacterMapShow)),
        _GT(typeof(cs_MapUserData)),
        _GT(typeof(EntitiesID)),
        _GT(typeof(EntitiesManager)),
        _GT(typeof(GameSystemSettings)),
        _GT(typeof(GameTime)),
        _GT(typeof(GlobalRegister)),
        _GT(typeof(IgnoreTimeScale)),
        _GT(typeof(ImageSeperateWithAlpha)),
        _GT(typeof(InputTextChecker)),
        _GT(typeof(IrregularGridLayOut)),
        _GT(typeof(KeyWordFilter)),
        _GT(typeof(LuaConfigManager)),
        _GT(typeof(LuaNetWorkManager)),
        _GT(typeof(LuaPanelBase)),
        _GT(typeof(MainPlayer)),
        _GT(typeof(ManagerRegister)),
        _GT(typeof(MeshFilter)),
        _GT(typeof(NetWorkBase)),
        _GT(typeof(NpctalkRawCharactorMgr)),
        _GT(typeof(EquipData)),
        _GT(typeof(EquipRandInfo)),
        _GT(typeof(t_Object)),
        _GT(typeof(t_TidyPackInfo)),
        _GT(typeof(OtherPlayer)),
        _GT(typeof(PropsBase)),
        _GT(typeof(RawCharactorMgr)),
        _GT(typeof(RenderTextureMgr)),
        _GT(typeof(RuneDataSetattribute)),
        _GT(typeof(SystemHelper)),
        _GT(typeof(TipsWindow)),
        _GT(typeof(TypewriterEffect)),
        _GT(typeof(UICenterOnChild)),
        _GT(typeof(UICirculationList)),
        _GT(typeof(UIFoldItem)),
        _GT(typeof(UIFoldOutList)),
        _GT(typeof(UIList)),
        _GT(typeof(UIManager)),
        _GT(typeof(UIManager.ParentType)),
        _GT(typeof(UITextureAsset)),
        _GT(typeof(UITextureMgr)),
        _GT(typeof(UITweener)),
        _GT(typeof(UI_NpcDlg)),
        _GT(typeof(WayFindItem)),
        _GT(typeof(WayNodeType)),
        _GT(typeof(WayNode)),
    };

    public static BindType _GT(Type t) {
        return new BindType(t);
    }
}
