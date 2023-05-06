using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class StateController : MonoBehaviour
{
    public List<GameObject> GameScenes = new List<GameObject>();

    private Dictionary<string, string> redirections = new Dictionary<string, string>()
    {
        {"none", "SplashScreen" },
		{"SplashScreen", "LoadingScreen" },
		{"LoadingScreen", "ActionPhaseScreen" },
		//{"SplashScreen", "SelectMenuScreen" },
		{"SelectMenuScreen", "ActionPhaseScreen" },
		{"ActionPhaseScreen", "DeathScreen" }
	};
    public string CurrentScreen = "none";

    private IScreen Screen = null;

	// Start is called before the first frame update
	void Start()
    {
		// by def, I start a scene by name
		IScreen screen = getScreen(this.CurrentScreen);
		this.MoveToScreen(screen);
    }
	public void OnHide(IScreen screen)
	{
		this.CurrentScreen = this.ToScreen(screen.getName());
		screen = getScreen(this.CurrentScreen);
		this.MoveToScreen(screen);

	}

	private string ToScreen(string from)
    {
        if (!this.redirections.ContainsKey(from))
        {
            from = "none";//just start
		}

		return this.redirections[from];
    }
    private IScreen getScreen(string name)
    {
		foreach (GameObject go in GameScenes)
		{
			if (go.name.Contains(name))
			{
				IScreen sh = go.GetComponent<IScreen>();
				return sh;
			}
		}

		return this.getScreen("SplashScreen");
	}

	private void MoveToScreen(IScreen screen)
	{
		this.Screen = screen;

		// activate new screen
		if (this.Screen != null)
		{
			this.Screen.ToShow(this);
		}
	}




    

    // Update is called once per frame
    void Update()
    {

	}
}
