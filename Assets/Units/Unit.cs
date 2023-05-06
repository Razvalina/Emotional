using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[Serializable]
public class Unit : MonoBehaviour
{
    public Character character;
    public List<Skill> Skills = new List<Skill>();
    public Skill ActiveSkill =null;

    public Skill UltSkill;

    public bool isDamageDriver = true;
    public GameObject TargetGo;
    public GameObject TargetGoTarget;
   
    // Start is called before the first frame update
    void Start()
    {
        if (this.Skills != null && this.Skills.Count > 0)
        {
            this.ActiveSkill = this.Skills[0];
        }

    }

    public void onInit()
    {
		if (!this.isDamageDriver)
		{
			TargetGoTarget = Instantiate(TargetGo, Vector3.zero, Quaternion.identity, this.transform);
            TargetGoTarget.transform.localPosition = Vector3.zero;
		}
	}

    public void UpdateCharacter()
    {
        this.character.Update();

        foreach( Skill skill in this.Skills )
        {
            if (skill != null)
            {
				skill.Update();
			}
        }
            

        if (this.UltSkill != null)
            this.UltSkill.Update();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
