using Pcx;
using UnityEngine;
using UnityEngine.UI;

public class PointCloudController : MonoBehaviour
{
    [SerializeField] private PointCloudRenderer _renderer;
    [SerializeField] private Slider _progressBar;

    public bool _loadFromUrl = true;
    
    [Header("Load file from disk")]
    [SerializeField] private string _localPath;
    [Header("Load file from url")]
    [SerializeField] private string _url;
    [Header("Point cloud controls")]
    [SerializeField][Range(1,20)] private int _sparseness;
    [SerializeField][Range(1,5)] private int _increment = 1;
    [SerializeField][Range(2,10)] private int _rotationSpeed = 1;

    private bool _rotate;
    private Vector3 _originPos;
    private Quaternion _originalRot;

    private void Start()
    {
        if (_loadFromUrl)
            LoadFileFromUrl();
        else
            LoadFileFromDisk();

        _originPos = _renderer.gameObject.transform.localPosition;
        _originalRot = _renderer.gameObject.transform.localRotation;
    }

    private void Update()
    {
        if(_rotate)
            Rotate();
    }

    private void LoadFileFromDisk()
    {
        // Import and convert our model
        var data = RuntimeConverter.ConvertToPointCloudData(_localPath);
        _renderer.sourceData = data;
    }

    private void LoadFileFromUrl()
    {
        StartCoroutine(DownloadService.DownloadFileAsync(_url, 
            progress =>
        {
            _progressBar.value = progress;
            
        }, (path) =>
        {
            // Import and convert our model
            var data = RuntimeConverter.ConvertToPointCloudData(path);
            _renderer.sourceData = data;
            
            _progressBar.gameObject.SetActive(false);
        }));
    }

    public void Reset()
    {
        _sparseness = 1;
        var buffer = _renderer.sourceData.GetNewDensity(_sparseness);
        _renderer.sourceBuffer = buffer;

        _renderer.transform.localPosition = _originPos;
        _renderer.transform.localRotation = _originalRot;
    }

    public void IncreaseSparseness()
    {
        _sparseness -= _increment;
        _sparseness = Mathf.Clamp(_sparseness, 1, 20);

        var buffer = _renderer.sourceData.GetNewDensity(_sparseness);
        _renderer.sourceBuffer = buffer;
    }

    public void DecreaseSparseness()
    {
        _sparseness += _increment;
        _sparseness = Mathf.Clamp(_sparseness, 1, 20);

        var buffer = _renderer.sourceData.GetNewDensity(_sparseness);
        _renderer.sourceBuffer = buffer;
    }

    public void LowerIncrement()
    {
        _increment -= 1;
        _increment = Mathf.Clamp(_increment, 1, 5);
        Debug.Log($"Increment: {_increment}");
    }

    public void RaiseIncrement()
    {
        _increment += 1;
        _increment = Mathf.Clamp(_increment, 1, 5);
        Debug.Log($"Increment: {_increment}");
    }

    public void StartRotate()
    {
        _rotate = true;
    }
    
    public void StopRotate()
    {
        _rotate = false;
    }

    public void RotateFaster()
    {
        _rotationSpeed += 2;
        _rotationSpeed = Mathf.Clamp(_rotationSpeed, 1, 10);
        Debug.Log($"Rotation Speed: {_rotationSpeed}");
    }

    public void RotateSLower()
    {
        _rotationSpeed -= 2;
        _rotationSpeed = Mathf.Clamp(_rotationSpeed, 1, 10);
        Debug.Log($"Rotation Speed: {_rotationSpeed}");
    }

    private void Rotate()
    {
        _renderer.gameObject.transform.Rotate(Vector3.forward, _rotationSpeed * Time.deltaTime, Space.Self);
    }
}


