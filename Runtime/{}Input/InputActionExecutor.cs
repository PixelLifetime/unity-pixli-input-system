using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PixLi
{
	public abstract class InputActionExecutor<TInputAction> : MonoBehaviour
	{
		[SerializeField] protected UnityEvent<InputAction.CallbackContext> onActionStarted;
		public UnityEvent<InputAction.CallbackContext> _OnActionStarted { get { return this.onActionStarted; } }
		
		[SerializeField] protected UnityEvent<InputAction.CallbackContext> onActionPerformed;
		public UnityEvent<InputAction.CallbackContext> _OnActionPerformed { get { return this.onActionPerformed; } }
		
		[SerializeField] protected UnityEvent<InputAction.CallbackContext> onActionCanceled;
		public UnityEvent<InputAction.CallbackContext> _OnActionCanceled { get { return this.onActionCanceled; } }

		[SerializeField] protected TInputAction inputAction;
		public TInputAction _InputAction => this.inputAction;

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
		//protected override void OnDrawGizmos()
		//{
		//}
#endif
	}

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

#if UNITY_EDITOR
		[CustomEditor(typeof(InputActionExecutor))]
		[CanEditMultipleObjects]
		public class InputActionExecutorEditor : Editor
		{
#pragma warning disable 0219, 414
			private InputActionExecutor _sInputActionExecutor;
#pragma warning restore 0219, 414

			private void OnEnable()
			{
				this._sInputActionExecutor = this.target as InputActionExecutor;
			}

			private bool _eventsFoldout;

			public override void OnInspectorGUI()
			{
				this._eventsFoldout = EditorGUILayout.Foldout(this._eventsFoldout, new GUIContent("Events"), true);

				if (this._eventsFoldout)
				{
					EditorGUILayout.PropertyField(this.serializedObject.FindProperty("onActionStarted"), true);
					EditorGUILayout.PropertyField(this.serializedObject.FindProperty("onActionPerformed"), true);
					EditorGUILayout.PropertyField(this.serializedObject.FindProperty("onActionCanceled"), true);
				}

				EditorGUILayout.PropertyField(this.serializedObject.FindProperty("inputAction"), true);

				this.serializedObject.ApplyModifiedProperties();
			}
		}
#endif
	}
}