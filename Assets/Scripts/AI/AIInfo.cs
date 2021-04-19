using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AI_Info", menuName = "ANT/Create AI Info")]
public class AIInfo : ScriptableObject
{
    public AnimationClip[] IdleAnimations;
    public AnimationClip[] WalkAnimations;
    public CustomizeController CostumizeController;

    private Gender[] Genders = { Gender.MALE, Gender.FEMALE };

    public AI Customize(AI ai)
    {
        CreateBasicInfo(ai);
        CreateFamily(ai);
        //CreateAnimations(ai);
        //CreateTextures(ai);
        CreateSize(ai);

        CostumizeController.CustomizeAI(ai);

        return ai;
    }

    private void CreateBasicInfo(AI ai)
    {
        var (firstname, lastname) = Names.GetRandomName();

        ai.Speed = 1.2f;
        ai.FirstName = firstname;
        ai.SurName = lastname;
        ai.Gender = UnityEngine.Random.Range(0, 100) > 90 ? Genders[1] : Genders[0];
        ai.Age = UnityEngine.Random.Range(4, 20);
    }

    private void CreateFamily(AI ai)
    {
        int childs = 0, parents = 0, partner = 0;
        string family = "";

        if (ai.Age > 10 && ai.Age < 15)
        {
            childs = UnityEngine.Random.Range(0, 100) > 50 ? UnityEngine.Random.Range(0, 5) : 0;
        }

        if (ai.Age > 8 && ai.Age < 15 || childs > 0)
        {
            partner = UnityEngine.Random.Range(0, 100) > 50 ? 1 : 0;
        }

        if (ai.Age < 20)
        {
            parents = UnityEngine.Random.Range(0, 100) > 80 ? UnityEngine.Random.Range(0, 100) > 90 ? 2 : 1 : 0;
        }

        if (partner > 0)
        {
            var married = UnityEngine.Random.Range(0, 100) > 85;
            var partnerStr = Gender.MALE == ai.Gender
                ? Names.female_partner[married ? 0 : 1]
                : Names.male_partner[married ? 0 : 1];

            family += $"{partnerStr}, ";
        }

        if (childs > 0)
        {
            if (childs == 1)
            {
                family += $"1 Child, ";
            }
            else
            {
                family += $"{childs} Children, ";
            }
        }

        if (parents > 0)
        {
            family += $"{parents} Parent{(parents > 1 ? "s" : "")}, ";
        }

        ai.Population = childs + parents + partner + 1;
        ai.Family = family.Length > 0 ? family.Substring(0, family.LastIndexOf(',')) : "( No Family )";
    }

    private void CreateSize(AI ai)
    {
        ai.transform.localScale = Vector3.one * UnityEngine.Random.Range(0.85f, 1f);
    }
}
