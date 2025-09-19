using AppointmentSimulator.Models;
using AppointmentSimulator.ViewModels;
using AppointmentSimulator.Pages;

namespace AppointmentSimulator.Pages
{
    public partial class MainPage : ContentPage
    {
        private AppointmentViewModel viewModel;
        private string idOculto;

        public MainPage()
        {
            InitializeComponent();
            viewModel = new AppointmentViewModel(null);
            BindingContext = viewModel;
            AppointmentsCollectionView.ItemsSource = GlobalData.Appointments;
        }

        private async void OnAddNewAppointmentClicked(object sender, EventArgs e)
        {
            var addViewModel = new AppointmentViewModel(viewModel);
            var addPage = new AddNewAppointmentPage
            {
                BindingContext = addViewModel
            };
            await Navigation.PushAsync(addPage);
        }

        private void AppointmentsCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Appointment selectedAppointment)
            {
                idOculto = selectedAppointment.Id;
            }
        }
    }
}
