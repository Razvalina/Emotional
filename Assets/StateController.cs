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
		//{"SplashScreen", "SelectMenuScreen" },
		{"SelectMenuScreen", "ActionPhaseScreen" },
		{"ActionPhaseScreen", "DeathScreen" }
	};
    public string CurrentScreen = "none";

    private IScreen Screen = null;
	private float ToNewScreen = 0.0f;

	// Start is called before the first frame update
	void Start()
    {
		// by def, I start a scene by name
		IScreen screen = getScreen(this.CurrentScreen);
		this.MoveToScreen(screen);
    }
	public void OnHide(IScreen screen)
	{
		this.CurrentScreen = screen.getName();
		this.ToNewScreen = 1.0f;
		this.Screen = null;
		
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
		string toStreen = this.ToScreen(this.CurrentScreen);
		foreach (GameObject go in GameScenes)
		{
			if (go.name.Contains(toStreen))
			{
				IScreen sh = go.GetComponent<IScreen>();
				return sh;
			}
		}
		return null;
	}

	private void MoveToScreen(IScreen screen)
	{
		// stop controller in screen
		if (this.Screen != null)
		{
			this.Screen.StopController();
		}

		this.Screen = screen;
		
		// activate new screen
		if (this.Screen != null)
		{
			this.Screen.ToShow(this);
			this.Screen.StartController();
		}
	}




    

    // Update is called once per frame
    void Update()
    {
        if (this.ToNewScreen > 0.0f )
		{
			this.ToNewScreen -= Time.deltaTime;
		}

		if (this.ToNewScreen < 0.0f && this.Screen == null)
		{
			IScreen screen = getScreen(this.CurrentScreen);
			this.MoveToScreen(screen);
			this.ToNewScreen = 0.0f;
		}


	}
}
