using UnityEngine;
using System.Collections;
using Newtonsoft.Json;

public class SkillAnimData1
{
    [JsonProperty("고유번호")]
    public string UniqueID;
    public string castAnim;
    public string castPrefab;
    public string hitAnim;
    public string hitPrefab;


    public GameObject GetCastPrefab()
    {
        if (castPrefab == null)
            return null;
        return Resources.Load<GameObject>("#09.SkillPrefabs/" + castPrefab.Split(", ")[0]);
    }
    public GameObject GetHitPrefab()
    {
        if (hitPrefab == null)
            return null;
        return Resources.Load<GameObject>("#09.SkillPrefabs/" + hitPrefab.Split(", ")[0]);
    }

    public RuntimeAnimatorController GetCastAnimator()
    {
        return Resources.Load<RuntimeAnimatorController>("#10.Skill_Animators/" + castAnim);
    }
    public RuntimeAnimatorController GetHitAnimator()
    {
        return Resources.Load<RuntimeAnimatorController>("#10.Skill_Animators/" + castAnim);
    }
    public Avatar GetCastAvatar()
    {
        return Resources.Load<Avatar>("#10.Skill_Animators/" + castAnim);
    }
    public RuntimeAnimatorController GetHitAvatar()
    {
        return Resources.Load<RuntimeAnimatorController>("#10.Skill_Animators/" + castAnim);
    }
}
