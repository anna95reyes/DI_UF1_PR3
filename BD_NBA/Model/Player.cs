using System;
using System.Collections.Generic;
using System.Text;

namespace NBA_BD.Model
{
    public class Player
    {
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

        public int PlayerCurrentNumber { get => playerCurrentNumber; set => playerCurrentNumber = value; }
        public string PlayerFirstName { get => playerFirstName; set => playerFirstName = value; }
        public string PlayerLastName { get => playerLastName; set => playerLastName = value; }
        public byte[] PlayerPhoto { get => playerPhoto; set => playerPhoto = value; }
        public string CollageName { get => collageName; set => collageName = value; }
        public int PlayerCareerStartYear { get => playerCareerStartYear; set => playerCareerStartYear = value; }
        public string CountryName { get => countryName; set => countryName = value; }
        public string CountryShortName { get => countryShortName; set => countryShortName = value; }
        public int PlayerHeight { get => playerHeight; set => playerHeight = value; }
        public float PlayerWeight { get => playerWeight; set => playerWeight = value; }
        public DateTime PlayerBithday { get => playerBithday; set => playerBithday = value; }
        public String PlayerPosition { get => playerPosition; set => playerPosition = value; }
    }
}
