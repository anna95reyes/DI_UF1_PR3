using System;
using System.Collections.Generic;
using System.Text;

namespace NBA_BD.Model
{
    public class Division
    {
        private String caption;

        public Division(string caption)
        {
            Caption = caption;
        }

        public string Caption { get => caption; set => caption = value; }

        public override bool Equals(object obj)
        {
            return obj is Division division &&
                   Caption == division.Caption;
        }

        public override int GetHashCode()
        {
            return 287138253 + EqualityComparer<string>.Default.GetHashCode(Caption);
        }

        public override string ToString()
        {
            return Caption;
        }
    }
}
