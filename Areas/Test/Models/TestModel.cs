using Microsoft.AspNetCore.Mvc.Rendering;
using MvcPractice.Models;

namespace MvcPractice.Areas.Test.Models
{
    public class TestModel : DataTablesServerSide
    {
        public Strings Strings = new("test", "Test");
        //public List</*RecordName*/> RecordsList { get; set; }
        //public /*RecordName*/? Record { get; set; }
        public string Type { get; set; }
        //SELECT LISTS
        //public SelectList /*SelectListName*/SelectList { get; set; }
    }    
}