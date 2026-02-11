using Cysharp.Threading.Tasks;
using LazyCoder.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LazyCoder.Gui
{
    [RequireComponent(typeof(CanvasGroup))]
    public class GuiNavPage : MonoBase
    {
        public enum State
        {
            Opening,
            Opened,

            Closing,
            Closed,

            Revealing,
            Revealed,

            Blocking,
            Blocked
        }

        [FoldoutGroup("Animation")]
        [LabelText("Open")]
        [SerializeField] private GuiNavPageAnimation _animationOpen;

        [FoldoutGroup("Animation")]
        [LabelText("Close")]
        [SerializeField] private GuiNavPageAnimation _animationClose;

        [FoldoutGroup("Animation")]
        [LabelText("Reveal")]
        [SerializeField] private GuiNavPageAnimation _animationReveal;

        [FoldoutGroup("Animation")]
        [LabelText("Block")]
        [SerializeField] private GuiNavPageAnimation _animationBlock;

        [Title("Config")]
        [SerializeField] private bool _closeWhenBackButtonPressed = false;

        private GuiNavPageContainer _container;

        private CanvasGroup _canvasGroup;

        private IGuiNavPage[] _listeners;

        private State _currentState = State.Opening;

        private readonly CancelToken _cancelToken = new();

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();

            if (_closeWhenBackButtonPressed)
                GameObjectCached.AddComponent<GuiNavPagePressBackHandler>();
        }

        private void OnDestroy()
        {
            _cancelToken.Cancel();
        }

        #endregion

        private void SwitchState(State newState)
        {
            // Update state
            _currentState = newState;

            switch (newState)
            {
                case State.Opened:
                case State.Revealed:
                    _canvasGroup.interactable = true;
                    break;

                default:
                    _canvasGroup.interactable = false;
                    break;
            }

            // Notify listeners
            for (int i = 0; i < _listeners.Length; i++)
                _listeners[i].OnStateChanged(_currentState);
        }

        [Button]
        private void AddBackground()
        {
            GuiNavPageBackground.Spawn(this);
        }

        #region Functions -> Public

        public void Construct(GuiNavPageContainer container, GuiNavContext context)
        {
            _container = container;

            _listeners = GetComponentsInChildren<IGuiNavPage>();

            for (int i = 0; i < _listeners.Length; i++)
                _listeners[i].OnConstruct(this, context);
        }

        public async UniTask OpenAsync()
        {
            SwitchState(State.Opening);

            _cancelToken.Cancel();

            await _animationOpen.Play(_cancelToken.Token);

            SwitchState(State.Opened);
        }

        public void Close()
        {
            CloseAsync().Forget();
        }

        public async UniTask CloseAsync()
        {
            if (_currentState != State.Opened && _currentState != State.Revealed)
                return;

            SwitchState(State.Closing);

            _cancelToken.Cancel();

            await _animationClose.Play(_cancelToken.Token);

            SwitchState(State.Closed);

            _container.ClosePage(this);
        }

        public async UniTask RevealAsync()
        {
            SwitchState(State.Revealing);

            // Stop any blocking animation
            _animationBlock.Stop();

            _cancelToken.Cancel();

            await _animationReveal.Play(_cancelToken.Token);

            SwitchState(State.Revealed);
        }

        public async UniTask BlockAsync()
        {
            SwitchState(State.Blocking);

            _cancelToken.Cancel();

            await _animationBlock.Play(_cancelToken.Token);
            
            SwitchState(State.Blocked);
        }
        
        public void SetEnabled(bool isEnabled)
        {
            _canvasGroup.interactable = isEnabled;
        }

        #endregion
    }
}