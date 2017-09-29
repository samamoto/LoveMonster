using UnityEngine;

[System.Flags]
public enum LockState
{
    STATE_UNLOCKED = 1,
    STATE_CLEARED = 1 << 1,
}

[System.Serializable]
public struct SaveData
{
    public string stageName;
    public float cleartime;
    public int maxAcorns;
    public int cntAcorns;
    public LockState lockState;
};

public enum Stages
{
    STAGE_1,
    STAGE_MAX
}

public class SaveDataManager : Singleton<SaveDataManager>
{
    //ファイル場所
    private string savedataPath;

    private SaveData[] InitSavedata;

    //セーブデータ一覧
    private SaveData[] savedata;

    public bool RemakeSaveData = false;

    //一時保存空間
    public SaveData temp { set; get; }

    private void Awake()
    {
        this.savedataPath = Application.persistentDataPath + "/FilmushiSave.xml";
    }

    private void CreateSaveData()
    {
        this.InitSavedata = new SaveData[(int)Stages.STAGE_MAX];

        XmlUtility.Seialize<SaveData[]>(this.savedataPath, this.InitSavedata);
        Debug.Log("Create new savedata:" + this.savedataPath);
    }

    public bool LoadSaveData()
    {
        //ファイル存在チェック
        if (System.IO.File.Exists(savedataPath))
        {
            //読み込み
            if (this.RemakeSaveData)
            {
                CreateSaveData();
            }
            savedata = XmlUtility.Deserialize<SaveData[]>(savedataPath);
            Debug.Log("Load savedata Path:" + this.savedataPath);
        }
        else
        {
            Debug.LogWarning("Not exist savedata");
            this.CreateSaveData();
            this.LoadSaveData();

            return false;
        }
        return true;
    }

    public bool WriteSaveData()
    {
        //iCloudに保存させない
        UnityEngine.iOS.Device.SetNoBackupFlag(savedataPath);
        //書き出し
        XmlUtility.Seialize<SaveData[]>(savedataPath, savedata);
        Debug.Log("Write savedata Path:" + this.savedataPath);

        return true;
    }

    public SaveData GetSaveData(string stageName)
    {
        for (int i = 0; i < savedata.Length; i++)
        {
            if (stageName == savedata[i].stageName)
            {
                return savedata[i];
            }
        }
        SaveData none = new SaveData();
        return none;
    }

    public SaveData GetSaveData(int stageIndex)
    {
        for (int i = 0; i < savedata.Length; i++)
        {
            if (stageIndex == i)
            {
                return savedata[i];
            }
        }
        SaveData none = new SaveData();
        return none;
    }

    public void SetSaveData(SaveData data)
    {
        for (int i = 0; i < savedata.Length; i++)
        {
            if (data.stageName == savedata[i].stageName)
            {
                savedata[i] = data;
            }
        }
    }

    public SaveData[] GetAllSaveData()
    {
        SaveData[] dest = new SaveData[(int)Stages.STAGE_MAX];
        for (int i = 0; i < (int)Stages.STAGE_MAX; i++)
        {
            dest[i] = savedata[i];
        }
        return dest;
    }

    public SaveData GetNextSaveData(string beforeStageName)
    {
        SaveData dest = new SaveData();
        for (int i = 0; i < (int)Stages.STAGE_MAX - 1; i++)
        {
            if (this.savedata[i].stageName == beforeStageName)
            {
                dest = this.savedata[i + 1];
                return dest;
            }
        }
        return dest;
    }

    public SaveData GetNextSaveData(int stageIndex)
    {
        SaveData dest = new SaveData();
        for (int i = 0; i < (int)Stages.STAGE_MAX - 1; i++)
        {
            if (i == stageIndex)
            {
                dest = this.savedata[i + 1];
                return dest;
            }
        }
        return dest;
    }
}