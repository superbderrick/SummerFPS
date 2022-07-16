using System;
using System.Text;
using UnityEngine.SceneManagement;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Com.LGUplus.Homework.Minifps.Utills
{
    public class CommonUtils
    {
        private static string NETWORK_FAILURE = "네트워크 문제로 접속 실패 \n";
        private static string PLAYER = "Player ";
        
        
        public static void LoadScene(string sceneName){
            SceneManager.LoadScene(sceneName);
        }

        public static string GetErrorMessage(string message)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(NETWORK_FAILURE);
            sb.Append(message);
            
            return sb.ToString();
        }
        
        public static string GetStringMessage(string tittle, string message)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(tittle);
            sb.Append(message);
            
            return sb.ToString();
        }
        
        public static string GetStringMessage(string number, string fullNumber, string bar,string title)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(title);
            sb.Append(number);
            sb.Append(bar);
            sb.Append(fullNumber);
            
            return sb.ToString();
        }
        
        public static string GetPlayerName()
        {
            string number = Random.Range(0, 10000).ToString("0000");
            StringBuilder sb = new StringBuilder();
            
            sb.Append(PLAYER);
            sb.Append(number);
            return sb.ToString();
        }
        
    }
}