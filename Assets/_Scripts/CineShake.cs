using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using AethicalTools;

public class CineShake : MonoBehaviour
{
    [SerializeField] float _shake, _loudShake = 0.01f;
    [SerializeField] float _time = 0.1f;
    [SerializeField] ObVoidEvent _onScreenshake;
    [SerializeField] ObVoidEvent _onLoudScreenshake;
    CinemachineBasicMultiChannelPerlin _channel;
    Ticker _ticker;
    Ticker _loud;
    private void Awake()
    {
        _ticker = new Ticker(_time, false);
        _loud = new Ticker(_time, false);
        _onScreenshake.Message += () => _ticker.WindUp();
        _onLoudScreenshake.Message += () => _loud.WindUp();
        _channel = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    // Update is called once per frame
    void Update()
    {
        float shake = (_ticker.Tick(Time.deltaTime)) ? 0f : _shake;
        if (!_loud.Tick(Time.deltaTime))
        {
            shake = _loudShake;
        }
        _channel.m_AmplitudeGain = shake;
    }
}
