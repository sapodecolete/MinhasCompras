namespace MinhasCompras.Views
{
    public partial class RelatorioPage : ContentPage
    {
        public RelatorioPage()
        {
            InitializeComponent();
            dataFimPicker.Date = DateTime.Now;
        }

        private async void OnFiltrarClicked(object sender, EventArgs e)
        {
            try
            {
                var produtos = await App.Db.GetByDateRange(
                    dataInicioPicker.Date,
                    dataFimPicker.Date.AddDays(1)); // Inclui o dia final

                listaProdutos.ItemsSource = produtos;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }
    }
}