using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace k180211_Q1
{
    class RecordsInfo
    {
        private string email;
        private string password;
        private string campus_id;

        public RecordsInfo()
        {

        }
        public RecordsInfo(string email, string password, string campus_id)
        {
            this.email = email;
            this.password = password;
            this.campus_id = campus_id;
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        public string Campus_id
        {
            get { return campus_id; }
            set { campus_id = value; }
        }
    }
}
