namespace Example.FormsApp.Shell
{
    using Xamarin.Forms;

    public static class ShellProperty
    {
        public static readonly BindableProperty TitleProperty = BindableProperty.CreateAttached(
            "Title",
            typeof(string),
            typeof(ShellProperty),
            null,
            propertyChanged: PropertyChanged);

        public static string GetTitle(BindableObject view)
        {
            return (string)view.GetValue(TitleProperty);
        }

        public static void SetTitle(BindableObject view, string value)
        {
            view.SetValue(TitleProperty, value);
        }

        private static void PropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var parent = ((ContentView)bindable).Parent;
            if (parent is null)
            {
                return;
            }

            if (parent.BindingContext is IShellControl shell)
            {
                UpdateShellControl(shell, bindable);
            }
        }

        public static void UpdateShellControl(IShellControl shell, BindableObject bindable)
        {
            shell.Title.Value = bindable is null ? string.Empty : GetTitle(bindable);
        }
    }
}