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

        int playerId;
        int playerCurrentNumber;
        String playerFirstName;
        String playerLastName;
        Byte[] playerPhoto;
        College collageName;
        int playerCareerStartYear;
        Country country;
        int playerHeight;
        float playerWeight;
        DateTime playerBithday;
        String playerPosition;

        public Player(int playerId, int playerCurrentNumber, string playerFirstName, string playerLastName, byte[] playerPhoto, College collageName, int playerCareerStartYear, Country country, int playerHeight, float playerWeight, DateTime playerBithday, String playerPosition)
        {
            PlayerId = playerId;
            PlayerCurrentNumber = playerCurrentNumber;
            PlayerFirstName = playerFirstName;
            PlayerLastName = playerLastName;
            PlayerPhoto = playerPhoto;
            CollageName = collageName;
            PlayerCareerStartYear = playerCareerStartYear;
            Country = country;
            PlayerHeight = playerHeight;
            PlayerWeight = playerWeight;
            PlayerBithday = playerBithday;
            PlayerPosition = playerPosition;
        }

        public int PlayerId {
            get
            {
                return playerId;
            }
            set
            {
                playerId = value;
            }
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
        public College CollageName { 
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
        public Country Country { 
            get
            {
                return country;
            }
            set
            {
                country = value;
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
