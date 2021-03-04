using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
        [SerializeField] Text scoreText;
        [SerializeField] SettingsPopUp settingsPopUp;

        int score;

        void Start()
        {
                score = 0;
                scoreText.text = score.ToString();

                Messenger.AddListener(GameEvent.ENEMY_HIT, OnEnemyHit);
                settingsPopUp.Close();
        }

        void OnEnemyHit()
        {
                score += 1;
                scoreText.text = score.ToString();
        }

        void OnDestroy()
        {
                Messenger.RemoveListener(GameEvent.ENEMY_HIT, OnEnemyHit);
        }

        public void OnOpenSettings()
        {
                settingsPopUp.Open();
        }

        public void OnCloseSettings()
        {
                settingsPopUp.Close();
        }

        public void OnSubmitName(string name)
        {
                Debug.Log($"Submitted Name: {name}");
        }

        public void OnSpeedChange(float speed)
        {
                Messenger<float>.Broadcast(GameEvent.SPEED_CHANGED, speed);
        }
}
