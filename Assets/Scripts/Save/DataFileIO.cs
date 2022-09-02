using System.IO;
using UnityEngine;

public class DataFileIO {
    private readonly string FILE_PATH = Application.dataPath +"/instantiateData.json";

    public void Save(InstantiateData data){
        var saveString = JsonUtility.ToJson(data);
        File.WriteAllText(  FILE_PATH, saveString);
    }

    public InstantiateData TryLoad(){
        if (File.Exists(FILE_PATH))
            return JsonUtility.FromJson<InstantiateData>(File.ReadAllText(FILE_PATH));

        return null;
    }
}
