using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet_fly : MonoBehaviour
{
    public Player player;
    public Unit unit;
    public Skill skill;

    public float Power;
    public float LifeRemain;
    public Vector2 Direction;
    public float Speed = 1.0f;
    public float Boost = 1.0f;

    bool isReady = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init()
    {
        this.LifeRemain = this.skill.LifeTime;
        this.Direction = this.player.Direction;
        this.Direction.Normalize();
        this.Boost = this.skill.Boost;
        this.Speed /= this.Boost;

        this.transform.localScale *= this.Boost;

		this.Speed = this.skill.Speed;// projectiles only
        this.isReady = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (!this.isReady)
            return;

        this.transform.localPosition += new Vector3( this.Direction.x, this.Direction.y, 0.0f) * this.Speed;
        this.LifeRemain -= Time.deltaTime;
        if (this.LifeRemain < 0.0f)
        {
			this.isReady = false;
			Destroy(this.gameObject);

		}

    }
}
