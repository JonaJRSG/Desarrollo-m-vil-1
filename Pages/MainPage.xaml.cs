using AppointmentSimulator.Models;
using AppointmentSimulator.ViewModels;
using AppointmentSimulator.Pages;

namespace AppointmentSimulator.Pages
{
    public partial class MainPage : ContentPage
    {
        private AppointmentViewModel viewModel;

        public MainPage()
        {
            InitializeComponent();
            viewModel = new AppointmentViewModel();
            BindingContext = viewModel;
            AppointmentsCollectionView.ItemsSource = GlobalData.Appointments;
        }

        private async void OnAddNewAppointmentClicked(object sender, EventArgs e)
        {
            var addViewModel = new AddNewAppointmentViewModel(viewModel);
            var addPage = new AddNewAppointmentPage
            {
                BindingContext = addViewModel
            };
            await Navigation.PushAsync(addPage);
        }
    }
}
