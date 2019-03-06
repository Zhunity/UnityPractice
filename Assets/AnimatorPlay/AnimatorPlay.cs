using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorPlay : MonoBehaviour {

	// Use this for initialization
	Animator animator;
	public float time = 1;

	void Start () {
		animator = gameObject.GetComponent<Animator>();
	}

	private void OnEnable()
	{
		if(animator != null)
			animator.Play("Roate");
	}

	private void OnGUI()
	{
		// 过渡
		if (GUI.Button(new Rect(0, 0, 100, 50), "CrossFade"))
		{
			animator.CrossFade("Shake", time);
		}

		// https://docs.unity3d.com/ScriptReference/Animator.PlayInFixedTime.html 不是很懂，也没试出什么效果
		if (GUI.Button(new Rect(0, 100, 100, 50), "PlayInFixedTime"))
		{
			animator.PlayInFixedTime("Shake", 0, time);
		}

		// 直接播放
		if (GUI.Button(new Rect(0, 200, 100, 50), "Play"))
		{
			animator.Play("Shake");
		}

		// 延时播放
		if (GUI.Button(new Rect(0, 300, 100, 50), "DelayPlayAnim"))
		{
			DelayPlayAnim("Shake", time);
		}

		if (GUI.Button(new Rect(0, 400, 100, 50), "Reset"))//点击后返回true
		{
			animator.Play("Roate");
		}
	}

	Coroutine _delayAnim;
	public void DelayPlayAnim(string animName, float delayTime)
	{
		if (_delayAnim != null)
		{
			StopCoroutine(_delayAnim);
		}
		_delayAnim = StartCoroutine(DelayAnimFun(animName, delayTime));
	}

	private IEnumerator DelayAnimFun(string animName, float delayTime)
	{
		yield return new WaitForSeconds(delayTime);
		animator.Play(animName);
	}
}
