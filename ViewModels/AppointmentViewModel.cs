using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AppointmentSimulator.ViewModels
{
    public partial class AppointmentViewModel : ObservableObject
    {
        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string subject;

        [ObservableProperty]
        private DateTime appointmentDate = DateTime.Today;

        [ObservableProperty]
        private TimeSpan startingTime;

        [ObservableProperty]
        private TimeSpan endingTime;

        [ObservableProperty]
        private ObservableCollection<AppointmentViewModel> appointments = new();

        [ObservableProperty]
        private AppointmentViewModel selectedAppointment;

        public AppointmentViewModel()
        {
            AddAppointmentCommand = new RelayCommand(AddAppointment);
            DeleteAppointmentCommand = new RelayCommand(DeleteAppointment, CanDeleteAppointment);
        }

        public ICommand AddAppointmentCommand { get; }
        public ICommand DeleteAppointmentCommand { get; }

        private void AddAppointment()
        {
            var newAppointment = new AppointmentViewModel
            {
                Name = this.Name,
                Subject = this.Subject,
                AppointmentDate = this.AppointmentDate,
                StartingTime = this.StartingTime,
                EndingTime = this.EndingTime
            };
            Appointments.Add(newAppointment);

            // Limpiar campos
            Name = string.Empty;
            Subject = string.Empty;
            AppointmentDate = DateTime.Today;
            StartingTime = TimeSpan.Zero;
            EndingTime = TimeSpan.Zero;
        }

        private void DeleteAppointment()
        {
            if (SelectedAppointment != null)
            {
                Appointments.Remove(SelectedAppointment);
                SelectedAppointment = null;
            }
        }

        private bool CanDeleteAppointment()
        {
            return SelectedAppointment != null;
        }
    }
}
