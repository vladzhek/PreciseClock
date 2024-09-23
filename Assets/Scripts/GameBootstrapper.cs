using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infastructure
{
    public class GameBootstrapper : MonoBehaviour
    {
        private const string LOAD_SCENE = "Game";

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        void Start()
        {
            InitializeServices();

            SceneManager.LoadScene(LOAD_SCENE);
        }

        private void InitializeServices()
        {

        }
    }
}
