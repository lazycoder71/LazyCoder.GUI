using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LazyCoder.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LazyCoder.Gui
{
    public class GuiNavPageContainer : MonoBase
    {
        [Title("Config")]
        [SerializeField] private int _groupIndex = 0;

        private readonly List<GuiNavPage> _pages = new();

        #region Functions -> Public

        public async UniTask<GuiNavPage> PushPageAsync(AssetReference pageAsset, GuiNavContext context)
        {
            // Instantiate the page from the addressable asset
            var handle = pageAsset.InstantiateAsync(TransformCached, false);

            // Await the handle with cancellation on destroy
            await handle.ToUniTask(cancellationToken: this.GetCancellationTokenOnDestroy());

            // Get the GuiNavPage component from the instantiated object
            GuiNavPage page = handle.Result.GetComponent<GuiNavPage>();

            // If the instantiated object does not have a GuiNavPage component, release it and log an error
            if (page == null)
            {
                LDebug.LogError<GuiNavPageContainer>(
                    $"The instantiated page from [{pageAsset}] does not have a {typeof(GuiNavPage)} component!");

                Addressables.ReleaseInstance(handle.Result);
                return null;
            }

            PushPage(page, context);

            return page;
        }

        private void PushPage(GuiNavPage page, GuiNavContext context)
        {
            // If the instantiated object does not have a GuiNavPage component, log an error
            if (page == null)
            {
                LDebug.LogError<GuiNavPageContainer>("The provided page is null!");
                return;
            }

            // Block the current top page, if any
            if (_pages.Count > 0)
                _pages[^1].BlockAsync().Forget();

            // Construct new page
            page.Construct(this, context);

            // Add to the page stack
            _pages.Add(page);

            // Open the new page
            page.OpenAsync().Forget();
        }

        public void ClosePage(GuiNavPage page)
        {
            if (_pages.Count > 1)
            {
                // If the page being closed is the top page, reveal the one beneath it
                if (_pages[^1] == page)
                    _pages[^2].RevealAsync().Forget();
            }

            _pages.Remove(page);

            // Release the page instance
            Addressables.ReleaseInstance(page.GameObjectCached);

            if (page != null)
                Destroy(page.GameObjectCached);
        }

        #endregion

        #region MonoBehaviour Callbacks

        protected override void Start()
        {
            base.Start();

            GuiNavPage childPage = GetComponentInChildren<GuiNavPage>();

            if (childPage != null)
                PushPage(childPage, null);
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            GuiNavPageContainerManager.AddContainer(this, _groupIndex);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            GuiNavPageContainerManager.RemoveContainer(this, _groupIndex);
        }

        private void OnDestroy()
        {
            foreach (var page in _pages)
            {
                if (page != null)
                    Addressables.ReleaseInstance(page.GameObjectCached);
            }
        }

        #endregion
    }
}