  ____
 | __ ) _ __ _____      _____  ___ _ __
 |  _ \| '__/ _ \ \ /\ / / __|/ _ \ '__|
 | |_) | | | (_) \ V  V /\__ \  __/ |
 |____/|_|  \___/ \_/\_/ |___/\___|_|

Embed a full-stack Chromium browser into your game.

Copyright 2015, 2016 Zen Fulcrum LLC
https://www.assetstore.unity3d.com/#!/content/55459

Need help? support@zenfulcrum.com

===========
Quick Start
===========

Basic Browser:
	Import ZFBrowser into your project.
	Grab ZFBrowser/Prefabs/BrowserQuad and drop it into your scene.
	On this new object, set the URL to a site of your choice, such as http://google.com/
	Fiddle with your camera so the quad fills most of the view.
	Hit play and interact with the web site.

Embedded assets:
	Start with the scene above.
	Create a folder called "BrowserAssets" right next to (not inside!) your "Assets" folder.
	Create a file called "index.html" inside "BrowserAssets" and put this in it: "<h1>Hello World!</h1>"
	Set the URL for the BrowserQuad to "localGame://index.html"
	Hit play.

Congratulations! You now have a browser in your game!


=========
Important
=========

This library is made possible by a number of third-party open source software projects.

Include a copy of ThirdPartyNotices.txt with your distributed product.


===================
Supported Platforms
===================

This asset allows you to embed a full copy of Chromium, the open-source core of Google Chrome, inside your game. This plugin is available for the given platforms and architectures:

	- Windows Standalone, 32- and 64-bit
	- OS X Standalone, 64-bit

Browsers can be previewed when running inside the Unity Editor on the following platforms:

	- Unity Editor, Windows Standalone, 32- and 64-bit
	- Unity Editor, OS X Standalone, 64-bit

A 64-bit Linux port is partially completed but too unstable for distribution at this time. If you're excited to see this reach completion, send me an email: info@zenfulcrum.com.


=========
Scripting
=========

You can send and receive messages from the currently loaded browser page.

	-----------------------
	A word about JavaScript
	-----------------------

		Browsers run JavaScript, specifically, ECMAScript. JavaScript has a .js extension.
		Unity runs .NET languages, in particular: C# (.cs), Boo (.boo), and UnityScript (.js). Due to a terrible historical mistake, UnityScript has been, and often still is, called "JavaScript". To be clear:

			UnityScript IS NOT JavaScript.

		In this package all references to "JavaScript" refer to what web browsers run.
		Beware that UnityScript and JavaScript files both have the same .js extension. In general, files in Assets/ with a .js extension will be treated as UnityScript and files in BrowserAssets/ with a .js extension will be treated as JavaScript.

Browsers have a Browser MonoBehaviour. You can send messages to the page by invoking CallFunction:

	//calls window.setPlayerStats("myPlayerName", 2, 4200, false) in the browser
	browser.CallFunction("setPlayerStats", "myPlayerName", 2, 4200, false);

Functions are resolved in the global scope of the top frame of the browser and executed with the given arguments. You can pass basic types (strings, integers, bools, etc.) without any extra effort.

To receive data or events from the page, you need to expose a function callback. To expose a callback call RegisterFunction:

	browser.RegisterFunction("confirmClicked", args => Debug.Log("Button clicked: " + args[0]));

Then you can call it from your page:

	<!-- When this is clicked, Unity will log "Button clicked: button3" -->
	<button onclick="confirmClicked('button3')">Confirm Things</button>


Information:
	- All calls are asynchronous. If you want results from a function set up a callback and call it when the data is available.
	- If you'd like to pass something more complex than a basic type, build a JSONNode. A JSONNode allows you to build and access JSON data using a fairly fluent language-native interface. Take a look at the documentation in JSONNode.cs.
	- If you'd like to pass something yet still more complex, or have data automatically read from your objects' fields:
		- You can use Unity's JSON serialization (http://docs.unity3d.com/Manual/JSONSerialization.html) to pack and unpack data on the Unity side.
		- You can use JSON.stringify/parse (https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/JSON) on the JavaScript/browser side.
	- You can also evaluate raw JavaScript in the browser by calling browser.EvalJS
	- If the Browser isn't ready for commands or the page isn't loaded yet, it will queue your commands and execute them then the page is loaded.
	- The JavaScript execution engine is V8. Not only is JavaScript support robust and complete, you can also use some es6 features. http://es6-features.org/
	- Note that functions you expose won't be available at the time of the HTML load event. If you need to know when the page loads, use browser.WhenLoaded (Unity side).
	- Take a look at the included demo for complete examples of how to work with bi-directional communication between the browser and Unity.


