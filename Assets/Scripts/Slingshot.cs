using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Slingshot : MonoBehaviour
{
    [Header("LineRenderers")]
    [SerializeField] LineRenderer _bgSlingshot;
    [SerializeField] LineRenderer _fgSlingshot;

    [Header("Positions")]
    [SerializeField] Transform _bgSlingshotStart;
    [SerializeField] Transform _fgSlingshotStart;
    [SerializeField] Transform _centreSlingShot;
    [SerializeField] Transform _idleSlingshot;

    [Header("Bird")]
    [SerializeField] GameObject _birdPrefab;
    [SerializeField] Transform _spawnPosition;

    [Header("Variables")]
    [SerializeField] float _maxDistance = 2.0f;
    [SerializeField] float _spawnSpacing = 1.0f;
    [SerializeField] SlingshotArea _slingshotArea;
    [SerializeField] float _birdOffset = 0.2f;
    [SerializeField] float _slingForce = 10.0f;
    [SerializeField] float _slerpSpeed = 0.2f;

    Vector3 _clampedPosition;
    Queue<GameObject> _spawnedQueue;
    GameObject _activeBird;
    bool _wasClickedWithinCollider;
    Vector2 _direction;
    int _spawnCount;
    bool _slingShotPrimed = false;

    private void Awake()
    {
        _fgSlingshot.enabled = false;
        _bgSlingshot.enabled = false;

        _spawnCount = GameManager.instance.BirdCount;

        _spawnedQueue = new Queue<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < _spawnCount; i++)
        {
            _spawnedQueue.Enqueue(Instantiate(_birdPrefab, new Vector3(_spawnPosition.position.x - ((i - 1) * _spawnSpacing), _spawnPosition.position.y, 0f), Quaternion.identity));
        }

        NextBird();
    }

    void NextBird()
    {
        if (_activeBird != null)
        {
            Destroy(_activeBird);
        }

        _activeBird = _spawnedQueue.Dequeue();

        _activeBird.GetComponent<Renderer>().sortingOrder += 1;
        _activeBird.GetComponent<Rigidbody2D>().isKinematic = true;

        StartCoroutine(MoveBirdToSlingShot());
    }

    IEnumerator MoveBirdToSlingShot()
    {
        while (Vector2.Distance(_activeBird.transform.position, _centreSlingShot.position) > _slerpSpeed)
        {
            _activeBird.transform.position = Vector2.MoveTowards(_activeBird.transform.position, _centreSlingShot.position, _slerpSpeed);
            yield return null;
        }

        _activeBird.transform.position = _centreSlingShot.position;
        _slingShotPrimed = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.BringNextBird)
        {
            GameManager.instance.BringNextBird = false;
            NextBird();
        }

        if (!_bgSlingshot.enabled)
        {
            _bgSlingshot.enabled = true;
        }

        if (!_fgSlingshot.enabled)
        {
            _fgSlingshot.enabled = true;
        }

        if (_slingShotPrimed)
        {

            if (Mouse.current.leftButton.wasPressedThisFrame && _slingshotArea.IsWithinCollider())
            {
                _wasClickedWithinCollider = true;
            }

            if (Mouse.current.leftButton.isPressed && _wasClickedWithinCollider)
            {
                PullSlingshot();
                PullBird();
            }
            else
            {
                ResetSlingshot();
            }

            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                _slingShotPrimed = false;
                _wasClickedWithinCollider = false;
                _activeBird.GetComponent<Bird>().Launch(_direction, _slingForce);

                GameManager.instance.AvailableBirds--;
            }
        }

    }

    void PullSlingshot()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        _clampedPosition = _centreSlingShot.position + Vector3.ClampMagnitude(mousePosition - _centreSlingShot.position, _maxDistance);

        _bgSlingshot.SetPosition(0, _bgSlingshotStart.position);
        _bgSlingshot.SetPosition(1, _clampedPosition);

        _fgSlingshot.SetPosition(0, _fgSlingshotStart.position);
        _fgSlingshot.SetPosition(1, _clampedPosition);

        _direction = _centreSlingShot.position - _clampedPosition;
    }

    void ResetSlingshot()
    {
        _bgSlingshot.SetPosition(0, _bgSlingshotStart.position);
        _bgSlingshot.SetPosition(1, _bgSlingshotStart.position);

        _fgSlingshot.SetPosition(0, _fgSlingshotStart.position);
        _fgSlingshot.SetPosition(1, _fgSlingshotStart.position);
    }

    void PullBird()
    {
        _activeBird.transform.position = (Vector2)_clampedPosition + (_direction.normalized * _birdOffset);
        _activeBird.transform.right = _direction.normalized;
    }

    void ResetBird()
    {
        _activeBird.transform.position = _centreSlingShot.position;
    }
}
