using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScanlineBehaviour : MonoBehaviour
{
	public Sprite OtherScanlineImage;
	public float TimeToSwitchMs = 17; //about 1/60 sec

	private float _ttsAccum = 0;
	private Image _img;
	private Sprite _scanlineImage;
	private bool _useOther = false;

	private void Start()
	{
		_img = GetComponent<Image>();
		_scanlineImage = _img.sprite;
	}

	private void Update()
	{
		_ttsAccum += Time.deltaTime;
		if(_ttsAccum * 1000 >= TimeToSwitchMs)
		{
			_useOther = !_useOther;
			_img.sprite = (_useOther) ? OtherScanlineImage : _scanlineImage;
			_ttsAccum = 0f;
		}
	}
}
