using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class SaveFileDataWriter
{

    public string mSaveDataDirectoryPath = "";
    public string mSaveFileName = "";

    public bool CheckToSeeIfFileExists() 
    {
        if (File.Exists(Path.Combine(mSaveDataDirectoryPath, mSaveFileName))) 
        {
            return true;
        }

        return false;
    }

    public void DeleteSaveFile() 
    {
        File.Delete(Path.Combine(mSaveDataDirectoryPath, mSaveFileName));
    }

    public void CreateSaveFile(CharacterSaveData charactersavedata) 
    {
        string savepath = Path.Combine(mSaveDataDirectoryPath,mSaveFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(savepath));

            string datatostore = JsonUtility.ToJson(charactersavedata, true);

            using (FileStream stream = new FileStream(savepath, FileMode.Create))
            {
                using (StreamWriter filewrtier = new StreamWriter(stream))
                {
                    filewrtier.Write(datatostore);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR CAN'T SAVE DATA " + "/n" + savepath + "/n" + ex);
        }
    }

    public CharacterSaveData LoadSaveFile() 
    {
        CharacterSaveData ReadCharacterSaveData = null;
        string loadpath = Path.Combine(mSaveDataDirectoryPath, mSaveFileName);

        if (File.Exists(loadpath)) 
        {
            try
            {
                string datatoload = "";

                using (FileStream fileStream = new FileStream(loadpath, FileMode.Open))
                {
                    using (StreamReader streamreader = new StreamReader(fileStream))
                    {
                        datatoload = streamreader.ReadToEnd();
                    }
                }

                ReadCharacterSaveData = JsonUtility.FromJson<CharacterSaveData>(datatoload);
            }
            catch (Exception ex)
            {

            }
        }

        return ReadCharacterSaveData;
    }

}
