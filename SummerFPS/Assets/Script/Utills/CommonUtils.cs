using System.Text;
using UnityEngine.SceneManagement;

namespace Com.LGUplus.Homework.Minifps.Utills
{
    public class CommonUtils
    {
        private static string NETWORK_FAILURE = "네트워크 문제로 접속 실패 \n";
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
      
    }
}