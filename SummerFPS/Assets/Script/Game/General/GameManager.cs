

using System.Collections;
using System.IO;
using Com.LGUplus.Homework.Minifps.Utills;
using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using Script.Game;
using UnityEngine;

using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class GameManager : MonoBehaviourPunCallbacks
{
        public static GameManager Instance = null;

        public Text infoText;
        public Text gameStatusText;
        public Text gameTimeText;
        
        private float time_current;
        private bool isEnded;
        
        public float gameTime = 60f;
        public float resutOpenningTime = 3.0f;

        #region UNITY

        public void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (isEnded)
                return;
            Check_Timer();
        }

        public override void OnEnable()
        {
            base.OnEnable();

            CountdownTimer.OnCountdownTimerHasExpired += OnCountdownTimerIsExpired;
        }

        public void Start()
        {
            Hashtable props = new Hashtable
            {
                {AsteroidsGame.PLAYER_LOADED_LEVEL, true}
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
            
            Reset_Timer();
        }

        public override void OnDisable()
        {
            base.OnDisable();

            CountdownTimer.OnCountdownTimerHasExpired -= OnCountdownTimerIsExpired;
        }

        #endregion

        #region COROUTINES

    
        private IEnumerator EndOfGame(string winner, int score)
        {
            

            while (resutOpenningTime > 0.0f)
            {
                infoText.text = string.Format("Player {0} won with {1} points.\n\n\nReturning to login screen in {2} seconds.", winner, score, resutOpenningTime.ToString("n2"));

                yield return new WaitForEndOfFrame();

                resutOpenningTime -= Time.deltaTime;
            }
            gameStatusText.text = SummerFPSGame.FINISH_GAME;
            
            PhotonNetwork.LeaveRoom();
        }

        #endregion

        #region PUN CALLBACKS

        public override void OnDisconnected(DisconnectCause cause)
        {
            CommonUtils.LoadScene("TitleScene");
        }

        public override void OnLeftRoom()
        {
            PhotonNetwork.Disconnect();
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
            {
                //need to enemy
               // StartCoroutine(SpawnAsteroid());
            }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            CheckEndOfGame();
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (changedProps.ContainsKey(AsteroidsGame.PLAYER_LIVES))
            {
                CheckEndOfGame();
                return;
            }

            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }


            // if there was no countdown yet, the master client (this one) waits until everyone loaded the level and sets a timer start
            int startTimestamp;
            bool startTimeIsSet = CountdownTimer.TryGetStartTime(out startTimestamp);

            if (changedProps.ContainsKey(AsteroidsGame.PLAYER_LOADED_LEVEL))
            {
                
                if (CheckAllPlayerLoadedLevel())
                {
                    if (!startTimeIsSet)
                    {
                        gameStatusText.text = SummerFPSGame.PREPARE_GAME;
                        CountdownTimer.SetStartTime();
                    }
                }
                else
                {
                    infoText.text = "Waiting for other players...";
                }
            }
        
        }

        #endregion
        
        private void StartGame()
        {
            gameStatusText.text = SummerFPSGame.START_GAME;
            
            MakePlayerManager();
            MakeEnemyManager();
            
            if (PhotonNetwork.IsMasterClient)
            {
              //  StartCoroutine(SpawnAsteroid());
            }
        }

        private static void MakePlayerManager()
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
        }
        private static void MakeEnemyManager()
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "EnemyManager"), Vector3.zero, Quaternion.identity);
        }

        private bool CheckAllPlayerLoadedLevel()
        {
            gameStatusText.text = SummerFPSGame.CHECK_LOADING;
            
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                object playerLoadedLevel;

                if (p.CustomProperties.TryGetValue(AsteroidsGame.PLAYER_LOADED_LEVEL, out playerLoadedLevel))
                {
                    if ((bool) playerLoadedLevel)
                    {
                        continue;
                    }
                }

                return false;
            }

            return true;
        }

        private void CheckEndOfGame()
        {
            bool allDestroyed = true;

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                object lives;
                if (p.CustomProperties.TryGetValue(AsteroidsGame.PLAYER_LIVES, out lives))
                {
                    if ((int) lives > 0)
                    {
                        allDestroyed = false;
                        break;
                    }
                }
            }

            if (allDestroyed)
            {
                finishGame();
            }
        }

        private void finishGame()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                StopAllCoroutines();
            }

            string winner = "";
            int score = -1;

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                if (p.GetScore() > score)
                {
                    winner = p.NickName;
                    score = p.GetScore();
                }
            }

            StartCoroutine(EndOfGame(winner, score));
        }

        private void OnCountdownTimerIsExpired()
        {
            StartGame();
        }
        
        private void End_Timer()
        {
            time_current = 0;
            gameTimeText.text = $"{time_current:N1}";
            isEnded = true;
            finishGame();
        }


        private void Reset_Timer()
        {
            time_current = gameTime;
            gameTimeText.text = $"{time_current:N1}";
            isEnded = false;
            Debug.Log("Start");
        }
        
        private void Check_Timer()
        {
            if (0 < time_current)
            {
                time_current -= Time.deltaTime;
                gameTimeText.text = $"{time_current:N1}";
                Debug.Log(time_current);
            }
            else if (!isEnded)
            {
                End_Timer();
            }
            
        }
}
