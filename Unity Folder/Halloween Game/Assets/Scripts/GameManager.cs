using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Halloween
{
    public class GameManager : MonoBehaviour
    {

        [SerializeField] public TextMeshProUGUI _AmmoText;
        [SerializeField] public TextMeshProUGUI _HealthText;
        [SerializeField] public TextMeshProUGUI _LevelText;
        [SerializeField] public TextMeshProUGUI _EnemiesText;
        [SerializeField] private GameObject _PanelMenu;
        [SerializeField] private GameObject _PanelControls;
        [SerializeField] private GameObject _PanelPlayMode;
        [SerializeField] private GameObject _PanelGameOver;
        [SerializeField] private GameObject _PanelPauseMode;
        [SerializeField] private GameObject _SkeletonPrefab;
        [SerializeField] private GameObject _ZombiePrefab;
        [SerializeField] private GameObject _SkeletonWarriorPrefab;
        [SerializeField] private GameObject _AmmunitionSpawn;
        [SerializeField] private GameObject _Player;
        [SerializeField] private LayerMask _GroundMask;
        private GameObject[] _DestroyEnemies;
        private GameObject[] _DestroyAmmo;
        private GameObject _PlayerObject;
        private Vector3 _SpawnArea;
        private Vector3 _SpawnAreaAmmo;
        private State _State;
        private int _Ammunition, _AmmoDrops, _ClipAmmunition, _Lives, _Level, _ZombieCount, _SkeletonWarriorCount, _Enemies;
        private bool _Spawning = false, _FirstLevel = true, _AmmoSpawning = false;

        public enum State { MENU, CONTROLS, INIT, PLAY, PAUSE, GAMEOVER }

        public static GameManager Instance { get; private set; }

        public int ClipAmmunition
        {
            get { return _ClipAmmunition; }
            set { _ClipAmmunition = value; }
        }

        public int Ammunition
        {
            get { return _Ammunition; }
            set { _Ammunition = value; _AmmoText.text = "Ammo: " + _Ammunition + "/" + _ClipAmmunition; }
        }

        public int Lives
        {
            get { return _Lives; }
            set { _Lives = value; _HealthText.text = "Lives: " + _Lives; }
        }

        public int Enemies
        {
            get { return _Enemies; }
            set { _Enemies = value; _EnemiesText.text = "Enemies: " + _Enemies; }
        }

        void Start()
        {
            Instance = this;
            SwitchState(State.MENU);
            StartCoroutine(SpawnAmmo());
            _AmmoDrops = 0;
            _Level = 1;
        }

        void Update()
        {
            switch (_State)
            {
                case State.MENU:
                    break;
                case State.CONTROLS:
                    break;
                case State.INIT:
                    break;
                case State.PLAY:
                    _EnemiesText.text = "Enemies: " + _Enemies;
                    _LevelText.text = "Level: " + _Level;
                    _AmmoDrops = GameObject.FindGameObjectsWithTag("Ammo").Length;

                    if (Input.GetKeyDown(KeyCode.P))
                    {
                        Time.timeScale = 0;
                        SwitchState(State.PAUSE);
                    }

                    if (_AmmoSpawning == true && _AmmoDrops <= 4)
                    {
                        _SpawnAreaAmmo = new Vector3(Random.Range(-13f, 13f), 0.5f, Random.Range(-13f, 13f));
                        var _SpavnedObject = Instantiate(_AmmunitionSpawn, _SpawnAreaAmmo, Quaternion.identity);
                        Collider[] detectobject = Physics.OverlapSphere(_SpavnedObject.transform.position, 0.5f);

                        foreach (Collider col in detectobject)
                        {
                            if (col.tag == "Untagged" || col.tag == "Player" || col.tag == "Enemy" || col.tag == "sd")
                            {
                                Destroy(_SpavnedObject);
                            }
                            else
                            {
                                StartCoroutine(SpawnAmmo());
                            }
                        }
                    }

                    if (_Lives <= 0)
                     {
                         SwitchState(State.GAMEOVER);
                     }

                    if (_Enemies == 0 && _Spawning == false && _Level >= 1 )
                    {
                       if (_FirstLevel == false)
                        {
                            _Level++;
                       }

                         SpawnEnemy(_Level);
                        _FirstLevel = false;

                             if ((_Level % 2) == 0)
                            {
                                _ZombieCount++;
                            }

                            SpawnZombieEnemy(_ZombieCount);

                            if (_Level % 3 == 0)
                            {
                            _SkeletonWarriorCount++;
                            }

                            SpawnSkeletonWarriorEnemy(_SkeletonWarriorCount);
                    }

                    break;
                case State.PAUSE:
                    break;
                case State.GAMEOVER:

                    Destroy(_PlayerObject);
                    _PanelPlayMode.SetActive(false);
                    _DestroyEnemies = GameObject.FindGameObjectsWithTag("Enemy");
                    _DestroyAmmo = GameObject.FindGameObjectsWithTag("Ammo");

                    foreach (GameObject enemy in _DestroyEnemies)
                    {
                        GameObject.Destroy(enemy);
                    }

                    foreach (GameObject ammo in _DestroyAmmo)
                    {
                        GameObject.Destroy(ammo);
                    }
                    break;
            }
        }

        void SwitchState(State newState)
        {
            EndState();
            _State = newState;
            BeginState(newState);
        }

        void BeginState(State newState)
        {
            switch (newState)
            {
                case State.MENU:
                    Cursor.lockState = CursorLockMode.None;
                    _PanelMenu.SetActive(true);
                    break;
                case State.CONTROLS:
                    _PanelControls.SetActive(true);
                    break;
                case State.INIT:
                    _PanelPlayMode.SetActive(true);
                    Cursor.lockState = CursorLockMode.Locked;
                    _PlayerObject = Instantiate(_Player, new Vector3(0f, 1f, -4f), Quaternion.identity) ;
                    _Level = 1;
                    _Lives = 10;
                    _Enemies = 0;
                    _Ammunition = 5;
                    _ClipAmmunition = 5;
                    _AmmoDrops = 0;
                    _FirstLevel = true;
                    _ZombieCount = 0;
                    _SkeletonWarriorCount = 0;
                    SwitchState(State.PLAY);
                    break;
                case State.PLAY:
                    Cursor.lockState = CursorLockMode.Locked;
                    break;
                case State.PAUSE:
                    Cursor.lockState = CursorLockMode.None;
                    _PanelPauseMode.SetActive(true);
                    break;
                case State.GAMEOVER:
                    _PanelGameOver.SetActive(true);
                    Cursor.lockState = CursorLockMode.None;
                    break;
            }
        }

        void EndState()
        {
            switch (_State)
            {
                case State.MENU:
                    _PanelGameOver.SetActive(false);
                    _PanelMenu.SetActive(false);
                    _PanelControls.SetActive(false);
                    break;
                case State.CONTROLS:
                    _PanelMenu.SetActive(false);
                    break;
                case State.INIT:
                    _PanelGameOver.SetActive(false);
                    break;
                case State.PLAY:
                    _PanelPauseMode.SetActive(false);
                    break;
                case State.PAUSE:
                    break;
                case State.GAMEOVER:
                    _PanelPlayMode.SetActive(false);
                    break;
            }
        }

        public void PlayClick()
        {
            SwitchState(State.INIT);
        }

        public void MainMenuClick()
        {
            _PanelControls.SetActive(false);
            _PanelGameOver.SetActive(false);
            SwitchState(State.MENU);
        }

        public void ControlsClick()
        {
            SwitchState(State.CONTROLS);
        }

        public void ContinueClick()
        {
            _PanelPauseMode.SetActive(false);
            Time.timeScale = 1;
            SwitchState(State.PLAY);
        }

        IEnumerator SpawnAmmo()
        {
            _AmmoSpawning = false;
            yield return new WaitForSeconds(4);
            _AmmoSpawning = true;
        }

        private void SpawnEnemy(int skeletonsToSpawn)
        {
            _Spawning = true;
            for (int s = 0; s < skeletonsToSpawn; s++)
            {
                _SpawnArea = new Vector3(Random.Range(-27f, 27f), 0.1f, Random.Range(-27f, 27f));

                if (_SpawnArea.z > 15f && _SpawnArea.z < 27f)
                {
                    Instantiate(_SkeletonPrefab, new Vector3(Random.Range(-27f, 27f), 0.1f, Random.Range(15f, 27f)), Quaternion.identity);
                    _Enemies++;
                }
                else if (_SpawnArea.z < -15f && _SpawnArea.z > -27f)
                {
                    Instantiate(_SkeletonPrefab, new Vector3(Random.Range(-27f, 27f), 0.1f, Random.Range(-15f, -27f)), Quaternion.identity);
                    _Enemies++;
                }
                else if (_SpawnArea.x < -15f && _SpawnArea.x > -27f)
                {
                    Instantiate(_SkeletonPrefab, new Vector3(Random.Range(15f, 27f), 0.1f, Random.Range(-27f, 27f)), Quaternion.identity);
                    _Enemies++;
                }
                else if (_SpawnArea.x < -15f && _SpawnArea.x > -27f)
                {
                    Instantiate(_SkeletonPrefab, new Vector3(Random.Range(-15f, -27f), 0.1f, Random.Range(-27f, 27f)), Quaternion.identity);
                    _Enemies++;
                }
                else
                {
                    s--;
                }
            }
            _Spawning = false;
        }

        private void SpawnZombieEnemy(int zombiesToSpawn)
        {
            for (int z = 0; z < zombiesToSpawn; z++)
            {
                if (_SpawnArea.z > 15f && _SpawnArea.z < 27f)
                {
                    Instantiate(_ZombiePrefab, new Vector3(Random.Range(-27f, 27f), 0.1f, Random.Range(15f, 27f)), Quaternion.identity);
                    _Enemies++;
                }
                else if (_SpawnArea.z < -15f && _SpawnArea.z > -27f)
                {
                    Instantiate(_ZombiePrefab, new Vector3(Random.Range(-27f, 27f), 0.1f, Random.Range(-15f, -27f)), Quaternion.identity);
                    _Enemies++;
                }
                else if (_SpawnArea.x < -15f && _SpawnArea.x > -27f)
                {
                    Instantiate(_ZombiePrefab, new Vector3(Random.Range(15f, 27f), 0.1f, Random.Range(-27f, 27f)), Quaternion.identity);
                    _Enemies++;
                }
                else if (_SpawnArea.x < -15f && _SpawnArea.x > -27f)
                {
                    Instantiate(_ZombiePrefab, new Vector3(Random.Range(-15f, -27f), 0.1f, Random.Range(-27f, 27f)), Quaternion.identity);
                    _Enemies++;
                }
                else
                {
                    z--;
                }
            }
        }

        private void SpawnSkeletonWarriorEnemy(int skeletonWarriorsToSpawn)
        {
            for (int sw = 0; sw < skeletonWarriorsToSpawn; sw++)
            {
                if (_SpawnArea.z > 15f && _SpawnArea.z < 27f)
                {
                    Instantiate(_SkeletonWarriorPrefab, new Vector3(Random.Range(-27f, 27f), 0.1f, Random.Range(15f, 27f)), Quaternion.identity);
                    _Enemies++;
                }
                else if (_SpawnArea.z < -15f && _SpawnArea.z > -27f)
                {
                    Instantiate(_SkeletonWarriorPrefab, new Vector3(Random.Range(-27f, 27f), 0.1f, Random.Range(-15f, -27f)), Quaternion.identity);
                    _Enemies++;
                }
                else if (_SpawnArea.x < -15f && _SpawnArea.x > -27f)
                {
                    Instantiate(_SkeletonWarriorPrefab, new Vector3(Random.Range(15f, 27f), 0.1f, Random.Range(-27f, 27f)), Quaternion.identity);
                    _Enemies++;
                }
                else if (_SpawnArea.x < -15f && _SpawnArea.x > -27f)
                {
                    Instantiate(_SkeletonWarriorPrefab, new Vector3(Random.Range(-15f, -27f), 0.1f, Random.Range(-27f, 27f)), Quaternion.identity);
                    _Enemies++;
                }
                else
                {
                    sw--;
                }
            }
        }
    }
}
