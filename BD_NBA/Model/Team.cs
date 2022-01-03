using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NBA_BD.Model
{
    public class Team : INotifyPropertyChanged
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

        public Team(int teamId, string teamCaption, string teamShortCaption, Division divisionCaption, string teamLogo, string arenaCaption, string arenaLogo, string arenaAbout, int arenaCapacity, string arenaPhoto, double arenaLat, double arenaLong)
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
        public string TeamCaption {
            get
            {
                return teamCaption;
            }
            set
            {
                if (!validaText(value)) throw new Exception("La caption de Team es obligatoria");
                teamCaption = value;
            }
        }
        public string TeamShortCaption {
            get
            {
                return teamShortCaption;
            }
            set
            {
                if (value != null && !validaShortCaption(value)) throw new Exception("La short caption de Team ha de tenir 3 caracters");
                teamShortCaption = value;
            }
        }
        public Division DivisionCaption {
            get
            {
                return divisionCaption;
            }
            set
            {
                if (!validaDivision(value)) throw new Exception("La caption de Division ha de tenir minim 4 caracters");
                divisionCaption = value;
            }
        }
        public string TeamLogo {
            get
            {
                return teamLogo;
            }
            set
            {
                teamLogo = value;
            }  
        }
        public string ArenaCaption {
            get
            {
                return arenaCaption;
            }
            set
            {
                if (!validaText(value)) throw new Exception("La caption de Arena es obligatoria");
                arenaCaption = value;
            }
        }
        public string ArenaLogo {
            get
            {
                return arenaLogo;
            }
            set
            {
                if (!validaText(value)) throw new Exception("El Logo de Arena es obligatori");
                arenaLogo = value;
            }
        }
        public string ArenaAbout { 
            get
            {
                return arenaAbout;
            }
            set
            {
                if (!validaText(value)) throw new Exception("La About de Arena es obligatoria");
                arenaAbout = value;
            } 
        }
        public int ArenaCapacity { 
            get
            {
                return arenaCapacity;
            }
            set
            {
                if (!validaCapacity(value)) throw new Exception("La Capacity de Arena ha de ser positiva");
                arenaCapacity = value;
            } 
        }
        public string ArenaPhoto { 
            get
            {
                return arenaPhoto;
            }
            set
            {
                if (!validaText(value)) throw new Exception("La Photo de Arena es obligatoria");
                arenaPhoto = value;
            } 
        }
        public double ArenaLat { 
            get
            {
                return arenaLat;
            }
            set
            {
                if (!validaLatitud(value)) throw new Exception("La Latitud ha d'estat entre -90 i 90");
                arenaLat = value;
            } 
        }
        public double ArenaLong { 
            get
            {
                return arenaLong;
            }
            set
            {
                if (!validaLongitud(value)) throw new Exception("La Longitud ha d'estat entre -180 i 180");
                arenaLong = value;
            } 
        }

        public static bool validaText(String text)
        {
            return text.Length > 4;
        }

        public static bool validaShortCaption(String caption)
        {
            return caption.Length == 3;
        }
        
        public static bool validaDivision(Division division)
        {
            return division != null && division.Caption.Length > 4;
        }

        public static bool validaCapacity(int capacity)
        {
            return capacity >= 0;
        }

        public static bool validaLatitud(double latitud)
        {
            return latitud >= -90 && latitud <=90;
        }

        public static bool validaLongitud(double longitud)
        {
            return longitud >= -180 && longitud <= 180;
        }

    }
}
