using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[Serializable]
public class UnityEventCallbackContext : UnityEvent<InputAction.CallbackContext> { }

public abstract class InputActionExecutor<TInputAction> : MonoBehaviour
{
	[SerializeField] protected UnityEventCallbackContext onActionStarted;
	public UnityEventCallbackContext _OnActionStarted { get { return this.onActionStarted; } }
	
	[SerializeField] protected UnityEventCallbackContext onActionPerformed;
	public UnityEventCallbackContext _OnActionPerformed { get { return this.onActionPerformed; } }
	
	[SerializeField] protected UnityEventCallbackContext onActionCanceled;
	public UnityEventCallbackContext _OnActionCanceled { get { return this.onActionCanceled; } }

	[SerializeField] protected TInputAction inputAction;

	public bool Active_ { get; protected set; }

	protected abstract void InitializeActions();
	public virtual void Activate()
	{
		if (this.Active_)
			return;

		this.InitializeActions();

		this.Active_ = true;
	}

	protected abstract void DeInitializeActions();
	public virtual void Deactivate()
	{
		if (!this.Active_)
			return;

		this.DeInitializeActions();

		this.Active_ = false;
	}

#if UNITY_EDITOR
#endif
}

public class InputActionReferenceExecutor : InputActionExecutor<InputActionReference>
{
	protected override void InitializeActions()
	{
		this.inputAction.action.started += this.onActionStarted.Invoke;
		this.inputAction.action.performed += this.onActionPerformed.Invoke;
		this.inputAction.action.canceled += this.onActionCanceled.Invoke;

		this.inputAction.action.Enable();
	}

	protected override void DeInitializeActions()
	{
		this.inputAction.action.started -= this.onActionStarted.Invoke;
		this.inputAction.action.performed -= this.onActionPerformed.Invoke;
		this.inputAction.action.canceled -= this.onActionCanceled.Invoke;

		this.inputAction.action.Disable();
	}

#if UNITY_EDITOR
#endif
}
