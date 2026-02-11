using Cysharp.Threading.Tasks;
using LazyCoder.Core;
using UnityEngine.AddressableAssets;

namespace LazyCoder.Gui
{
    public static class GuiNav
    {
        public static async UniTask<GuiNavPage> PushPageAsync(AssetReference pageAsset,
            GuiNavPageContainer pageContainer, GuiNavContext context = null)
        {
            // If no page container is available, log an error and return null
            if (pageContainer == null)
            {
                LDebug.LogError(typeof(GuiNav),
                    $"No {nameof(GuiNavPageContainer)} available to push page {pageAsset}");

                return null;
            }

            // Push the page to the page container
            return await pageContainer.PushPageAsync(pageAsset, context);
        }

        public static async UniTask<GuiNavPage> PushPageAsync(AssetReference pageAsset, int groupIndex = 0,
            GuiNavContext context = null)
        {
            // Get the page container for the specified group index
            GuiNavPageContainer pageContainer = GuiNavPageContainerManager.GetContainer(groupIndex);

            return await PushPageAsync(pageAsset, pageContainer, context);
        }
    }
}