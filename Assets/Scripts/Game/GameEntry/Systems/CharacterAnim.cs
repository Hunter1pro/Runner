using System;
using UnityEngine;

[Serializable]
public class AnimParamView
{
    [SerializeField]
    private string _animParamName;
    public int AnimParamHash { get; private set; }

    public void Init()
    {
        AnimParamHash = Animator.StringToHash(_animParamName);
    }
}

public class CharacterAnim : MonoBehaviour, ICharacterAnim
{
    [SerializeField] 
    private Animator _animator;

    [SerializeField] 
    private AnimParamView _moveParam;
    
    private void Start()
    {
        _moveParam.Init();
    }

    public void PlayRun()
    {
        _animator.SetFloat(_moveParam.AnimParamHash, 0.8f);
    }

    public void PlayIdle()
    {
        _animator.SetFloat(_moveParam.AnimParamHash, 0);
    }
}

public interface ICharacterAnim
{
    void PlayRun();
    void PlayIdle();
}