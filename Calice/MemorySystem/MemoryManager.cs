using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

[System.Serializable]
public class CutScneceAni
{
    public List<AssetReferenceT<RuntimeAnimatorController>> UseCut;
}

public class MemoryManager : MonoBehaviour
{
    static public MemoryManager instance;

    [Header("직접 넣기")]
    //public asset

    [Header("컷씬 메모리")]
    public List<CutScneceAni> UseCut;
    [SerializeField] List<RuntimeAnimatorController> Cutins = new List<RuntimeAnimatorController>();
    [SerializeField] List<AsyncOperationHandle<RuntimeAnimatorController>> Cutins_handle = new List<AsyncOperationHandle<RuntimeAnimatorController>>();
    public int UnLoadIndex;     //해제할 개수
    public bool ComeCut;

    [Header("튜토리얼 메모리")]
    public List<AssetReferenceT<RuntimeAnimatorController>> UseTuto;
    [SerializeField] List<RuntimeAnimatorController> Tuto = new List<RuntimeAnimatorController>();

    [Header("적 메모리")]
    public List<GameObject> UseMonster=new List<GameObject>();
    [SerializeField] bool StartCheck;   //중복선언 체크

    [Header("스테이지 배경 메모리")]
    public List<AssetReferenceT<RuntimeAnimatorController>> UseStageBackGround;
    [SerializeField] RuntimeAnimatorController stagebg;

    /// <summary> 엔드리스에서만 사용 </summary>
    [SerializeField] List<RuntimeAnimatorController> stagebgList_End=new List<RuntimeAnimatorController>();

    [Header("맵 (노드선택) 배경 메모리")]
    public List<AssetReferenceT<RuntimeAnimatorController>> UseMapBackGround;
    [SerializeField] RuntimeAnimatorController mapbg;

