using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using AppointmentSimulator.Models;
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

        private readonly AppointmentViewModel mainViewModel;

        public AppointmentViewModel(AppointmentViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
            SaveAppointmentCommand = new AsyncRelayCommand(SaveAppointmentAsync);
            RemoveAppointmentByIdCommand = new AsyncRelayCommand<string>(RemoveAppointmentById);
        }

        public ICommand SaveAppointmentCommand { get; }
        public ICommand RemoveAppointmentByIdCommand { get; }

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
                Id = Guid.NewGuid().ToString(),
                Name = this.Name,
                Subject = this.Subject,
                AppointmentDate = DateOnly.FromDateTime(this.AppointmentDate),
                StartingTime = this.StartingTime,
                EndingTime = this.EndingTime
            };

            GlobalData.Appointments.Add(newAppointment);

            await App.Current.MainPage.Navigation.PopAsync();
        }
        
        public async Task RemoveAppointmentById(string? id)
        {
            if (id == null)
            {
                await App.Current.MainPage.DisplayAlert("Error", "El registro no existe", "OK");
                return;
            }

            var appointment = GlobalData.Appointments.FirstOrDefault(a => a.Id == id);
            if (appointment != null)
            {
                GlobalData.Appointments.Remove(appointment);
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "El registro no existe", "OK");
            }
        }
    }
}
