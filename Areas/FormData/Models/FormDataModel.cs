namespace MvcPractice.Areas.FormData.Models
{
    public class FormDataModel
    {
        public string FormData1String { get; set; }
        public int FormData1Number { get; set; }
        public List<int> FormData1List { get; set; }

        public string FormData2String { get; set; }
        public int FormData2Number { get; set; }
        public List<int> FormData2List { get; set; }

        public string FormData3String { get; set; }
        public int FormData3Number { get; set; }
        public List<int> FormData3List { get; set; }

        public string FormData4String { get; set; }
        public int FormData4Number { get; set; }
        public List<int> FormData4List { get; set; }

        public string FormData5String { get; set; }
        public int FormData5Number { get; set; }
        public List<int> FormData5List { get; set; }

        
        public class Student
        {
            public int StudentID { get; set; }
            public string StudentName { get; set; }
            public int Age { get; set; }
        }

    }
}