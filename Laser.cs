using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _speed = 8.0f;
    private bool _isEnemyLaser = false;
    [SerializeField] private float _enemyLaserSpeed = 8.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.tag == "Laser")
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
            if (transform.position.y >= 8.0f)
            {
                if (transform.parent != null)
                    Destroy(this.transform.parent.gameObject);
                else
                    Destroy(this.gameObject);
            }
        }
        else if (this.tag == "Enemy_Laser")
        {
            transform.Translate(Vector3.down * _enemyLaserSpeed * Time.deltaTime);
            if (transform.position.y <= -8.0f)
            {
                if (transform.parent != null)
                    Destroy(this.transform.parent.gameObject);
                else
                    Destroy(this.gameObject);
            }
        }
    }
}
