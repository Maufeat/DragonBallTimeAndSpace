using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace ZenFulcrum.EmbeddedBrowser { 

[RequireComponent(typeof(Browser))]
public class SimpleController : MonoBehaviour {

	private Browser browser;
	public InputField urlInput;

	public void Start() {
		browser = GetComponent<Browser>();
	}

	public void GoToURLInput() {
		browser.Url = urlInput.text;
        }

        public void GoToURL(string url)
        {
            browser.Url = url;
        }

    }

}
