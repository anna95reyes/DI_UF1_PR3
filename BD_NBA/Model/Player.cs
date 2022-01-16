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

        int teamId;
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
        int playerPosition;

        public Player(int teamId, int playerId, int playerCurrentNumber, string playerFirstName, string playerLastName, byte[] playerPhoto, College collageName, int playerCareerStartYear, Country country, int playerHeight, float playerWeight, DateTime playerBithday, int playerPosition)
        {
            TeamId = teamId;
            PlayerId = playerId;
            PlayerCurrentNumber = playerCurrentNumber;
            PlayerFirstName = playerFirstName;
            PlayerLastName = playerLastName;
            PlayerPhoto = playerPhoto;
            College = collageName;
            PlayerCareerStartYear = playerCareerStartYear;
            Country = country;
            PlayerHeight = playerHeight;
            PlayerWeight = playerWeight;
            PlayerBithday = playerBithday;
            PlayerPosition = playerPosition;
        }

        public int TeamId
        {
            get
            {
                return teamId;
            }
            set
            {
                teamId = value;
            }
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
                if (!validaNumber(value)) throw new Exception("El numero ha de ser positiu");
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
                if (!validaText(value)) throw new Exception("El nom es obligatori");
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
                if (!validaText(value)) throw new Exception("El cognom es obligatori");
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
        public College College { 
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
                if (!validaYear(value)) throw new Exception("L'any ha d'estar entre 1900 i l'any actual");
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
                if (!validaHeight(value)) throw new Exception("Entre 130 i 250 cm");
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
                if (!validaWeight(value)) throw new Exception("Entre 40 i 200 cm");
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
        public int PlayerPosition { 
            get
            {
                return playerPosition;
            }
            set
            {
                if (!validaPosition(value)) throw new Exception("Entre 1 i 7");
                playerPosition = value;
            } 
        }

        public String getPlayerPosition(int playerPosition)
        {
            switch (playerPosition)
            {
                case 1: return "Center";
                case 2: return "Guard";
                case 3: return "Center, Guard";
                case 4: return "Forward";
                case 5: return "Center, Forward";
                case 6: return "Guard, Forward";
                case 7: return "Center, Guard, Forward";
                default: return "";
            }
        }

        public static bool validaText(String text)
        {
            return text.Length > 0;
        }

        public static bool validaNumber(int number)
        {
            return number >= 0;
        }

        public static bool validaYear(int year)
        {
            return year >= 1900 && year <= DateTime.Today.Year;
        }

        public static bool validaHeight(int height)
        {
            return height > 130 && height < 250;
        }

        public static bool validaWeight(float weight)
        {
            return weight > 40 && weight < 200;
        }

        public static bool validaPosition(int position)
        {
            return position > 0 && position < 8;
        }

        public override bool Equals(object obj)
        {
            return obj is Player player &&
                   TeamId == player.TeamId &&
                   PlayerId == player.PlayerId &&
                   PlayerCurrentNumber == player.PlayerCurrentNumber &&
                   PlayerFirstName == player.PlayerFirstName &&
                   PlayerLastName == player.PlayerLastName &&
                   EqualityComparer<byte[]>.Default.Equals(PlayerPhoto, player.PlayerPhoto) &&
                   EqualityComparer<College>.Default.Equals(College, player.College) &&
                   PlayerCareerStartYear == player.PlayerCareerStartYear &&
                   EqualityComparer<Country>.Default.Equals(Country, player.Country) &&
                   PlayerHeight == player.PlayerHeight &&
                   PlayerWeight == player.PlayerWeight &&
                   PlayerBithday == player.PlayerBithday &&
                   PlayerPosition == player.PlayerPosition;
        }

        public override int GetHashCode()
        {
            int hashCode = 1255786363;
            hashCode = hashCode * -1521134295 + TeamId.GetHashCode();
            hashCode = hashCode * -1521134295 + PlayerId.GetHashCode();
            hashCode = hashCode * -1521134295 + PlayerCurrentNumber.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PlayerFirstName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PlayerLastName);
            hashCode = hashCode * -1521134295 + EqualityComparer<byte[]>.Default.GetHashCode(PlayerPhoto);
            hashCode = hashCode * -1521134295 + EqualityComparer<College>.Default.GetHashCode(College);
            hashCode = hashCode * -1521134295 + PlayerCareerStartYear.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Country>.Default.GetHashCode(Country);
            hashCode = hashCode * -1521134295 + PlayerHeight.GetHashCode();
            hashCode = hashCode * -1521134295 + PlayerWeight.GetHashCode();
            hashCode = hashCode * -1521134295 + PlayerBithday.GetHashCode();
            hashCode = hashCode * -1521134295 + PlayerPosition.GetHashCode();
            return hashCode;
        }
    }
}
