using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AppointmentSimulator.ViewModels;
using System.Threading.Tasks;
using AppointmentSimulator.Models;
namespace AppointmentSimulator.ViewModels
{
    public partial class AddNewAppointmentViewModel : ObservableObject
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

        private readonly AppointmentViewModel mainViewModel;

        public AddNewAppointmentViewModel(AppointmentViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
            SaveAppointmentCommand = new AsyncRelayCommand(SaveAppointmentAsync);
        }

        public ICommand SaveAppointmentCommand { get; }

        private async Task SaveAppointmentAsync()
        {
            if (string.IsNullOrWhiteSpace(Name) ||
                string.IsNullOrWhiteSpace(Subject))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Todos los campos son obligatorios.", "OK");
                return;
            }

            if (AppointmentDate < DateTime.Today)
            {
                await App.Current.MainPage.DisplayAlert("Error", "La fecha debe ser hoy o posterior.", "OK");
                return;
            }

            if (StartingTime >= EndingTime)
            {
                await App.Current.MainPage.DisplayAlert("Error", "La hora de inicio debe ser menor a la de término.", "OK");
                return;
            }

            // Verificar solapamiento de citas en la misma fecha
            bool solapamiento = GlobalData.Appointments.Any(a =>
                a.AppointmentDate == DateOnly.FromDateTime(this.AppointmentDate) &&
                (
                    (this.StartingTime < a.EndingTime && this.EndingTime > a.StartingTime)
                )
            );

            if (solapamiento)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Ya existe una cita que se solapa con el horario seleccionado.", "OK");
                return;
            }

            var newAppointment = new Appointment
            {
                Name = this.Name,
                Subject = this.Subject,
                AppointmentDate = DateOnly.FromDateTime(this.AppointmentDate),
                StartingTime = this.StartingTime,
                EndingTime = this.EndingTime
            };

            GlobalData.Appointments.Add(newAppointment);

            await App.Current.MainPage.Navigation.PopAsync();
        }
    }
}