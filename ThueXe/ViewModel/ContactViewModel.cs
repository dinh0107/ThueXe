using ThueXe.Models;
using System.Collections.Generic;

namespace ThueXe.ViewModel
{
    public class ListContactViewModel
    {
        public PagedList.IPagedList<Contact> Contacts { get; set; }
        public string Name { get; set; }
    }
}
