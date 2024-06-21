using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ZenFulcrum.EmbeddedBrowser
{
    [RequireComponent(typeof(Browser))]
    [RequireComponent(typeof(RawImage))]
    public class GUIBrowserUI : MonoBehaviour, IEventSystemHandler, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IBrowserUI
    {
        protected void Awake()
        {
            this.BrowserCursor = new BrowserCursor();
            this.InputSettings = new BrowserInputSettings();
            this.browser = base.GetComponent<Browser>();
            this.myImage = base.GetComponent<RawImage>();
            this.browser.afterResize += this.UpdateTexture;
            this.browser.UIHandler = this;
            this.BrowserCursor.cursorChange += delegate ()
            {
                this.SetCursor(this.BrowserCursor);
            };
            this.rTransform = base.GetComponent<RectTransform>();
        }

        protected void OnEnable()
        {
            base.StartCoroutine(this.WatchResize());
        }

        private IEnumerator WatchResize()
        {
            Rect currentSize = default(Rect);
            while (base.enabled)
            {
                Rect rect = this.rTransform.rect;
                if (rect.size.x <= 0f || rect.size.y <= 0f)
                {
                    rect.size = new Vector2(512f, 512f);
                }
                if (rect.size != currentSize.size)
                {
                    this.browser.Resize((int)rect.size.x, (int)rect.size.y);
                    currentSize = rect;
                }
                yield return null;
            }
            yield break;
        }

        protected void UpdateTexture(Texture2D texture)
        {
            this.myImage.texture = texture;
            this.myImage.uvRect = new Rect(0f, 0f, 1f, 1f);
        }

        public virtual void InputUpdate()
        {
            List<Event> list = this.keyEvents;
            this.keyEvents = this.keyEventsLast;
            this.keyEventsLast = list;
            this.keyEvents.Clear();
            if (this.MouseHasFocus)
            {
                if (!this.raycaster)
                {
                    this.raycaster = base.GetComponentInParent<BaseRaycaster>();
                }
                Vector2 mousePosition;
                RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)base.transform, Input.mousePosition, this.raycaster.eventCamera, out mousePosition);
                mousePosition.x = mousePosition.x / this.rTransform.rect.width + 0.5f;
                mousePosition.y = mousePosition.y / this.rTransform.rect.height + 0.5f;
                this.MousePosition = mousePosition;
                MouseButton mouseButton = (MouseButton)0;
                if (Input.GetMouseButton(0))
                {
                    mouseButton |= MouseButton.Left;
                }
                if (Input.GetMouseButton(1))
                {
                    mouseButton |= MouseButton.Right;
                }
                if (Input.GetMouseButton(2))
                {
                    mouseButton |= MouseButton.Middle;
                }
                this.MouseButtons = mouseButton;
                this.MouseScroll = Input.mouseScrollDelta;
            }
            else
            {
                this.MouseButtons = (MouseButton)0;
            }
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {
                this.keyEventsLast.Insert(0, new Event
                {
                    type = EventType.KeyDown,
                    keyCode = KeyCode.LeftShift
                });
            }
            if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
            {
                this.keyEventsLast.Add(new Event
                {
                    type = EventType.KeyUp,
                    keyCode = KeyCode.LeftShift
                });
            }
        }

        public void OnGUI()
        {
            Event current = Event.current;
            if (current.type != EventType.KeyDown && current.type != EventType.KeyUp)
            {
                return;
            }
            this.keyEvents.Add(new Event(current));
        }

        protected virtual void SetCursor(BrowserCursor newCursor)
        {
            if (!this._mouseHasFocus && newCursor != null)
            {
                return;
            }
            if (newCursor == null)
            {
                Cursor.visible = true;
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
            else if (newCursor.Texture != null)
            {
                Cursor.visible = true;
                Cursor.SetCursor(newCursor.Texture, newCursor.Hotspot, CursorMode.Auto);
            }
            else
            {
                Cursor.visible = false;
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
        }

        public bool MouseHasFocus
        {
            get
            {
                return this._mouseHasFocus && this.enableInput;
            }
        }

        public Vector2 MousePosition { get; private set; }

        public MouseButton MouseButtons { get; private set; }

        public Vector2 MouseScroll { get; private set; }

        public bool KeyboardHasFocus
        {
            get
            {
                return this._keyboardHasFocus && this.enableInput;
            }
        }

        public List<Event> KeyEvents
        {
            get
            {
                return this.keyEventsLast;
            }
        }

        public BrowserCursor BrowserCursor { get; private set; }

        public BrowserInputSettings InputSettings { get; private set; }

        public void OnSelect(BaseEventData eventData)
        {
            this._keyboardHasFocus = true;
        }

        public void OnDeselect(BaseEventData eventData)
        {
            this._keyboardHasFocus = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            this._mouseHasFocus = true;
            this.SetCursor(this.BrowserCursor);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            this._mouseHasFocus = false;
            this.SetCursor(null);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            EventSystem.current.SetSelectedGameObject(base.gameObject);
        }

        protected RawImage myImage;

        protected Browser browser;

        public bool enableInput = true;

        protected List<Event> keyEvents = new List<Event>();

        protected List<Event> keyEventsLast = new List<Event>();

        public BaseRaycaster raycaster;

        protected RectTransform rTransform;

        protected bool _mouseHasFocus;

        protected bool _keyboardHasFocus;
    }
}
