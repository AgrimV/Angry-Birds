using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bird : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _liveTimeAfterCollision;

    [SerializeField] private Button _poweUpButton;

    [SerializeField] AudioClip _despawnSound;
    //[SerializeField] AudioSource _source;

    [SerializeField] List<Sprite> _sprites = new();

    [SerializeField] GameObject _kunai;
    [SerializeField] GameObject _antler;

    [SerializeField] float _kunaiSpeed = 10.0f;
    [SerializeField] float _spreadAngle = 15.0f;
    [SerializeField] float _antlerSpeed = 10.0f;

    private bool _isFlying = false;
    private int _type;

    public bool IsFlying { get { return _isFlying; } }

    private void Awake()
    {
        //_source = GetComponent<AudioSource>();

        int randomIndex = Random.Range(0, _sprites.Count);
        GetComponent<SpriteRenderer>().sprite = _sprites[randomIndex];
        _type = randomIndex;
    }

    private void FixedUpdate()
    {
        if (_isFlying)
        {
            transform.right = _rb.velocity;
        }
    }

    public void Launch(Vector2 direction, float force)
    {
        _isFlying = true;
        _rb.isKinematic = false;
        _rb.AddForce(direction * force, ForceMode2D.Impulse);
    }

    public void Despawn()
    {
        SoundManager.instance.PlayAtPoint(_despawnSound, gameObject.transform.position);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isFlying)
        {
            _isFlying = false;
            StartCoroutine(DestroyBirdAfterCollision());
        }
    }

    IEnumerator DestroyBirdAfterCollision()
    {
        yield return new WaitForSeconds(_liveTimeAfterCollision);

        if (GameManager.instance.AvailableBirds > 0)
        {
            GameManager.instance.BringNextBird = true;
        }
    }

    public void Signature()
    {
        switch (_type)
        {
            case 0:
                NokoSign();
                break;
            case 1:
                AnkoSign();
                break;
            case 2:
                MemeSign();
                break;
            default: break;
        }
    }

    private void NokoSign()
    {
        GameObject antler = Instantiate(_antler, transform.position, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
        antler.GetComponent<Rigidbody2D>().AddForce((-antler.transform.up.normalized + antler.transform.right.normalized) * _antlerSpeed, ForceMode2D.Impulse);
    }

    private void AnkoSign()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject kunai = Instantiate(_kunai, transform.position, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + (180 - _spreadAngle) + (i * _spreadAngle)));
            kunai.GetComponent<Rigidbody2D>().AddForce(-kunai.transform.right.normalized * _kunaiSpeed, ForceMode2D.Impulse);
        }
    }

    private void MemeSign()
    {

    }
}
