using UnityEngine.SceneManagement;

namespace Com.LGUplus.Homework.Minifps.Utills
{
    public class CommonUtils
    {
        public static void LoadScene(string sceneName){
            SceneManager.LoadScene(sceneName);
        }
    }
}