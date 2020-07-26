using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputActionExecutor : InputActionExecutor<InputAction>
{
	protected override void InitializeActions()
	{
		this.inputAction.started += this.onActionStarted.Invoke;
		this.inputAction.performed += this.onActionPerformed.Invoke;
		this.inputAction.canceled += this.onActionCanceled.Invoke;

		this.inputAction.Enable();
	}

	protected override void DeInitializeActions()
	{
		this.inputAction.started -= this.onActionStarted.Invoke;
		this.inputAction.performed -= this.onActionPerformed.Invoke;
		this.inputAction.canceled -= this.onActionCanceled.Invoke;

		this.inputAction.Disable();
	}
}