==================
Embedded Resources
==================

You can embed web resources (HTML, JS, CSS, images, etc.) with your game and access them without the need for an external web server.

Place these resources in a folder named BrowserAssets next to your Assets folder.

Information:
	- Access these embedded browser assets with "localGame://index.html" instead of the usual "http://example.com/index.html".
	- localGame:// is treated like a unique domain and is subject to same-domain policies like other websites.
		- Remember: If you want to run scripts on arbitrary websites, you can open a browser to that website and inject JavaScript with browser.EvalJS.
	- If you want to customize or change how these resources are fetched, read the documentation in WebResources.cs.
	- In the editor, you can simply refresh a page to see updated changes for changed files.
	- File names are case-sensitive! Test your standalone build. Under the Editor some platforms may incorrectly load wRonGcapS successfully.
	- When you build, all browser assets are packaged up as a single file and included with the build.
	- localGame:// is not the actual URL the browser uses, so don't hardcode it in your HTML. (The real URL is currently https://game.local/, but may change.)
	- Avoid putting untrusted code in BrowserAssets
	- You may need to URL-encode your path if it contatins non-alphanumeric characters: https://en.wikipedia.org/wiki/Percent-encoding

=============
Browser Input
=============

All input events are collected in Unity and forwarded to the underlying browser. How this input is sent can be customized.

Browser Input
	- Mouse clicks and keyboard events are fed to the Browser via the IBrowserUI interface. Additionally, the IBrowserUI informs us about mouse cursor changes from the page.
	- This package comes with a few useful versions of IBrowserUI out of the box:
		- ClickMeshBrowserUI (used by default) - Lets you click and interact with browsers with a free mouse hovering over the scene.
			- Requires a mesh collider.
			- It projects the mouse's position into the scene with the main camera and a physics raycast.
			- UVs on the mesh collider are used to look up the "in-browser" mouse position.
			- If the mouse if over the browser it is considered focused and will receive keyboard input.
			- Automatically updates the mouse cursor to reflect the currently hovered area.
			- Used by default, so long as the browser has a MeshCollider.
		- FPSBrowserUI - Lets a first-person controlled character interact with items in a scene by walking up and pointing.
			- Extends ClickMeshBrowserUI, but:
				- Mouse position is inferred from the direction the camera is pointing.
				- Creates a single FPSCursorRenderer in the scene to render the central cursor and crosshair for you.
			- Attach to a browser to use it. See the demo for a working example. A prefab is also included.
		- GUIBrowserUI - For use with full-screen browsers and browsers alongside non-browser UI elements.
			- Operates inside a Unity UI Canvas. Renders to a RawImage and intercepts input like other UI elements.
			- Drop the BrowserGUI prefab into a canvas element to use.
	- You can always create your own input manager by extending one of these or by implementing IBrowserUI outright. Set browser.UIHandler to your custom implementation before the first call to browser.Update(). You can also set it to null, which will disable all input.
	- Mouse scroll speed and double-click handling options can be adjusted by altering myBrowserUI.InputSettings.


=======
General
=======

