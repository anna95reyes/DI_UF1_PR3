using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NBA_BD.Model
{
    public class TeamEditor : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int teamId;
        private String teamCaption;
        private String teamShortCaption;
        private Division divisionCaption;
        private String teamLogo;
        private String arenaCaption;
        private String arenaLogo;
        private String arenaAbout;
        private int arenaCapacity;
        private String arenaPhoto;
        private double arenaLat;
        private double arenaLong;

        public TeamEditor(int teamId, string teamCaption, string teamShortCaption, Division divisionCaption, string teamLogo, string arenaCaption, string arenaLogo, string arenaAbout, int arenaCapacity, string arenaPhoto, double arenaLat, double arenaLong)
        {
            TeamId = teamId;
            TeamCaption = teamCaption;
            TeamShortCaption = teamShortCaption;
            DivisionCaption = divisionCaption;
            TeamLogo = teamLogo;
            ArenaCaption = arenaCaption;
            ArenaLogo = arenaLogo;
            ArenaAbout = arenaAbout;
            ArenaCapacity = arenaCapacity;
            ArenaPhoto = arenaPhoto;
            ArenaLat = arenaLat;
            ArenaLong = arenaLong;
        }

        public int TeamId { get => teamId; set => teamId = value; }
        public string TeamCaption { get => teamCaption; set => teamCaption = value; }
        public string TeamShortCaption { get => teamShortCaption; set => teamShortCaption = value; }
        public Division DivisionCaption { get => divisionCaption; set => divisionCaption = value; }
        public string TeamLogo { get => teamLogo; set => teamLogo = value; }
        public string ArenaCaption { get => arenaCaption; set => arenaCaption = value; }
        public string ArenaLogo { get => arenaLogo; set => arenaLogo = value; }
        public string ArenaAbout { get => arenaAbout; set => arenaAbout = value; }
        public int ArenaCapacity { get => arenaCapacity; set => arenaCapacity = value; }
        public string ArenaPhoto { get => arenaPhoto; set => arenaPhoto = value; }
        public double ArenaLat { get => arenaLat; set => arenaLat = value; }
        public double ArenaLong { get => arenaLong; set => arenaLong = value; }
    }
}