    [SerializeField] int StageIndex_m;  //맵
    [SerializeField] int StageIndex_s;  //스테이지
    [SerializeField] int SelectSlot;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        StartCheck = false;
        stagebg = null;
        mapbg = null;
        ComeCut = false;
    }

    public IEnumerator CutLoad()
    {
        if (ComeCut)
        {

            if (nowStage == 1 && ES3.Load<bool>("isIntroStart", "./SaveData/SaveFile" + nowSlot + ".es3", true))       //튜토없이 입장할 경우 0 (0이 입장 컷씬)
            {
                nowStage = 0;
            }

            if (nowStage != 5)
            {
                for (int i = 0; i < UseCut[nowStage].UseCut.Count; i++)
                {
                    var handle = UseCut[nowStage].UseCut[i].LoadAssetAsync<RuntimeAnimatorController>();

                    yield return handle;

                    RuntimeAnimatorController loadedAsset = handle.Result;
                    Cutins.Add(loadedAsset);
                    Cutins_handle.Add(handle);
                }
            }
            ComeCut = false;

            yield return null;
        }
    }

    public void CutUnLoad()
    {
        if (Cutins.Count != 0)
        {
            for (int i = 0; i < UnLoadIndex; i++)
            {
                Cutins.RemoveAt(0);
                Addressables.Release(Cutins_handle[0]);
                Cutins_handle.RemoveAt(0);
            }
            UnLoadIndex = 0;
        }
    }

    public IEnumerator CutAllUnLoad()
    {
        if (Cutins.Count != 0)
        {
            int n = Cutins.Count;
            for (int i = 0; i < n; i++)
            {
                Cutins.RemoveAt(0);
                Addressables.Release(Cutins_handle[0]);
                Cutins_handle.RemoveAt(0);
            }
            UnLoadIndex = 0;
        }
        yield return null;
    }

    public List<RuntimeAnimatorController> CutGet()
    {
        if (Cutins.Count != 0)
        {
            return Cutins;
        }
        else
        {
            return null;
        }
    }

    public IEnumerator MemoryLoad(Image progressBar)
    {
        yield return DataSet();

        if (!EndlessManager.instance.isEndless)
        {
            if (nowStage == 0)
            {
                yield return TutorialLoad();
            }
            else
            {
                yield return TutorialUnLoad();
            }
        }

        progressBar.fillAmount = 0.3f;

        yield return StageLoad();
        progressBar.fillAmount = 0.41f;
        
        if (!EndlessManager.instance.isEndless)
        {
            yield return MapBackGroundLoad();
        }

        progressBar.fillAmount = 0.5f;
        yield return StageBackGroundLoad();
        progressBar.fillAmount = 0.6f;
        
        if (!EndlessManager.instance.isEndless)
        {
            yield return CutLoad();
        }

        progressBar.fillAmount = 0.8f;
    }

    public void UnLoad()
    {
        if (UseMonster.Count != 0)
        {
            for (int i = 0; i < UseMonster.Count; i++)
            {
                Debug.LogError(UseMonster[i].name+"메모리 해제");
                Addressables.Release(UseMonster[i]);
            }
            UseMonster.Clear();
        }

        Resources.UnloadUnusedAssets();
    }
    
    int nowSlot;
    int NextStage;
    int nowStage;
    public void EventLoad()
    { 

    }

    public void ShopLoad()
    {

    }

    public void BlackShopLoad()
    {

    }

    public IEnumerator TutorialUnLoad()
    {
        if (Tuto.Count != 0)
        {
            for (int i = 0; i < UseTuto.Count; i++)
            {
                UseTuto[i].ReleaseAsset();
            }
        }
        Tuto.Clear();

        yield return null;
    }

    public IEnumerator TutorialLoad()
    {
        if (Tuto.Count == 0)
        {
            for (int i = 0; i < UseTuto.Count; i++)
            {
                UseTuto[i].LoadAssetAsync<RuntimeAnimatorController>().Completed += ListTutoUp;
            }

            DebugX.Log("튜토리얼 메모리 로드");
        }
        yield return null;
    }

    public List<RuntimeAnimatorController> TutoGet()
    {
        if (Tuto.Count!=0)
        {
            return Tuto;
        }
        else
        {
            return null;
        }
    }

    public IEnumerator DataSet()
    {
        nowSlot = ES3.Load<int>("nowSlot", "./SaveData/NowSlot.es3", 1);

        if (!EndlessManager.instance.isEndless)
        {
            NextStage = ES3.Load<int>("NextStage", "./SaveData/SaveFile" + nowSlot + ".es3", 0);
            nowStage = ES3.Load<int>("NowStage", "./SaveData/SaveFile" + nowSlot + ".es3", 0);
        }
        else
        {
            NextStage = EndlessManager.instance.GetStageNum();
            nowStage = EndlessManager.instance.GetStageNum();
        }

        yield return null;
    }

    public IEnumerator MapBackGroundLoad()
    {
        //올려둔 메모리 해제
        if (mapbg)
        {
            DebugX.LogError(StageIndex_m + "스테이지 맵 배경 메모리 해제");
            mapbg = null;
            UseMapBackGround[StageIndex_m].ReleaseAsset();            
        }

        if (nowStage != 5)
        {
            switch (NextStage)
            {
                case 0:
                case 1:
                    mapbg = UseMapBackGround[0].LoadAssetAsync<RuntimeAnimatorController>().WaitForCompletion();
                    StageIndex_m = 0;
                    break;
                case 2:
                    mapbg = UseMapBackGround[1].LoadAssetAsync<RuntimeAnimatorController>().WaitForCompletion();
                    StageIndex_m = 1;
                    break;
                case 3:
                    mapbg = UseMapBackGround[2].LoadAssetAsync<RuntimeAnimatorController>().WaitForCompletion();
                    StageIndex_m = 2;
                    break;
                case 4:
                    mapbg = UseMapBackGround[3].LoadAssetAsync<RuntimeAnimatorController>().WaitForCompletion();
                    StageIndex_m = 3;
                    break;
                case 5:
                    mapbg = UseMapBackGround[4].LoadAssetAsync<RuntimeAnimatorController>().WaitForCompletion();
                    StageIndex_m = 4;
                    break;
                default:
                    break;
            }
        }

        yield return null;
    }

    public RuntimeAnimatorController MapBackGroundGet()
    {
        if (mapbg)
        {
            return mapbg;
        }
        else
        {
            return null;
        }
    }

    public IEnumerator StageBackGroundLoad()
    {
        //올려둔 메모리 해제
        if (stagebg)
        {
            DebugX.LogError(StageIndex_s + "스테이지 전투 배경 메모리 해제");
            stagebg = null;
            UseStageBackGround[StageIndex_s].ReleaseAsset();
        }
        else
        {
            DebugX.LogError(StageIndex_s + "엔드리스 메모리 해제");
            for (int i = 0; i < UseStageBackGround.Count; i++)
            {
                UseStageBackGround[i].ReleaseAsset();
            }
        }

        if (EndlessManager.instance.isEndless)
        {
            for (int i = 0; i < UseStageBackGround.Count; i++)
            {
                stagebg=UseStageBackGround[i].LoadAssetAsync<RuntimeAnimatorController>().WaitForCompletion();
                stagebgList_End.Add(stagebg);
            }
            stagebg = null;
        }
        else
        {
            if (nowStage != 5)
            {
                switch (NextStage)
                {
                    case 0:
                    case 1:
                        stagebg = UseStageBackGround[0].LoadAssetAsync<RuntimeAnimatorController>().WaitForCompletion();
                        StageIndex_s = 0;
                        break;
                    case 2:
                        stagebg = UseStageBackGround[1].LoadAssetAsync<RuntimeAnimatorController>().WaitForCompletion();
                        StageIndex_s = 1;
                        break;
                    case 3:
                        stagebg = UseStageBackGround[2].LoadAssetAsync<RuntimeAnimatorController>().WaitForCompletion();
                        StageIndex_s = 2;
                        break;
                    case 4:
                        stagebg = UseStageBackGround[3].LoadAssetAsync<RuntimeAnimatorController>().WaitForCompletion();
                        StageIndex_s = 3;
                        break;
                    case 5:
                        stagebg = UseStageBackGround[4].LoadAssetAsync<RuntimeAnimatorController>().WaitForCompletion();
                        StageIndex_s = 4;
                        break;
                    default:
                        break;
                }
            }
        }

        yield return null;
    }

    public RuntimeAnimatorController StageBackGroundGet()
    {
        if (EndlessManager.instance.isEndless)
        {            
            return stagebgList_End[EndlessManager.instance.GetStageNum()-1];
        }
        else
        {
            if (stagebg)
            {
                return stagebg;
            }
            else
            {
                return null;
            }
        }
    }

    public IEnumerator StageLoad()
    {
        //올려둔 메모리 해제
        UnLoad();

        //이번에 쓸 몬스터 메모리 올림
        if (!StartCheck)
        {
            //공용 몬스터
            Addressables.LoadAssetAsync<GameObject>("Crow");
            Addressables.LoadAssetAsync<GameObject>("Rat_gold");
        }

        if (EndlessManager.instance.isEndless)
        {
            Addressables.LoadAssetAsync<GameObject>("Dodo_bird").Completed += ListUp;
            Addressables.LoadAssetAsync<GameObject>("Rat").Completed += ListUp;
            Addressables.LoadAssetAsync<GameObject>("Duck_baby").Completed += ListUp;
            Addressables.LoadAssetAsync<GameObject>("Parrot").Completed += ListUp;
            Addressables.LoadAssetAsync<GameObject>("Duck_adult").Completed += ListUp;
            Addressables.LoadAssetAsync<GameObject>("Clock_Rabbit").Completed += ListUp;
            Addressables.LoadAssetAsync<GameObject>("Puppy").Completed += ListUp;
            Addressables.LoadAssetAsync<GameObject>("Lizard").Completed += ListUp;
            Addressables.LoadAssetAsync<GameObject>("Guinea").Completed += ListUp;
            Addressables.LoadAssetAsync<GameObject>("Hedgehog").Completed += ListUp;
            Addressables.LoadAssetAsync<GameObject>("Clock").Completed += ListUp;
            Addressables.LoadAssetAsync<GameObject>("RatWizard").Completed += ListUp;
            Addressables.LoadAssetAsync<GameObject>("Teapot").Completed += ListUp;
            Addressables.LoadAssetAsync<GameObject>("Chef").Completed += ListUp;
            Addressables.LoadAssetAsync<GameObject>("Baby").Completed += ListUp;
            Addressables.LoadAssetAsync<GameObject>("Duchess").Completed += ListUp;
            Addressables.LoadAssetAsync<GameObject>("FrogButler").Completed += ListUp;
            Addressables.LoadAssetAsync<GameObject>("JokerA").Completed += ListUp;
            Addressables.LoadAssetAsync<GameObject>("King").Completed += ListUp;
            Addressables.LoadAssetAsync<GameObject>("SpadeSoldier").Completed += ListUp;
            Addressables.LoadAssetAsync<GameObject>("SpadeKnight").Completed += ListUp;
            Addressables.LoadAssetAsync<GameObject>("DiamondSoldier").Completed += ListUp;
            Addressables.LoadAssetAsync<GameObject>("DiamondTrader").Completed += ListUp;
            Addressables.LoadAssetAsync<GameObject>("CloverConductor").Completed += ListUp;
            Addressables.LoadAssetAsync<GameObject>("CloverSoldier").Completed += ListUp;
            Addressables.LoadAssetAsync<GameObject>("HeartSister").Completed += ListUp;
            Addressables.LoadAssetAsync<GameObject>("HeartSoldier").Completed += ListUp;
            Addressables.LoadAssetAsync<GameObject>("JokerB").Completed += ListUp;
            Addressables.LoadAssetAsync<GameObject>("Queen").Completed += ListUp;
        }
        else
        {
            switch (NextStage)
            {
                case 0:
                case 1:
                    Addressables.LoadAssetAsync<GameObject>("Dodo_bird").Completed += ListUp;
                    Addressables.LoadAssetAsync<GameObject>("Rat").Completed += ListUp;
                    Addressables.LoadAssetAsync<GameObject>("Duck_baby").Completed += ListUp;
                    Addressables.LoadAssetAsync<GameObject>("Parrot").Completed += ListUp;
                    Addressables.LoadAssetAsync<GameObject>("Duck_adult").Completed += ListUp;
                    break;
                case 2:
                    Addressables.LoadAssetAsync<GameObject>("Clock_Rabbit").Completed += ListUp;
                    Addressables.LoadAssetAsync<GameObject>("Puppy").Completed += ListUp;
                    Addressables.LoadAssetAsync<GameObject>("Lizard").Completed += ListUp;
                    Addressables.LoadAssetAsync<GameObject>("Guinea").Completed += ListUp;
                    Addressables.LoadAssetAsync<GameObject>("Hedgehog").Completed += ListUp;
                    Addressables.LoadAssetAsync<GameObject>("Clock").Completed += ListUp;
                    break;
                case 3:
                    Addressables.LoadAssetAsync<GameObject>("RatWizard").Completed += ListUp;
                    Addressables.LoadAssetAsync<GameObject>("Teapot").Completed += ListUp;
                    Addressables.LoadAssetAsync<GameObject>("Chef").Completed += ListUp;
                    Addressables.LoadAssetAsync<GameObject>("Baby").Completed += ListUp;
                    Addressables.LoadAssetAsync<GameObject>("Duchess").Completed += ListUp;
                    Addressables.LoadAssetAsync<GameObject>("FrogButler").Completed += ListUp;
                    break;
                case 4:
                case 5:
                    Addressables.LoadAssetAsync<GameObject>("JokerA").Completed += ListUp;
                    Addressables.LoadAssetAsync<GameObject>("King").Completed += ListUp;
                    Addressables.LoadAssetAsync<GameObject>("SpadeSoldier").Completed += ListUp;
                    Addressables.LoadAssetAsync<GameObject>("SpadeKnight").Completed += ListUp;
                    Addressables.LoadAssetAsync<GameObject>("DiamondSoldier").Completed += ListUp;
                    Addressables.LoadAssetAsync<GameObject>("DiamondTrader").Completed += ListUp;
                    Addressables.LoadAssetAsync<GameObject>("CloverConductor").Completed += ListUp;
                    Addressables.LoadAssetAsync<GameObject>("CloverSoldier").Completed += ListUp;
                    Addressables.LoadAssetAsync<GameObject>("HeartSister").Completed += ListUp;
                    Addressables.LoadAssetAsync<GameObject>("HeartSoldier").Completed += ListUp;
                    Addressables.LoadAssetAsync<GameObject>("JokerB").Completed += ListUp;
                    Addressables.LoadAssetAsync<GameObject>("Queen").Completed += ListUp;
                    break;
                default:
                    break;
            }
        }

        StartCheck = true;
        yield return null;
        /*Addressables.LoadResourceLocationsAsync("stage").Completed += ListLoad;
        switch (GameManager.instance.nowStage)
        {
            case 0:
            case 1:
                Addressables.LoadResourceLocationsAsync("stage1").Completed+= ListLoad;
                break;
            case 2:
                Addressables.LoadResourceLocationsAsync("stage2").Completed += ListLoad;
                break;
            case 3:
                Addressables.LoadResourceLocationsAsync("stage3").Completed += ListLoad;
                break;
            case 4:
                Addressables.LoadResourceLocationsAsync("stage4").Completed += ListLoad;
                break;
            case 5:
                Addressables.LoadResourceLocationsAsync("stage5").Completed += ListLoad;
                break;
        }*/
    }

    public void ListTutoUp(AsyncOperationHandle<RuntimeAnimatorController> operation)
    {
        RuntimeAnimatorController loadedAsset = operation.Result;
        Tuto.Add(loadedAsset);
    }

    public void ListUp(AsyncOperationHandle<GameObject> operation)
    {
        GameObject loadedAsset = operation.Result;
        UseMonster.Add(loadedAsset);
    }

    public void ListBackGroundUp(AsyncOperationHandle<RuntimeAnimatorController> operation)
    {
        RuntimeAnimatorController loadAsset = operation.Result;
        stagebgList_End.Add(loadAsset);
    }

    public void EndlessBackgroundMemoryCheck()
    {
        if (EndlessManager.instance.isEndless)
        {
            if (EndlessManager.instance.stageCount == 0)
            {
                if (stagebg)
                {
                    stagebg = null;
                    UseStageBackGround[StageIndex_s].ReleaseAsset();
                }

                switch (EndlessManager.instance.cycleCount)
                {
                    case 0:
                        stagebg = UseStageBackGround[0].LoadAssetAsync<RuntimeAnimatorController>().WaitForCompletion();
                        StageIndex_s = 0;
                        break;
                    case 1:
                        stagebg = UseStageBackGround[1].LoadAssetAsync<RuntimeAnimatorController>().WaitForCompletion();
                        StageIndex_s = 1;
                        break;
                    case 2:
                        stagebg = UseStageBackGround[2].LoadAssetAsync<RuntimeAnimatorController>().WaitForCompletion();
                        StageIndex_s = 2;
                        break;
                    case 3:
                        stagebg = UseStageBackGround[3].LoadAssetAsync<RuntimeAnimatorController>().WaitForCompletion();
                        StageIndex_s = 3;
                        break;
                    case 4:
                        stagebg = UseStageBackGround[4].LoadAssetAsync<RuntimeAnimatorController>().WaitForCompletion();
                        StageIndex_s = 4;
                        break;
                    default:
                        int randBg = UnityEngine.Random.Range(0, 5);
                        stagebg = UseStageBackGround[randBg].LoadAssetAsync<RuntimeAnimatorController>().WaitForCompletion();
                        StageIndex_s = randBg;
                        break;
                }
            }
        }
    }
}
