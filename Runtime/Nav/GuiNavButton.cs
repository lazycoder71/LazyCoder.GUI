using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LazyCoder.Gui
{
    public class GuiNavButton : GuiButton
    {
        [Title("Page")]
        [Required]
        [SerializeField] private AssetReference _page;

        [Title("Container")]
        [SerializeField] private GuiNavPageContainer _container;

        [ShowIf("@!_container")]
        [SerializeField] private int _containerGroupIndex;

        private bool _isOpening = false;

        protected virtual GuiNavContext Context => null;

        public override void Button_OnClick()
        {
            base.Button_OnClick();

            PushPageAsync().Forget();
        }

        private async UniTask PushPageAsync()
        {
            if (_isOpening)
                return;

            _isOpening = true;

            GuiNavPage page;

            if (_container != null)
                page = await GuiNav.PushPageAsync(_page, _container, Context);
            else
                page = await GuiNav.PushPageAsync(_page, _containerGroupIndex, Context);

            _isOpening = false;

            OnPagePushed(page);
        }

        protected virtual void OnPagePushed(GuiNavPage page)
        {
        }
    }
}