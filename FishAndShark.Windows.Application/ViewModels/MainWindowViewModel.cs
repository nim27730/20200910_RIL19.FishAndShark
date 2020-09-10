using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using RIL19.FishAndShark.Windows.Application.Data;
using RIL19.FishAndShark.Windows.Application.Services;
using Serilog;

namespace RIL19.FishAndShark.Windows.Application.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Notification
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Properties
        private string _windowTitle;
        public string WindowTitle
        {
            get => _windowTitle;
            set
            {
                _windowTitle = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Aquarium> _aquariums;
        public ObservableCollection<Aquarium> Aquariums
        {
            get => _aquariums;
            set
            {
                _aquariums = value;
                OnPropertyChanged();
            }
        }

        private Aquarium _selectedAquarium;
        public Aquarium SelectedAquarium
        {
            get => _selectedAquarium;
            set
            {
                _selectedAquarium = value;
                OnPropertyChanged();
            }
        }
        #endregion

        private readonly IAquariumApi _api;
        private readonly ILogger _logger;
        private readonly HubConnection _connection;

        public MainWindowViewModel(IAquariumApi api, ILogger logger, HubConnection connection)
        {
            _api = api;
            WindowTitle = "FishAndShark";
            _logger = logger;
            _connection = connection;
            _connection.On<Guid>("AquariumCreated", OnAquariumCreated);
        }

        private async Task OnAquariumCreated(Guid aquariumId)
        {
            try
            {
                _logger.Information($"OnAquariumCreated message received. [aquariumId:{aquariumId}]");

                var aquarium = await _api.GetAsync(aquariumId.ToString());
                if (aquarium == null)
                    _logger.Information("Aquarium not found!");
                else
                {
                    Aquariums.Add(aquarium);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "OnAquariumCreated");
            }
        }

        public void RefreshAquariumList()
        {
            try
            {
                var aquariums = _api.ListAsync().GetAwaiter().GetResult();
                Aquariums = aquariums;
            }
            catch (Exception e)
            {
                _logger.Error(e, "RefreshAquariumList");
            }
        }

        public void CreerAquarium(string name, double hauteur, double largeur)
        {
            try
            {
                var request = new { Height = hauteur, Width = largeur, Name = name };

                var guid = _api.CreateAsync(request).GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                _logger.Error(e, "CreerAquarium");
            }

            WindowTitle = $"FishAndShark-({Aquariums.Count})";
        }
    }
}
