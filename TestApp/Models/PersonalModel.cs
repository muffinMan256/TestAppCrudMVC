﻿namespace TestApp.Models
{
    public class PersonalModel
    {
        public Guid UserId { get; set; }

        public string FullName { get; set; }

        public string Nume { get; set; }
        public string Prenume { get; set; }

        public bool Valid { get; set; }

        public DateTime? Birthday { get; set; }
    }
}
