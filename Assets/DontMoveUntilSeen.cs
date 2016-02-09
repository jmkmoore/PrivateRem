using UnityEngine;
using System.Collections;

public class DontMoveUntilSeen : MonoBehaviour {

    private Animator _animator;
    private Renderer renderer;
    private bool isVisible;

	// Use this for initialization
	void Start () {
        _animator = GetComponentInChildren<Animator>();
        renderer = GetComponentInChildren<Renderer>();

	}
	
	// Update is called once per frame
	void Update () {
        isVisible = renderer.isVisible;
        if (isVisible)
        {
            _animator.Play(Animator.StringToHash("Move"));
        }
    }
}
