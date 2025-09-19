using AppointmentSimulator.Models;
using AppointmentSimulator.ViewModels;
using AppointmentSimulator.Pages;

namespace AppointmentSimulator.Pages
{
    public partial class MainPage : ContentPage
    {
        private AppointmentViewModel viewModel;
        private string idOculto;

        /// <summary>
        /// 
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            viewModel = new AppointmentViewModel(null);
            BindingContext = viewModel;
            AppointmentsCollectionView.ItemsSource = GlobalData.Appointments;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnAddNewAppointmentClicked(object sender, EventArgs e)
        {
            var addViewModel = new AppointmentViewModel(viewModel);
            var addPage = new AddNewAppointmentPage
            {
                BindingContext = addViewModel
            };
            await Navigation.PushAsync(addPage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppointmentsCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Appointment selectedAppointment)
            {
                idOculto = selectedAppointment.Id;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnDeleteAppointmentClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(idOculto))
            {
                await DisplayAlert("Error", "No hay ninguna cita seleccionada para eliminar.", "OK");
                return;
            }
            var confirm = await DisplayAlert("Confirmar", "¿Estás seguro de que deseas eliminar esta cita?", "Sí", "No");
            if (confirm)
            {
                var AppointmentViewModel = new AppointmentViewModel(viewModel);
                await AppointmentViewModel.RemoveAppointmentById(idOculto);
                AppointmentsCollectionView.SelectedItem = null; // Deseleccionar el ítem
                idOculto = null; // Limpiar el id oculto
            }
        }
    }
}
