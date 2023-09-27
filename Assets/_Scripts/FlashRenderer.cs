using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FlashRenderer
{
    SpriteRenderer _renderer;
    float _flash;
    static int s_FLASH = Shader.PropertyToID("_Flash");
    public FlashRenderer(SpriteRenderer renderer)
    {
        _renderer = renderer;
        _flash = 0f;
    }
    public void SetFlash()
    {
        _flash = 1.05f;
        _renderer.material.SetFloat(s_FLASH, 2f);
    }
    public void Reset()
    {
        _renderer.material.SetFloat(s_FLASH, 0f);
        _flash = 0f;
    }
    public void Update(float deltaTime)
    {
        if (_flash > 1f)
        {
            _flash -= deltaTime;
        }
        else if (_flash > 0f)
        {
            Reset();
        }
    }
}
