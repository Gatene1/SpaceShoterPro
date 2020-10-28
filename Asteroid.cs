using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{ 
    [SerializeField] private float _speedRotate = 3.0f;
    [SerializeField] private GameObject _explosionVFX;
    [SerializeField] private AudioSource _explosionSound;
    private SpawnManager _spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 1) * _speedRotate * Time.deltaTime);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            _explosionSound.Play();
            Instantiate(_explosionVFX, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            _spawnManager.StartSpawning(); 
            Destroy(this.gameObject, 0.5f);
        }
    }
}
