using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _shieldVisualizer;
    [SerializeField] private GameObject _rightEngine;
    [SerializeField] private GameObject _leftEngine;
    [SerializeField] private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField] private int _lives = 3;
    [SerializeField] private int _score = 0;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;
    private bool _playerDeath = false;
    private bool _enemyLaser1Damage = false;  //Check below in Damage() method for explanation
    private float _speedBoostMultiplier = 2.0f;
    private Animator _anim;
    [SerializeField] private AudioSource _laserSound;
    [SerializeField] private AudioSource _explosionSound;
    [SerializeField] private AudioSource _powerupShield;
    [SerializeField] private AudioSource _powerupSpeed;
    [SerializeField] private AudioSource _powerupTripleShot;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0f, 0f, 0f);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_spawnManager == null)
            Debug.LogError("The Spawn Manager is NULL");

        if (_uiManager == null)
            Debug.LogError("The UIManager is NULL");

        _anim = gameObject.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!_playerDeath)
        {
            CalculateMovement();

            if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
            {
                FireLaser();
            }
        }
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                _anim.SetTrigger("Turn_Left");
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                _anim.SetTrigger("Turn_Right");
            else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
                _anim.SetTrigger("Normal_State");
            else if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
                _anim.SetTrigger("Normal_State");
        
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");        
         
        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x > 11.3f)
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        else if (transform.position.x < -11.3f)
            transform.position = new Vector3(11.3f, transform.position.y, 0);
    }

    void FireLaser()
    {
        

        _canFire = Time.time + _fireRate;

        if (_isTripleShotActive)
        {
            _laserSound.Play();
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            _laserSound.Play();
            Instantiate(_laserPrefab, new Vector3(transform.position.x, transform.position.y + 1.05f, 0), Quaternion.identity);
        }
    }

    public void Damage()
    {
        if (_playerDeath)
            return;

        if (_isShieldActive)
        {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }
        
            _lives--;                                      

        if (_lives == 2)
            _rightEngine.gameObject.SetActive(true);
        else if (_lives == 1)
            _leftEngine.gameObject.SetActive(true);

        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            _playerDeath = true;
            _explosionSound.Play();
            _anim.SetTrigger("Explosion_Death");
            foreach (Transform child in transform)
                Destroy(child.gameObject);
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject, 2.3f);

        }
    }

    public void TripleShotActivate()
    {
        if (!_playerDeath)
        {
            _powerupTripleShot.Play();
            _isTripleShotActive = true;
            StartCoroutine(TripleShotPowerDownRoutine());
        }
    }

    public void SpeedBoostActivate()
    {
        if (!_isSpeedBoostActive && !_playerDeath)
        {
            _powerupSpeed.Play();
            _isSpeedBoostActive = true;
            _speed *= _speedBoostMultiplier;
            StartCoroutine(SpeedBoostPowerDownRoutine());
        }        
    }

    public void ShieldActivate()
    {
        if (!_playerDeath)
        {
            _powerupShield.Play();
            _isShieldActive = true;
            _shieldVisualizer.SetActive(true);
        }
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;        
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed /= _speedBoostMultiplier;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy_Laser" && !_playerDeath)
        {
            Destroy(other.gameObject);
            if (!_enemyLaser1Damage) //Basically, if one enemy laser has already damaged, the other will not.
            {
                _enemyLaser1Damage = true;
                Damage();
            }
            else
                _enemyLaser1Damage = false;
        }                            
    }

    public void UpdateScore(int addend)
    {       
        _score += addend;

        _uiManager.UpdateScore(_score);
    }    

}
