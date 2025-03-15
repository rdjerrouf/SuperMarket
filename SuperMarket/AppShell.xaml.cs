using SuperMarket.Views;

namespace SuperMarket
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Register routes for detail pages that aren't directly in the shell
            Routing.RegisterRoute("ProductDetailsPage", typeof(ProductDetailsPage));
        }
    }
}