using System;

namespace Parkheesung.Domain.Models
{
    public class SetupMember
    {
        public string Name { get; set; }

        public string NowPass { get; set; }

        public string NewPass { get; set; }

        public string NewPassConfirm { get; set; }

        public string UserPhotoURL { get; set; }

        public SetupMember()
        {
            this.Name = String.Empty;
            this.NowPass = String.Empty;
            this.NewPass = String.Empty;
            this.NewPassConfirm = String.Empty;
            this.UserPhotoURL = String.Empty;
        }
    }
}
