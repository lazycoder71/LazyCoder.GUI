using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LazyCoder.Gui
{
    public class GuiNavButton : GuiButton
    {
        [SerializeField] private AssetReference _page;
        
        public override void Button_OnClick()
        {
            base.Button_OnClick();
            
            GuiNav.PushPageAsync(_page).Forget();
        }
    }
}
