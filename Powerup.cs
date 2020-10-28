using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private float _speed = 3.0f;
    [SerializeField] private int powerupID; //0 = TripleShot  1 = Speed Boost  2 = Shields

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y <= -5.77f)
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
                switch (powerupID)
                {
                    case 0:
                        player.TripleShotActivate();
                        break;

                    case 1:
                        player.SpeedBoostActivate();
                        break;

                    case 2:
                        player.ShieldActivate();
                        break;
                }

            Destroy(this.gameObject);
        }
    }
}
