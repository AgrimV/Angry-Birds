using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Camera")]
    [SerializeField] CameraManager _cameraManager;

    [Header("Bird")]
    [SerializeField] GameObject _birdPrefab;
    [SerializeField] Transform _spawnPosition;

    [Header("Sounds")]
    [SerializeField] AudioClip _pullSound;
    [SerializeField] AudioClip _releaseSound;
    [SerializeField] AudioSource _audioSource;

    [Header("Variables")]
    [SerializeField] SlingshotArea _slingshotArea;
    [SerializeField] float _maxDistance = 2.0f;
    [SerializeField] float _spawnSpacing = 1.0f;
    [SerializeField] float _birdOffset = 0.2f;
    [SerializeField] float _slingForce = 10.0f;
    [SerializeField] float _primeSpeed = 0.2f;

    Vector3 _clampedPosition;
    Queue<GameObject> _spawnedQueue;
    GameObject _activeBird;
    bool _wasClickedWithinCollider;
    Vector2 _direction;
    int _spawnCount;
    bool _slingShotPrimed = false;

    Controls _gameControls;

    private void Awake()
    {
        _fgSlingshot.enabled = false;
        _bgSlingshot.enabled = false;

        _spawnCount = GameManager.instance.BirdCount;

        _spawnedQueue = new Queue<GameObject>();

        _gameControls = new Controls();

        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _gameControls.GameControls.Enable();
    }

    private void OnDisable()
    {
        _gameControls.GameControls.Disable();
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
            _activeBird.GetComponent<Bird>().Despawn();
        }

        _activeBird = _spawnedQueue.Dequeue();

        _activeBird.GetComponent<Renderer>().sortingOrder += 1;
        _activeBird.GetComponent<Rigidbody2D>().isKinematic = true;

        float distance = Vector2.Distance(_activeBird.transform.position, _centreSlingShot.position);
        float time = distance / _primeSpeed;

        _activeBird.transform.DOMove(_centreSlingShot.position, time).SetEase(Ease.Linear);

        StartCoroutine(MoveBirdToSlingShot(_activeBird, time));
    }

    IEnumerator MoveBirdToSlingShot(GameObject bird, float time)
    {
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            _activeBird.transform.position = bird.transform.position;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _activeBird.transform.position = _centreSlingShot.position;
        _slingShotPrimed = true;
        _cameraManager.SwitchToIdleCam();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.GamePaused)
        {
            return;
        }

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

            if (InputManager.WasPressedThisFrame && _slingshotArea.IsWithinCollider())
            {
                SoundManager.instance.PlayClip(_pullSound, _audioSource);
                _wasClickedWithinCollider = true;
            }

            if (InputManager.IsPressed && _wasClickedWithinCollider)
            {
                PullSlingshot();
                PullBird();
            }
            else
            {
                ResetSlingshot();
            }

            if (InputManager.WasReleasedThisFrame && _wasClickedWithinCollider)
            {
                _cameraManager.SwitchToFollowCam(_activeBird.transform);
                _slingShotPrimed = false;
                _wasClickedWithinCollider = false;
                SoundManager.instance.PlayClip(_releaseSound, _audioSource);
                _activeBird.GetComponent<Bird>().Launch(_direction, _slingForce);

                GameManager.instance.BirdsFired();
            }
        }

    }

    void PullSlingshot()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(InputManager.MousePosition);

        _clampedPosition = _centreSlingShot.position + (Vector3)Vector2.ClampMagnitude(mousePosition - _centreSlingShot.position, _maxDistance);

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
        _activeBird.transform.position = (Vector2)_fgSlingshot.GetPosition(1) + (_direction.normalized * _birdOffset);
        _activeBird.transform.right = _direction.normalized;
    }

    void ResetBird()
    {
        _activeBird.transform.position = _centreSlingShot.position;
    }
}
