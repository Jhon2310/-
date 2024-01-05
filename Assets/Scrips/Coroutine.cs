using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Coroutine : MonoBehaviour
{
    [FormerlySerializedAs("_cube")] [SerializeField] private Renderer _cubeRenderer;
    [SerializeField] private int countX = 10;
    [SerializeField] private int countZ = 10;
    [SerializeField] private Vector3 newPosition;

    [SerializeField] private float _distanceBetweenObjects = 1.5f;
    [SerializeField] private float _theTimeOfTheAppearanceOfObjects = 0.04f;
    [SerializeField] private float _timeToChangeTheColor = 0.2f;
    [FormerlySerializedAs("_time")] [SerializeField] private float _timeChangeColor = 0.04f;
    private List<Renderer> _renderers = new();
    private Color _color;
    private void Awake()
    {
        StartCoroutine(CubeSpawner());
    }
    private IEnumerator CubeSpawner()
    {
        var positionX = newPosition.x;
        for (int i = 0; i < countX; i++)
        {
            for (int j = 0; j < countZ; j++)
            {
                var cube = Instantiate(_cubeRenderer);
                _renderers.Add(cube);
                cube.transform.position = newPosition;
                newPosition.x+=_distanceBetweenObjects;
               
                yield return new WaitForSeconds(_theTimeOfTheAppearanceOfObjects);
            }
            newPosition.x = positionX;
            newPosition.z-=_distanceBetweenObjects;
           
            yield return new WaitForSeconds(_theTimeOfTheAppearanceOfObjects);
        }
    }
    private IEnumerator ChangeColorCoroutine()
    {
        Color randomColor = Random.ColorHSV(0.1f,1,0.8f,1,0.8f,1,0.8f,1);
        for (int i = 0; i < _renderers.Count; i++)
        {

            var startColor = _renderers[i].material.color;
            StartCoroutine(TimeChangeColor(_renderers[i], startColor, randomColor));
            yield return new WaitForSeconds(_timeToChangeTheColor); 
        }
    }

    private IEnumerator TimeChangeColor(Renderer renderer, Color startColor, Color endColor)
    {
        var time = 0f;
        while (time<_timeChangeColor)
        {
            renderer.material.color = Color.Lerp(startColor,endColor,time/_timeChangeColor);
            time += Time.deltaTime;
            yield return null;
        }
    }
    public void ChangeCubeColors()
    {
        StartCoroutine(ChangeColorCoroutine());
    }
    
}
