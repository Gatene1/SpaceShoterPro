using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4.0f;
    private Animator _anim;
    private AudioSource _explosionSound;
    private AudioSource _laserSound;
    private AudioSource[] audioList;
    

    [SerializeField] private GameObject _laserPrefab;

    private Player _player;

    private bool _destroyed = false;   
    private float _fireRate = 3.0f;
    private float _canFire = -1;
    //handle to animator component

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null)
            Debug.LogError("_Player is NULL");

        _anim = gameObject.GetComponent<Animator>();
        
        if (_anim == null)
            Debug.LogError("The Animator is NULL");

        audioList = GetComponents<AudioSource>();
        _explosionSound = audioList[0];
        _laserSound = audioList[1];

        //_explosionSound = audioList[0];
        //_laserSound = audioList[1];

    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Time.time > _canFire && !_destroyed)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            _laserSound.Play();
            Instantiate(_laserPrefab, transform.position, Quaternion.identity);            
        }

    }

    private void CalculateMovement()
    {
        float randomX = Random.Range(-9.22f, 9.22f);

        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -5.6f)
            transform.position = new Vector3(randomX, 5.6f, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            if (_destroyed == false) // The purpose of this is to not keep getting hit by an enemy's animation                  
            {                        // that is already destroyed.
                _destroyed = true;

                if (_player != null)
                    _player.Damage();

                _explosionSound.Play();
                _anim.SetTrigger("OnEnemyDeath");
                _speed = 0;
                Destroy(this.gameObject, 2.8f);
            }
        }
        else if (other.tag == "Laser")
        {
            if (_destroyed == false)
            {
                _destroyed = true; // This is to also let the game know that an enemy destroyed by a laser
                                   // can't hurt the player by its animation sequence after death.
                _player.UpdateScore(10);
               
                _explosionSound.Play();
                _anim.SetTrigger("OnEnemyDeath");
                _speed = 0;
                Destroy(other.gameObject);
                Destroy(this.gameObject, 2.8f);
            }
        }
    }

    
}
