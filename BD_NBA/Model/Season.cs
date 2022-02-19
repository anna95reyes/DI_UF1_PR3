using System;
using System.Collections.Generic;
using System.Text;

namespace NBA_BD.Model
{
    public class Season
    {
        private String year;
        private String caption;

        public Season(string year, string caption)
        {
            Year = year;
            Caption = caption;
        }

        public string Year { get => year; set => year = value; }
        public string Caption { get => caption; set => caption = value; }

        public override bool Equals(object obj)
        {
            return obj is Season season &&
                   Year == season.Year &&
                   Caption == season.Caption;
        }

        public override int GetHashCode()
        {
            int hashCode = -2120060511;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Year);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Caption);
            return hashCode;
        }

        public override string ToString()
        {
            return Caption;
        }
    }
}
