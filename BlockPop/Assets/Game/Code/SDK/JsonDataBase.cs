using System;
namespace JollyArt
{
    
    [Serializable]
    public class JsonDataBase
    {
        public string code;
        public string msg;
        public bool Success => code == "0"; 
    }
}