Information:
	- You can customize which browser items the user is allowed to right-click (and get a context menu on). Look in the inspector under "Allow Context Menu On". By default, only text boxes will allow a context menu.
	- By default, the <body> of pages will be rendered as transparent. You can change to from fully transparent to any fully opaque color by changing the "Background Color" option in the inspector. The color must be changed before the first Update().
	- Custom mouse cursors are supported via CSS cursors (https://developer.mozilla.org/en-US/docs/Web/CSS/cursor).
	- You can control what happens when navigation attempts to open a new window. Change browser.newWindowAction to a value of your choice. If you'd like to allow opening new windows inside the game world, check out the NewWindow demo.
	- We use a user-agent that makes most websites think we're Chrome, but it does identify the engine, library, and your application's name and version. Change your application's name and version in the Player Settings. Restart the editor to see changes.
	- This system uses Chromium's separate process rendering architecture. It is normal to see one or more "ZFGameBrowser" child processes once a browser has been opened.
	- A number of events, such as page load error, are available for you to hook into. Search Browser.cs for "public event" and read the documentation above each.
	- Disabling the Browser MonoBehaviour will not stop the underlying browser from running. Destroy the object or load a blank page to free up resources a page is using.
	- When a browser’s image changes, we re-upload the texture to the GPU. Generating mipmaps is expensive, so the included prefabs turn off mipmap generation and use a special mipmap-emulating shader that "fakes" a texture with mipmaps, keeping ugly pixelation at bay for a minimal cost. Check out the Materials/EmulatedMipmap folder.
	- OS X users: The Asset Store strips the executable bit off our child process. After importing the Asset, run a browser in the editor's playmode at least once before exporting a build. This will ensure the permission gets added back so the system can run.


=============
Customization
=============

Information:
	- Customize alert() dialogs, prompt() dialogs, beforeUnload dialogs and context menus by editing ZFBrowser/Resources/Browser/Dialogs.html
	- Customize error pages by editing ZFBrowser/Resources/Browser/Errors.html
		- Hook these events by listening to Browser.onLoadError/onCertError/onSadTab.
	- Mouse cursors are:
		- Stored in ZFBrowser/Resources/Browser/Cursors.png. (These are more-or-less the base Chromium icons.)
		- Loaded and managed by the BrowserCursor class.
		- To change how a current cursor looks without altering the size, just edit Cursors.png.
		- If you wish to change the cursor sizes or which cursor types map to which Cursors.png item, take a look at IconGenerator.cs. (This script isn't intended for general use, so be prepared to get your hands dirty.)


===============
Debugging Pages
===============

Often during development, you will find your pages need debugging.

Here's a few way to go about it:
	- Debug with page inspector:
		- Pop-up page inspector: You can access the Webkit/Chromium inspector, which opens in a new window beside the running game.
			- Under the Editor in play mode: Select the Browser, then click "Show Dev Tools"
			- You can also use browser.ShowDevTools() to open an inspector window.
		- External inspector: You can access the page inspector from another browser.
			- To debug when running in the editor, visit http://localhost:9848/
			- To debug when running a debug build, visit http://localhost:9849/
			- Release builds cannot be debugged using this method.
			- Use a Chromium-based browser to load the above URLs, such as Chrome. Other browsers are unlikely to work.
		- This inspector can be a bit unstable at times. If you have issues, try the next option.
	- Debug with an ordinary browser:
		- You can also load your pages in an ordinary browser and debug them there. If you do, you may find it useful to create small "log what happened" stubs for the functions you expose from Unity. Call functions you want to test from the JavaScript console.
	- Don't forget: you don't always have to restart the game to test a change, in some cases you can simply refresh the page. (Unity inspector -> browser instance -> Refresh or Pop-up inspector -> ctrl+r).


===================
Latency/Performance
===================

The browser is rendered offscreen in a separate process and the resulting pixels are transferred to a texture. Performance will not be as good as native Chromium, but should be fine when using either small textures or minimal animation.

Latency:
	- All activity (mouse clicks, JS calls, input) is sent asynchronously to a separate renderer process and the results are asynchronously reported back. As such, there is a short delay between when a command is issued and when the results are visible. The delay should be quite reasonable in most cases, but don't expect results the same frame you issued a command.
	- Avoid using a Browser for latency-critical use cases such as rhythm games, world-aligned HUD markers, and rendering mouse cursors.

Performance:
	- Try to use small (texture size) Browsers for animations and things that frequently update. Large sizes are more suitable for static displays and user interfaces.
	- Frame updates for large (resolution) browsers are expensive. Often the final image is composited on the GPU, pulled back to the CPU, then pushed to the GPU again from a different process. Depending on the hardware, size of texture, and frequency of updates, this can impact framerates poorly.
	- Remember that on almost all systems the browsers use the same GPU for rendering that Unity does. If you have heavy content, be sure to factor this into your rendering budget.

Special note:
	- When running under the Unity Editor in OS X or Linux, an extra layer of indirection must be used to avoid library conflicts. This reduces performance in the editor. Run a standalone build to see actual performance.


============
Known issues
============

	- Support for the inspector is experimental, it can be unstable at times.
	- On rare occasion the editor may crash/hang when closing/switching projects or opening pop-up windows.


=====
Ideas
=====

- Build your complicated crafting UI in HTML
- Replace your HUD with a browser page
- Door access/control panels
- In-game billboard advertisements


====
Help
====

Need help? support@zenfulcrum.com
