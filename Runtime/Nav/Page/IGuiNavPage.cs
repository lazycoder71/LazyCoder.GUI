namespace LazyCoder.Gui
{
    public interface IGuiNavPage
    {
        void OnConstruct(GuiNavPage page, GuiNavContext context);

        void OnStateChanged(GuiNavPage.State currentState);
    }
}