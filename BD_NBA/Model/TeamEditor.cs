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
        private String divisionCaption;
        private String teamLogo;
        private String arenaCaption;
        private String arenaLogo;
        private String arenaAbout;
        private int arenaCapacity;
        private String arenaPhoto;
        private Decimal arenaLat;
        private Decimal arenaLong;

        public TeamEditor(string teamLogo, string teamCaption, string divisionCaption, string arenaCaption)
        {
            TeamLogo = teamLogo;
            TeamCaption = teamCaption;
            DivisionCaption = divisionCaption;
            ArenaCaption = arenaCaption;
        }

        public TeamEditor(int teamId, string teamCaption, string teamShortCaption, string divisionCaption, string teamLogo, string arenaCaption, string arenaLogo, string arenaAbout, int arenaCapacity, string arenaPhoto, decimal arenaLat, decimal arenaLong)
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
        public string DivisionCaption { get => divisionCaption; set => divisionCaption = value; }
        public string TeamLogo { get => teamLogo; set => teamLogo = value; }
        public string ArenaCaption { get => arenaCaption; set => arenaCaption = value; }
        public string ArenaLogo { get => arenaLogo; set => arenaLogo = value; }
        public string ArenaAbout { get => arenaAbout; set => arenaAbout = value; }
        public int ArenaCapacity { get => arenaCapacity; set => arenaCapacity = value; }
        public string ArenaPhoto { get => arenaPhoto; set => arenaPhoto = value; }
        public decimal ArenaLat { get => arenaLat; set => arenaLat = value; }
        public decimal ArenaLong { get => arenaLong; set => arenaLong = value; }
    }
}
