using System;

namespace PureCms.Core.Domain
{
    [Serializable]
    public class BaseEntity
    {
        [PetaPoco.Ignore]
        public object Id { get; set; }

        private DateTime _createdOn = DateTime.Now;
        public DateTime CreatedOn {
            get
            {
                return _createdOn;
            }
            set
            {
                _createdOn = value;
            }
        }
    }
}
