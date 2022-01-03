using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace NBA_BD.Model
{
    public class Player : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        int playerCurrentNumber;
        String playerFirstName;
        String playerLastName;
        Byte[] playerPhoto;
        String collageName;
        int playerCareerStartYear;
        String countryName;
        String countryShortName;
        int playerHeight;
        float playerWeight;
        DateTime playerBithday;
        String playerPosition;

        public Player(int playerCurrentNumber, string playerFirstName, string playerLastName, byte[] playerPhoto, string collageName, int playerCareerStartYear, string countryName, string countryShortName, int playerHeight, float playerWeight, DateTime playerBithday, String playerPosition)
        {
            PlayerCurrentNumber = playerCurrentNumber;
            PlayerFirstName = playerFirstName;
            PlayerLastName = playerLastName;
            PlayerPhoto = playerPhoto;
            CollageName = collageName;
            PlayerCareerStartYear = playerCareerStartYear;
            CountryName = countryName;
            CountryShortName = countryShortName;
            PlayerHeight = playerHeight;
            PlayerWeight = playerWeight;
            PlayerBithday = playerBithday;
            PlayerPosition = playerPosition;
        }

        public int PlayerCurrentNumber {
            get
            {
                return playerCurrentNumber;
            }
            set
            {
                playerCurrentNumber = value;
            }
        }
        public string PlayerFirstName {
            get
            {
                return playerFirstName;
            }
            set
            {
                playerFirstName = value;
            }
        }
        public string PlayerLastName { 
            get
            {
                return playerLastName;
            }
            set
            {
                playerLastName = value;
            } 
        }
        public byte[] PlayerPhoto { 
            get
            {
                return playerPhoto;
            }
            set
            {
                playerPhoto = value;
            } 
        }
        public string CollageName { 
            get
            {
                return collageName;
            }
            set
            {
                collageName = value;
            } 
        }
        public int PlayerCareerStartYear { 
            get
            {
                return playerCareerStartYear;
            }
            set
            {
                playerCareerStartYear = value;
            } 
        }
        public string CountryName { 
            get
            {
                return countryName;
            }
            set
            {
                countryName = value;
            } 
        }
        public string CountryShortName { 
            get
            {
                return countryShortName;
            }
            set
            {
                countryShortName = value;
            } 
        }
        public int PlayerHeight { 
            get
            {
                return playerHeight;
            }
            set
            {
                playerHeight = value;
            } 
        }
        public float PlayerWeight { 
            get
            {
                return playerWeight;
            }
            set
            {
                playerWeight = value;
            } 
        }
        public DateTime PlayerBithday { 
            get
            {
                return playerBithday;
            }
            set
            {
                playerBithday = value;
            } 
        }
        public String PlayerPosition { 
            get
            {
                return playerPosition;
            }
            set
            {
                playerPosition = value;
            } 
        }
    }
}
