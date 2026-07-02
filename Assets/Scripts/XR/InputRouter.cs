using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SignTrainer.XR
{
    public class InputRouter : MonoBehaviour
    {
        [SerializeField] private InputActionReference confirmAction;
        [SerializeField] private InputActionReference backAction;
        [SerializeField] private InputActionReference menuAction;

        public event Action ConfirmPressed;
        public event Action BackPressed;
        public event Action MenuRequested;

        private void OnEnable()
        {
            Subscribe(confirmAction, OnConfirm);
            Subscribe(backAction, OnBack);
            Subscribe(menuAction, OnMenu);
        }

        private void OnDisable()
        {
            Unsubscribe(confirmAction, OnConfirm);
            Unsubscribe(backAction, OnBack);
            Unsubscribe(menuAction, OnMenu);
        }

        private void Subscribe(InputActionReference reference, Action<InputAction.CallbackContext> handler)
        {
            if (reference == null || reference.action == null) return;
            reference.action.performed += handler;
            reference.action.Enable();
        }

        private void Unsubscribe(InputActionReference reference, Action<InputAction.CallbackContext> handler)
        {
            if (reference == null || reference.action == null) return;
            reference.action.performed -= handler;
        }

        private void OnConfirm(InputAction.CallbackContext _) => ConfirmPressed?.Invoke();
        private void OnBack(InputAction.CallbackContext _) => BackPressed?.Invoke();
        private void OnMenu(InputAction.CallbackContext _) => MenuRequested?.Invoke();
    }
}
